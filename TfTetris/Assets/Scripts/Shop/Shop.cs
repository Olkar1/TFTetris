using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private RectTransform monsterIconContainer;
    private float containerWidth;

    [SerializeField] private int numbersOfIcons;

    [SerializeField] private List<MonsterShopIcon> monsterIcons;
    private List<MonsterShopIcon> shopIcons = new List<MonsterShopIcon>();

    [SerializeField] private Button rollButton;
    private void Awake() {
        containerWidth = monsterIconContainer.rect.width;
        rollButton.onClick.AddListener(SetNewMonsters);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) {
            SetNewMonsters();
        }
    }
    public void SpawnIcons() {
        if (GameManager.instance.GetGameStatus() != GameManager.GameStatus.Shoping) {
            return;
        }
        float conteinerLeft = -containerWidth / 2;
        bool iconWidthSet = false;
        float iconWidth = 0;
        float offset = 0;

        for (int iconIndex =0; iconIndex < numbersOfIcons; iconIndex++) {
            var icon = Instantiate(GetRandomMonster()); ///SpawnRandomIcon
            icon.transform.name = "icon: " + (iconIndex + 1);
            shopIcons.Add(icon);
            RectTransform iconRect = icon.GetComponent<RectTransform>();
            if (!iconWidthSet) {
                iconWidth = iconRect.rect.width;
            }
            if (iconWidth * numbersOfIcons > containerWidth) {
                iconWidth = containerWidth / numbersOfIcons;
                iconRect.sizeDelta = new Vector2(iconWidth, iconRect.rect.height);
                iconRect.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(iconWidth, iconRect.rect.height);
            }
            else {
                offset = (containerWidth - iconWidth * numbersOfIcons) /2;
            }

            icon.transform.SetParent(monsterIconContainer.transform);
            iconRect.anchoredPosition = new Vector3(conteinerLeft + iconWidth / 2 + iconWidth * iconIndex + offset, 0,0) ;
        }
    }
    private MonsterShopIcon GetRandomMonster() {
        int i = Random.Range(0, monsterIcons.Count);
        return monsterIcons[i];
    }
    public void SetNewMonsters() {
        if(GameManager.instance.gold < 2) { return; }
        if (GameManager.instance.GetGameStatus() != GameManager.GameStatus.Shoping) { return; }
        foreach (var icon in shopIcons) {
            icon.SetNewMonster(GetRandomMonster());
            icon.bought = false;
            icon.ResetIconAlpha();
        }
        GameManager.instance.SubstractGold(2);
    }
    public void SetNewMonsters(bool forced) {
        foreach (var icon in shopIcons) {
            icon.SetNewMonster(GetRandomMonster());
            icon.bought = false;
            icon.ResetIconAlpha();
        }
    }
}
