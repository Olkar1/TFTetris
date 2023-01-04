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
                transform.position = currentField.middlePos;
                currentField.SetMonster(this);
                currentPositionField = currentField;
                isHold = false;
                Pointer.hold = false;
                animator.SetBool("idle", true);
            }
            else {
                Debug.LogWarning("Wrong place");
            }
        }
    }
    public IEnumerator MoveMonster() {
        Field nextField = GridManager.instance.GetUpFrontField(currentPositionField);

        while (nextField) {
            MovementModificationObject currentFieldModification = currentPositionField.GetMovementModificationObject();
            if (currentFieldModification && 
                currentFieldModification.modificationType == MovementModificationObject.ModificationType.JumpOver &&
               !nextField.IsEmpty()) {
                nextField = GridManager.instance.GetNextEmptyField(nextField);
                if (!nextField) {
                    break;
                }
                else {
                    ///MoveToNextPosition
                    //SetMonsterPosition(nextField);
                    yield return MoveMonsterToPosition(nextField,true);
                    nextField = GetNextField();
                }
            }
            else if (nextField.IsEmpty()) {
                ///MoveToNextPosition
                yield return MoveMonsterToPosition(nextField,false);
                //SetMonsterPosition(nextField);
                nextField = GetNextField();
            }
            else {
                break;
            }
            //yield return new WaitForSeconds(0.5f);
        }
        animator.SetBool("idle", true);
        currentPositionField.SetMonster(this);
        SpawnAttack();
    }

    private Field GetNextField() {
        Field upfrontField = GridManager.instance.GetUpFrontField(currentPositionField);
        return upfrontField;
    }

    private IEnumerator MoveMonsterToPosition(Field position,bool flyOver) {
        if (!flyOver) {
            animator.SetBool("idle", false);
        }
        else {
            transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
        }
        currentPositionField.SetMonster(null);
        bool moveing = true;
        while (moveing) {
            transform.position = Vector3.MoveTowards(transform.position,position.middlePos,moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position,position.middlePos)<0.01f) {
                moveing = false;
            }
            yield return new WaitForEndOfFrame();
        }
        currentPositionField = position;
    }

    private void SpawnAttack() {
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
                //Destroy(spawnField.GetSpecialObject().gameObject);
            }
            var attack = Instantiate(attackVisual, positionToSpawn,Quaternion.identity);
            attack.transform.SetParent(attackParent);
        }
    }
}
