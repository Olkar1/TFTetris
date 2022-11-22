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
    private bool locked = false;
    private void Awake() {
        containerWidth = monsterIconContainer.rect.width;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) {
            ClearShop();
            SpawnIcons();
        }
    }
    public void SpawnIcons() {
        if (GameManager.gameStatus != GameManager.GameStatus.shoping) {
            return;
        }
        float conteinerLeft = -containerWidth / 2;
        bool iconWidthSet = false;
        float iconWidth = 0;
        float offset = 0;

        for (int iconIndex =0; iconIndex < numbersOfIcons; iconIndex++) {
            var icon = Instantiate(GetRandomMonster()); ///SpawnRandomIcon
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

            icon.transform.parent = monsterIconContainer.transform;
            iconRect.anchoredPosition = new Vector3(conteinerLeft + iconWidth / 2 + iconWidth * iconIndex + offset, 0,0) ;
        }
    }
    private MonsterShopIcon GetRandomMonster() {
        int i = Random.Range(0, monsterIcons.Count);
        return monsterIcons[i];
    }
    private void ClearShop() {
        if (GameManager.gameStatus != GameManager.GameStatus.shoping) {
            return;
        }
        int childCount = monsterIconContainer.childCount;
        for (int i = 0; i<childCount; i++) {
            Destroy(monsterIconContainer.transform.GetChild(i).gameObject);
        }
    }
}
