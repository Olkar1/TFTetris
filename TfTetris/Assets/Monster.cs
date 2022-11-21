using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public bool isHold = false;
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
                isHold = false;
            }
        }   
    }
}
