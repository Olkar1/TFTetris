using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectOnField : MonoBehaviour
{
    public Field currentPositionField;
    public bool isHold = false;

    public abstract void PutObjectOnField(Field fieldToPut);

    private void Update() {
        if (!isHold) { return; }
        GlueToPointerAndSetCurrentPositionField();
        PutObjectOnField(currentPositionField);
    }
    private void GlueToPointerAndSetCurrentPositionField() {
        if (isHold) {
            transform.position = Pointer.pointerPosition;
            currentPositionField = GridManager.instance.GetCurrentActiveField();
        }
    }
}
