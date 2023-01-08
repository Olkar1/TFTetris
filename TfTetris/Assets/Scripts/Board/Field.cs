using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    /// <summary>
    /// ORIGIN TRANSFORM IS IN THE LEFT DOWN CORNER
    /// AXIS swaped in inspector wtf why! TODO
    /// </summary>
    [HideInInspector]
    public Vector3 leftUpperCorner;
    [HideInInspector]
    public Vector3 leftDownCorner;
    [HideInInspector]
    public Vector3 rightUpperCorner;
    [HideInInspector]
    public Vector3 rightDownCorner;
    [HideInInspector]
    public Vector3 middlePos;
    [HideInInspector]
    public Vector2 coordinates;
    [HideInInspector]
    public Vector3 startPos;

    public int column;
    public int row;

    [SerializeField] private float spawnTime;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float spawnHeight = 2f;

    private float meshSize;

    private Monster currentMosnter;
    private MovementModificationObject movementObject;
    private SpecialObject enemyAttack;
    public bool scored = false;

    [SerializeField] private ParticleSystem scoredVFX;
    [SerializeField] public VisualEffectController visualEffectController;
    [SerializeField] private Color scoredInitColor;
    [SerializeField] private Color doubleScoreColor;

    [SerializeField] private TakenFieldVfx conqueredVFX;

    [SerializeField] private ParticleSystem spawnVFX;

    [SerializeField] private GameObject outline;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Mesh blackFieldMesh;
    [SerializeField] private Mesh whiteFieldMesh;

    private void Start() {
        scoredInitColor = scoredVFX.main.startColor.color;
        visualEffectController.StopEffect();
    }
    public void CreateField(int column, int row, bool white) {
        meshFilter.mesh = white ? whiteFieldMesh : blackFieldMesh;
        coordinates = new Vector2(row, column);
        MoveMeshToStartCorner();
        SetCornersPosition();
        startPos = transform.position;
        visualEffectController.SetInitPosition();
        StartCoroutine(SpawnAnim());
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
    public IEnumerator SpawnAnim() {
        float currentAnimTime = 0f;
        while (currentAnimTime < spawnTime) {
            float fieldYPos = animationCurve.Evaluate(1- currentAnimTime / spawnTime);
            transform.position = new Vector3(startPos.x, startPos.y + spawnHeight * fieldYPos, startPos.z);
            currentAnimTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.position = startPos;
        spawnVFX.Play();
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
        specialObject.worldPosition = middlePos;
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
    public void SetToScored(Color colorOfConquer) {
        scored = true;
        scoredVFX.Play();
        conqueredVFX.ChangeParticleColor(colorOfConquer);
        conqueredVFX.PlayEffect();
    }
    public void ChangeScoredColor() {
        var main = scoredVFX.main;
        main.startColor = doubleScoreColor;
    }
    public void ClearField() {
        scoredVFX.Stop();
        visualEffectController.StopEffect();
        var main = scoredVFX.main;
        main.startColor = scoredInitColor;
        scored = false;
        SetMonster(null);
        SetMovementObject(null);
    }
    public IEnumerator MoveOrbToStatue() {
        yield return new WaitForSeconds(0.5f);
        Transform orbTransform = visualEffectController.transform;
        Vector3 orbPosition = visualEffectController.transform.position;
        Vector3 enemyStatuePosition = GameManager.instance.enemyStatue.eyeTransform.position;
        while (Vector3.Distance(orbPosition, enemyStatuePosition) > 0.1f) {
            orbTransform.position = Vector3.Lerp(orbTransform.position, enemyStatuePosition, 0.1f);
            yield return new WaitForSeconds(Time.deltaTime);

            if (Vector3.Distance(orbTransform.position, enemyStatuePosition) < 0.15f) {
                break;
            }
        }

        visualEffectController.StopEffect();
        visualEffectController.ResetPosition();
        yield break;

    }
    public bool IsEmpty() {
        bool empty = (currentMosnter == null && 
                         enemyAttack == null && 
                     (movementObject == null || movementObject.modificationType == MovementModificationObject.ModificationType.JumpOver));

        return empty;
    }
}
