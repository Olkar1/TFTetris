using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{

    public Vector2 coordinates;
    private Vector2 worldCoordinates;

    [SerializeField] private Mesh blackFieldMesh;
    [SerializeField] private Mesh whiteFieldMesh;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform modelPosition;
    private float meshSize;

    public void CreateField(int column, int row, bool white) {
        meshFilter.mesh = white ? whiteFieldMesh : blackFieldMesh;
        coordinates = new Vector2(row, column);
        MoveMeshToStartCorner();
    }
    private void MoveMeshToStartCorner() {
        meshSize = meshRenderer.bounds.size.x;
        modelPosition.localPosition = new Vector3(meshSize,0,0);
    }
}
