using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObject : MonoBehaviour
{
    public enum SplecialEffectStatus {
        heal,
        dmg
    }
    public Vector2 position;
    public SplecialEffectStatus splecialEffectStatus;
    public delegate void SpecialEffect();
    public SpecialEffect specialEffect;
    private void Start() {
        switch (splecialEffectStatus) {
            case SplecialEffectStatus.dmg:
                specialEffect = DmgPlayer;
                break;
            case SplecialEffectStatus.heal:
                specialEffect = HealEnemy;
                break;
        }
    }

    private void HealEnemy() {
        Debug.LogError("HealEnemy");
    }
    private void DmgPlayer() {
        Debug.LogError("DmgPlayer");
    }
}
