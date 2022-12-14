using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModificationObject : ObjectOnField
{
    public ModificationType modificationType;
    public enum ModificationType {
        Stop,
        JumpOver
    }
    public override void PutObjectOnField(Field currentField) {
        if (Input.GetMouseButton(0)) {
            if (currentField && currentField.IsEmpty()) {
                transform.position = currentField.middlePos;
                currentField.SetMovementObject(this);
                currentPositionField = currentField;
                isHold = false;
                Pointer.hold = false;
            }
            else {
                Debug.LogWarning("Wrong place");
            }
        }
    }
}
