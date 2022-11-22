using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public bool isHold = false;
    public Field positionField;
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
        }
    }
}
