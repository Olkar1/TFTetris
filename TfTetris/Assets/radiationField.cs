using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RadiationField", menuName = "ScriptableObjects/RadiationField", order = 1)]
public class radiationField : ScriptableObject {
    public List<Vector2> affectedSqueres;
    public GameObject visualization;
}
