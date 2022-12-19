using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BoardScenerio", menuName = "BoardScenerio/NewScenerio", order = 1)]
public class BoardScenerio : ScriptableObject {
    public List<Scenerio> scenerioObjects;
}
[Serializable]
public struct Scenerio {
    public SpecialObject objectToSpawn;
    public List<Vector2> positions;
}
