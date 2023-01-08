using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
public class GridForTesting : MonoBehaviour
{
    private GridManager grid;

    private Field currentPointerField;
    private Field currentFieldToModificate;

    public BoardScenerio testScenerio;

    public Transform specialObjectParent;
    public SpecialObject dmgObject;
    public SpecialObject healObject;
    [HideInInspector]
    public string newScenerioName;
    private void Awake() {
        grid = GetComponent<GridManager>();
    }
    private void Start() {
        StartCoroutine(grid.SpawnGrid());
        grid.testGrid = true;
    }
    private void Update() {
        currentPointerField = grid.GetFieldByPointerPosition(Pointer.pointerPosition);
        if (Input.GetMouseButtonDown(0) && currentPointerField != null) {
            currentFieldToModificate = currentPointerField;
            foreach (var field in grid.GetSortedFields()) {
                field.ActiveOutline(false);
            }
            currentFieldToModificate.ActiveOutline(true);
        }
    }
    public void SpawnScenrio() {
        StartCoroutine(LoadBoardScenerio());
    }
    private IEnumerator LoadBoardScenerio() {
        if (!testScenerio) {
            yield break;
        }
        yield return ClearSpecialObjects();
        foreach (var special in testScenerio.scenerioObjects) {
            for (int i = 0; i < special.positions.Count; i++) {
                SpecialObject specialObject = Instantiate(special.objectToSpawn);
                specialObject.transform.SetParent(specialObjectParent);
                GridManager.instance.GetFieldByIndex((int)special.positions[i].x, (int)special.positions[i].y).SetSpecialObject(specialObject);
            }
        }
    }
    private IEnumerator ClearSpecialObjects() {
        for (int specialObjIndex = 0; specialObjIndex < specialObjectParent.childCount; specialObjIndex++) {
            Destroy(specialObjectParent.GetChild(specialObjIndex).gameObject);
        }
        yield break;
    }
    public void SpawnDmgAtSelected() {
        StartCoroutine(Spawn(true));
    }
    public void SpawnHealAtSelected() {
        StartCoroutine(Spawn(false));
    }
    public void ClearField() {
        SpecialObject currentObject = currentFieldToModificate.GetSpecialObject();
        if (currentObject) {
            Destroy(currentObject.gameObject);
        }

    }
    private IEnumerator Spawn(bool dmg) {
        SpecialObject currentObject = currentFieldToModificate.GetSpecialObject();
        yield return DestroyObject(currentObject);
 
        SpecialObject specialObject = dmg ? Instantiate(dmgObject): Instantiate(healObject);

        specialObject.transform.SetParent(specialObjectParent);
        currentFieldToModificate.SetSpecialObject(specialObject);
    }
    private IEnumerator DestroyObject(SpecialObject currentObject) {
        if (currentObject) {
            Destroy(currentObject.gameObject);
        }
        yield break;
    }
    public void SaveScenerio() {
        testScenerio.UpdateScenerio(GetCurrentScenerio());
    }
    public void CreateNewScenerio(string assetName) {
        BoardScenerio newScenerio = ScriptableObject.CreateInstance<BoardScenerio>();
        AssetDatabase.CreateAsset(newScenerio, "Assets/Scenerios/" + assetName + ".asset");

        newScenerio.UpdateScenerio(GetCurrentScenerio());
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = newScenerio;
    }
    private List<Scenerio> GetCurrentScenerio() {
        Scenerio dmgScenerio = new Scenerio();
        dmgScenerio.positions = new List<Vector2>();
        dmgScenerio.objectToSpawn = dmgObject;
        Scenerio healScenerio = new Scenerio();
        healScenerio.positions = new List<Vector2>();
        healScenerio.objectToSpawn = healObject;

        List<Field> fields = grid.GetSortedFields();
        foreach (var field in fields) {
            SpecialObject specialObject = field.GetSpecialObject();
            if (specialObject) {
                if (specialObject.splecialEffectStatus == SpecialObject.SplecialEffectStatus.dmg) {
                    dmgScenerio.positions.Add(field.coordinates);
                }
                else {
                    healScenerio.positions.Add(field.coordinates);
                }
            }
        }
        return new List<Scenerio>{ dmgScenerio,healScenerio };
    }

}
#endif