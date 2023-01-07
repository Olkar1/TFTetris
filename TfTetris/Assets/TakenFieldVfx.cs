using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakenFieldVfx : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    [SerializeField] private List<ParticleSystem> particlesToSwapColor;
    public void PlayEffect() {
        particle.Play();
    }
    public void ChangeParticleColor(Color color) {
        foreach (var particle in particlesToSwapColor) {
            Color initColor = particle.main.startColor.color; ;
            Color newColor = new Color(color.r,color.g,color.b,initColor.a);
            var main = particle.main;
            main.startColor = newColor;
        }
    }
}
