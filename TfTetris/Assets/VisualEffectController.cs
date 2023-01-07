using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectController : MonoBehaviour
{
    [SerializeField] private VisualEffect specialEffect;
    
    public void StopEffect() {
        specialEffect.Stop();
    }
    public void PlayEffect() {
        specialEffect.Play();
    }
}
