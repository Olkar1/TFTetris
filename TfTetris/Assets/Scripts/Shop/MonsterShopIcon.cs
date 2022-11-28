using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterShopIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private int cost = 0;
    [SerializeField] private Button button;
    [SerializeField] private Monster monster;

    private void Start() {
        button.onClick.AddListener(SpawnMonster);
    }
    private void SpawnMonster() {
        if(GameManager.instance.GetGameStatus() != GameManager.GameStatus.shoping) { return; }
        Monster newMonster = Instantiate(monster);
        newMonster.transform.SetParent(GameManager.instance.monstersParent);
        newMonster.isHold = true;
    }
    public void SetNewMonster(MonsterShopIcon newMonster) {
        iconImage.color = newMonster.iconImage.color;///TODO set correct image
        cost = newMonster.cost;
        button = newMonster.button;
        monster = newMonster.monster;
    }
}
