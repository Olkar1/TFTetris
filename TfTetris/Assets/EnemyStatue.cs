using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatue : MonoBehaviour
{
    [SerializeField] private Light light;
    [SerializeField] private Color initColor;
    [SerializeField] private float topIntensity;
    [SerializeField] private float bottomIntensity;
    [SerializeField] private float changeSpeed;
    public Transform eyeTransform;
    private bool increesing = true;
    private void Awake() {
        light.color = initColor;
    }
    void Update()
    {
        ChangeLightIntensity();
    }
    private void ChangeLightIntensity() {
        float lightIntencity = light.intensity;
        if (lightIntencity > topIntensity) {
            increesing = false;
        }
        else if(lightIntencity < bottomIntensity) {
            increesing = true;
        }
        if (increesing) {
            light.intensity += Time.deltaTime * changeSpeed;
        }
        else {
            light.intensity -= Time.deltaTime * changeSpeed;
        }
    }

}
