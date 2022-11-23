using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public bool isHold = false;
    public Field positionField;
    public radiationField radiationField;
    bool reachEnd = false;
    bool move = false

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHold) {
            transform.position = Pointer.pointerPosition;
            Field currentField = GridManager.instance.currentActiveField;
            if (Input.GetMouseButton(0) && currentField && currentField.empty) {
                transform.position = currentField.middlePos;
                currentField.empty = false;
                positionField = currentField;
                Debug.LogError(positionField.name);
                isHold = false;
            }
        }
        if (GameManager.gameStatus == GameManager.GameStatus.fighting) {
            Field upfrontField = GridManager.instance.GetUpFrontField(positionField);
            if (upfrontField && upfrontField.empty) {
                positionField.empty = true;
                positionField = upfrontField;
                positionField.empty = false;
                transform.position = positionField.middlePos;
            }
            else {
                if (!reachEnd) {
                    SpawnAttack();
                    reachEnd = true;
                }
            }
        }
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
