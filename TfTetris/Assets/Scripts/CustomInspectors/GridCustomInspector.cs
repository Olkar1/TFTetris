using UnityEngine;
using UnityEditor;
using System.Collections;

#if UNITY_EDITOR
[CustomEditor(typeof(GridForTesting))]
public class GridCustomInspector : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        GUIStyle headlineStyle = new GUIStyle();
        headlineStyle.fontStyle = FontStyle.Bold;
        headlineStyle.fontSize = 18;
        headlineStyle.normal.textColor = Color.white;
        headlineStyle.margin.top = 20;
        headlineStyle.alignment = TextAnchor.MiddleCenter;

        GridForTesting gridForTesting = (GridForTesting)target;

        GUILayout.Label("Scenerio", headlineStyle);
        gridForTesting.newScenerioName = EditorGUILayout.TextField(gridForTesting.newScenerioName);
        if (GUILayout.Button("Load Scenerio")) {
            gridForTesting.SpawnScenrio();
        }
        if (GUILayout.Button("Modify Scenerio")) {
            gridForTesting.SaveScenerio();
        }
        if (GUILayout.Button("Save as new Scenerio")) {
            gridForTesting.CreateNewScenerio(gridForTesting.newScenerioName);
        }

        GUILayout.Label("Field", headlineStyle);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Heal")) {
            gridForTesting.SpawnHealAtSelected();
        }
        if (GUILayout.Button("Add Dmg")) {
            gridForTesting.SpawnDmgAtSelected();
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("ClearField")) {
            gridForTesting.ClearField();
        }
    }
}
#endif