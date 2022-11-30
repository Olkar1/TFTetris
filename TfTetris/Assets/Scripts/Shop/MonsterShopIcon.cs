using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterShopIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private int cost = 0;
    [SerializeField] private Button button;
    [SerializeField] private Monster monster;
    [SerializeField] private TextMeshProUGUI costText;

    public bool bought = false;
    Color iconColor = new Color();
    private void Awake() {
        button.onClick.AddListener(SpawnMonster);
        iconColor = iconImage.color;
    }
    private void SpawnMonster() {
        if(GameManager.instance.gold < cost) { return; }
        if(GameManager.instance.GetGameStatus() != GameManager.GameStatus.Shoping) { return; }
        if (bought) { return; }
        if (Pointer.hold) { return; }
        Monster newMonster = Instantiate(monster);
        newMonster.transform.SetParent(GameManager.instance.monstersParent);
        newMonster.isHold = true;
        Pointer.hold = true;
        bought = true;
        iconImage.color = new Color(iconColor.r, iconColor.g, iconColor.b, 0.1f);
        GameManager.instance.SubstractGold(cost);
    }
    public void SetNewMonster(MonsterShopIcon newMonster) {
        iconImage.color = newMonster.iconImage.color;///TODO set correct image
        cost = newMonster.cost;
        costText.text = cost + " gold";
        button = newMonster.button;
        monster = newMonster.monster;
        iconColor = newMonster.iconImage.color;
    }
    public void ResetIconAlpha() {
        iconImage.color = new Color(iconColor.r, iconColor.g, iconColor.b, 1f);
    }
}
