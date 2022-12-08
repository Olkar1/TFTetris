using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectOnField : MonoBehaviour
{
    public Field positionField;
    public bool isHold = false;

    public abstract void PutObjectOnField(Field fieldToPut);

    private void Update() {
        if (!isHold) { return; }
        GlueToPointerAndSetCurrentPositionField();
        PutObjectOnField(positionField);
    }
    private void GlueToPointerAndSetCurrentPositionField() {
        if (isHold) {
            transform.position = Pointer.pointerPosition;
            positionField = GridManager.instance.GetCurrentActiveField();
        }
    }
}
