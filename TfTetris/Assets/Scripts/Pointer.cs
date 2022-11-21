using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public static Vector3 pointerPosition;
    [SerializeField] private LayerMask pointerLayer;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out RaycastHit raycastHit, float.MaxValue,pointerLayer)) {
            pointerPosition = raycastHit.point;
        }
    }
}
