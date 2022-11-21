using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{

    public Vector2 coordinates;
    private Vector2 worldCoordinates;
    /// <summary>
    /// ORIGIN TRANSFORM IS IN THE LEFT DOWN CORNER
    /// AXIS swaped in inspector wtf why! TODO
    /// </summary>
    public Vector3 leftUpperCorner;
    public Vector3 leftDownCorner;
    public Vector3 rightUpperCorner;
    public Vector3 rightDownCorner;
    public Vector3 middlePos;

    [SerializeField] private Mesh blackFieldMesh;
    [SerializeField] private Mesh whiteFieldMesh;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform modelPosition;
    [SerializeField] private GameObject outline;
    private float meshSize;

    public bool empty = true;
    public void CreateField(int column, int row, bool white) {
        meshFilter.mesh = white ? whiteFieldMesh : blackFieldMesh;
        coordinates = new Vector2(row, column);
        MoveMeshToStartCorner();
        SetCornersPosition();
    }
    private void MoveMeshToStartCorner() {
        meshSize = meshRenderer.bounds.size.x;
        modelPosition.localPosition = new Vector3(meshSize,0,0);
    }
    private void SetCornersPosition() {
        leftUpperCorner = transform.position + new Vector3(0, 0, (int)meshSize);
        rightUpperCorner = transform.position + new Vector3((int)meshSize, 0, (int)meshSize);
        leftDownCorner = new Vector3((int)transform.position.x, 0, (int)transform.position.z);
        rightDownCorner = transform.position + new Vector3((int)meshSize, 0, 0);
        middlePos = transform.position + new Vector3((int)meshSize/2,0, (int)meshSize/2);
    }
    public void ActiveOutline(bool active) {
        if(active && !outline.gameObject.activeSelf) {
            outline.gameObject.SetActive(active);
        }
        else if (outline.gameObject.activeSelf) {
            outline.gameObject.SetActive(active);
        }
    }
}
