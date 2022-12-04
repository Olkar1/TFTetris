using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public bool isHold = false;
    public Field positionField;
    public radiationField radiationField;
    public Transform attackParent;

    void Update()
    {
        if (!isHold) { return; }
        GlueToPointerAndSetCurrentPositionField();
        PutMonsterOnField(positionField);
    }
    private void GlueToPointerAndSetCurrentPositionField() {
        if (isHold) {
            transform.position = Pointer.pointerPosition;
            positionField = GridManager.instance.GetCurrentActiveField();
        }
    }
    private void PutMonsterOnField(Field currentField) {
        if (Input.GetMouseButton(0)) {
            if (currentField && currentField.IsEmpty()) {
                transform.position = currentField.middlePos;
                currentField.SetMonster(this);
                positionField = currentField;
                isHold = false;
                Pointer.hold = false;
            }
            else {
                Debug.LogWarning("Wrong place");
            }
        }
    }

    public IEnumerator MoveMonster() {
        Field upfrontField = GridManager.instance.GetUpFrontField(positionField);
        while (upfrontField && upfrontField.IsEmpty()) {
            positionField.SetMonster(null);
            positionField = upfrontField;
            transform.position = positionField.middlePos;
            upfrontField = GridManager.instance.GetUpFrontField(positionField);
            yield return new WaitForSeconds(0.5f);
        }
        positionField.SetMonster(this);
        SpawnAttack();
    }
    private void SpawnAttack() {
        GameObject attackVisual = radiationField.visualization;
        foreach (Vector2 attackIndex in radiationField.affectedSqueres) {
            Vector2 currentPositionIndex = GridManager.instance.GetIndexByField(positionField);
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
