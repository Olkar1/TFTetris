using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "BoardScenerio", menuName = "BoardScenerio/NewScenerio", order = 1)]
public class BoardScenerio : ScriptableObject {
    public List<Scenerio> scenerioObjects;
#if UNITY_EDITOR
    public void UpdateScenerio(List<Scenerio> newScenerios) {
        EditorUtility.SetDirty(this);
        scenerioObjects = newScenerios;
    }
#endif
}
[Serializable]
public struct Scenerio {
    public SpecialObject objectToSpawn;
    public List<Vector2> positions;
}
