using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectController : MonoBehaviour
{
    [SerializeField] private VisualEffect specialEffect;
    [SerializeField] private float initSize;
    [SerializeField] private float animTime;

    private void Start() {
        initSize = specialEffect.GetFloat("Size");
    }
    public void StopEffect() {
        specialEffect.Stop();
    }
    public void PlayEffect(Color effectColor) {
        StartCoroutine(ChangeSize());
        specialEffect.SetVector4("Color", effectColor);
        specialEffect.Play();
    }
    
    private IEnumerator ChangeSize() {
        float currrentAnimTime = 0f;

        while (currrentAnimTime<animTime) {
            float size = initSize * currrentAnimTime/animTime;
            specialEffect.SetFloat("Size", size);

            currrentAnimTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }
}
