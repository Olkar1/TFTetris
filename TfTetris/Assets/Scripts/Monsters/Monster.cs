using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : ObjectOnField {

    public radiationField radiationField;
    public Transform attackParent;

    public float moveSpeed;

    [SerializeField] private Animator animator;

    public override void PutObjectOnField(Field currentField) {
        if (Input.GetMouseButton(0)) {
            if (currentField && currentField.IsEmpty()) {
                PutMonsterOnField(currentField);
                Pointer.hold = false;
            }
            else {
                Debug.LogWarning("Wrong place");
            }
        }
    }
    private void PutMonsterOnField(Field field) {
        transform.position = field.middlePos;
        field.SetMonster(this);
        currentPositionField = field;
        isHold = false;

        animator.SetBool("idle", true);
    }
    public IEnumerator MoveMonster() {
        Field nextField = GridManager.instance.GetUpFrontField(currentPositionField);
        while (nextField) {
            MovementModificationObject currentFieldModification = currentPositionField.GetMovementModificationObject();
            if (IsJumpOverField(currentFieldModification,nextField)) {
                nextField = GridManager.instance.GetNextEmptyField(nextField);
                if (!nextField) {
                    break;
                }
                else {
                    yield return MoveMonsterToPosition(nextField,true);
                    nextField = GetNextField();
                }
            }
            else if (nextField.IsEmpty()) {
                yield return MoveMonsterToPosition(nextField,false);
                nextField = GetNextField();
            }
            else {
                break;
            }
        }
        SetMonsterToPosition();
        SpawnAttack();
    }
    private bool IsJumpOverField(MovementModificationObject currentFieldModification, Field nextField) {
        bool isJumpOverField = currentFieldModification &&
                currentFieldModification.modificationType == MovementModificationObject.ModificationType.JumpOver &&
               !nextField.IsEmpty();
        return isJumpOverField;
    }
    private Field GetNextField() {
        Field upfrontField = GridManager.instance.GetUpFrontField(currentPositionField);
        return upfrontField;
    }
    private void SetMonsterToPosition() {
        animator.SetBool("idle", true);
        currentPositionField.SetMonster(this);
    }
    private IEnumerator MoveMonsterToPosition(Field position,bool flyOver) {
        if (!flyOver) {
            animator.SetBool("idle", false);
        }
        else {
            transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
        }
        currentPositionField.SetMonster(null);
        bool moving = true;
        while (moving) {
            transform.position = Vector3.MoveTowards(transform.position,position.middlePos,moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position,position.middlePos)<0.01f) {
                moving = false;
            }
            yield return new WaitForEndOfFrame();
        }
        currentPositionField = position;
    }
    private void SpawnAttack() {
        ///TO REFACTOR
        GameObject attackVisual = radiationField.visualization;
        foreach (Vector2 attackIndex in radiationField.affectedSqueres) {
            Vector2 currentPositionIndex = GridManager.instance.GetIndexByField(currentPositionField);
            Field spawnField = GridManager.instance.GetFieldByIndex((int)currentPositionIndex.x + (int)attackIndex.x,
                (int)currentPositionIndex.y + (int)attackIndex.y);
            if (!spawnField) { continue; }
            Vector3 positionToSpawn = spawnField.middlePos;

            spawnField.SetToScored();
            if (spawnField.GetSpecialObject()) {
                spawnField.GetSpecialObject().DestroyObject();
            }
            var attack = Instantiate(attackVisual, positionToSpawn,Quaternion.identity);
            attack.transform.SetParent(attackParent);
        }
    }
}
