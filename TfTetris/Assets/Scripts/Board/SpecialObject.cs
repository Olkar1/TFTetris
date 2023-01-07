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
    public Vector3 worldPosition;
    public SplecialEffectStatus splecialEffectStatus;
    public delegate void SpecialEffect();
    public SpecialEffect specialEffect;
    [SerializeField] private int effectValue;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    [SerializeField] public Color effectColor;

    [SerializeField] public float spawnTime;
    [SerializeField] private ParticleSystem spawnVFX;
    [SerializeField] private ParticleSystem flameVFX;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float spawnHeight = 2f;
    [SerializeField] private GameObject destroyedModel;
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

    public IEnumerator SpawnSpecialObjectAnimation() {
        float currentAnimTime = 0f;
        while (currentAnimTime < spawnTime) {
            float fieldYPos = animationCurve.Evaluate(1 - currentAnimTime / spawnTime);
            transform.position = new Vector3(worldPosition.x, worldPosition.y + spawnHeight * fieldYPos, worldPosition.z);
            currentAnimTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.position = worldPosition;
        spawnVFX.Play();
        flameVFX.Play();
    }
    private void HealEnemy() {
        GameManager.instance.GetCurrentEnemy().HealEnemy(effectValue);
        DestroyObject();
    }
    private void DmgPlayer() {
        Player.DmgPlayer(effectValue);
        DestroyObject();
    }

    public void DestroyObject() {
        var model = Instantiate(destroyedModel,transform.position,Quaternion.identity);
        for (int child = 0; child<model.transform.childCount; child++) {
            Rigidbody modelRb = model.transform.GetChild(child).GetComponent<Rigidbody>();
            modelRb.isKinematic = false;
            modelRb.AddExplosionForce(15f, transform.position,2f);
            modelRb.useGravity = true;
            Destroy(model.transform.GetChild(child).gameObject, 3f);
        }
        Destroy(gameObject);
    }
}
