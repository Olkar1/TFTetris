using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    /// <summary>
    /// ORIGIN TRANSFORM IS IN THE LEFT DOWN CORNER
    /// AXIS swaped in inspector wtf why! TODO
    /// </summary>
    public Vector3 leftUpperCorner;
    public Vector3 leftDownCorner;
    public Vector3 rightUpperCorner;
    public Vector3 rightDownCorner;
    public Vector3 middlePos;
    public Vector2 coordinates;

    public int column;
    public int row;

    private float meshSize;

    [SerializeField] private Monster currentMosnter;
    [SerializeField] private MovementModificationObject movementObject;
    [SerializeField] private SpecialObject enemyAttack;
    public bool scored = false;

    [SerializeField] private ParticleSystem scoredVFX;
    [SerializeField] private GameObject outline;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Mesh blackFieldMesh;
    [SerializeField] private Mesh whiteFieldMesh;

    public void CreateField(int column, int row, bool white) {
        meshFilter.mesh = white ? whiteFieldMesh : blackFieldMesh;
        coordinates = new Vector2(row, column);
        MoveMeshToStartCorner();
        SetCornersPosition();
    }
    private void MoveMeshToStartCorner() {
        meshSize = meshRenderer.bounds.size.x;
        modelTransform.localPosition = new Vector3(meshSize,0,0);
    }
    private void SetCornersPosition() {
        leftUpperCorner = transform.position + new Vector3(0, 0, meshSize);
        rightUpperCorner = transform.position + new Vector3(meshSize, 0, meshSize);
        leftDownCorner = new Vector3(transform.position.x, 0,transform.position.z);
        rightDownCorner = transform.position + new Vector3(meshSize, 0, 0);
        middlePos = transform.position + new Vector3(meshSize/2,0, meshSize/2);
    }
    public void ActiveOutline(bool active) {///CAN BE BETTER
        if(active && !outline.gameObject.activeSelf) {
            outline.gameObject.SetActive(active);
        }
        else if (outline.gameObject.activeSelf) {
            outline.gameObject.SetActive(active);
        }
    }
    public void SetMonster(Monster monster) {
        currentMosnter = monster;
    }
    public void SetMovementObject(MovementModificationObject movementModificationObject) {
        movementObject = movementModificationObject;
    }
    public MovementModificationObject GetMovementModificationObject() {
        return movementObject;
    }
    public void SetSpecialObject(SpecialObject specialObject) {
        if(!IsEmpty()) {
            Destroy(specialObject.gameObject);
            return;
        }
        specialObject.transform.position = middlePos;
        specialObject.position = new Vector2(column, row);
        this.enemyAttack = specialObject;
    }
    public float GetModelSize()
    {
        float size = meshFilter.sharedMesh.bounds.size.x;
        return size;
    }
    public SpecialObject GetSpecialObject() {
        return enemyAttack;
    }
    public Monster GetMonster() {
        return currentMosnter;
    }
    public void SetToScored() {
        scored = true;
        scoredVFX.Play();
    }
    public void ClearField() {
        scoredVFX.Stop();
        scored = false;
        SetMonster(null);
        SetMovementObject(null);
    }
    public bool IsEmpty() {
        bool empty = (currentMosnter == null && 
                         enemyAttack == null && 
                     (movementObject == null || movementObject.modificationType == MovementModificationObject.ModificationType.JumpOver));

        return empty;
    }
}
