using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : ObjectOnField {
    public radiationField radiationField;
    public Transform attackParent;

    public override void PutObjectOnField(Field currentField) {
        if (Input.GetMouseButton(0)) {
            if (currentField && currentField.IsEmpty()) {
                transform.position = currentField.middlePos;
                currentField.SetMonster(this);
                currentPositionField = currentField;
                isHold = false;
                Pointer.hold = false;
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
                Debug.LogError("Sprawdz nastepne pole");
                nextField = GridManager.instance.GetNextEmptyField(nextField);
                if (!nextField) {
                    break;
                }
                else {
                    Debug.LogError("Next empty field: " + nextField);
                    SetMonsterPosition(nextField);
                    nextField = GetNextField();
                }
            }
            else if (nextField.IsEmpty()) {
                Debug.LogError("stan na nastepne pole");
                SetMonsterPosition(nextField);
                nextField = GetNextField();
            }
            else {
                Debug.LogError("brak wyboru");
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
        currentPositionField.SetMonster(this);
        SpawnAttack();
    }

    private Field GetNextField() {
        Field upfrontField = GridManager.instance.GetUpFrontField(currentPositionField);
        return upfrontField;
    }

    private void SetMonsterPosition(Field positionToSet) {
        currentPositionField.SetMonster(null);
        currentPositionField = positionToSet;
        transform.position = currentPositionField.middlePos;
    }

    private void SpawnAttack() {
        GameObject attackVisual = radiationField.visualization;
        foreach (Vector2 attackIndex in radiationField.affectedSqueres) {
            Vector2 currentPositionIndex = GridManager.instance.GetIndexByField(currentPositionField);
            Field spawnField = GridManager.instance.GetFieldByIndex((int)currentPositionIndex.x + (int)attackIndex.x,
                (int)currentPositionIndex.y + (int)attackIndex.y);
            if (!spawnField) { continue; }
            Vector3 positionToSpawn = spawnField.middlePos;

            spawnField.scored = true;
            if (spawnField.GetSpecialObject()) {
                Destroy(spawnField.GetSpecialObject().gameObject);
            }
            var attack = Instantiate(attackVisual, positionToSpawn,Quaternion.identity);
            attack.transform.SetParent(attackParent);
        }
    }
}
