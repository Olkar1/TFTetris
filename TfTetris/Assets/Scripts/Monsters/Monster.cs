using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public bool isHold = false;
    public Field positionField;
    public radiationField radiationField;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHold) {
            transform.position = Pointer.pointerPosition;
            Field currentField = GridManager.instance.currentActiveField;
            if (Input.GetMouseButton(0) && currentField && !currentField.GetMonster()) {
                transform.position = currentField.middlePos;
                currentField.SetMonster(this);
                positionField = currentField;
                Debug.LogError(positionField.name);
                isHold = false;
            }
        }
    }
    public IEnumerator MoveMonster() {
        Field upfrontField = GridManager.instance.GetUpFrontField(positionField);
        while (upfrontField && !upfrontField.GetMonster()) {
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
            
            Instantiate(attackVisual, positionToSpawn,Quaternion.identity);
        }
    }
}
