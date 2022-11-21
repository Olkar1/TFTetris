using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private RectTransform monsterIconContainer;
    private float containerWidth;

    [SerializeField] private int numbersOfIcons;
    [SerializeField] private MonsterShopIcon monsterIcon;
    private void Awake() {
        containerWidth = monsterIconContainer.rect.width;
        SpawnIcons(numbersOfIcons);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) {
            ClearShop();
            SpawnIcons(numbersOfIcons);
        }
    }
    private void SpawnIcons(int numberOfIcons) {
        float conteinerLeft = -containerWidth / 2;
        bool iconWidthSet = false;
        float iconWidth = 0;
        float offset = 0;

        for (int iconIndex =0; iconIndex <numberOfIcons; iconIndex++) {
            var icon = Instantiate(monsterIcon); ///SpawnRandomIcon
            RectTransform iconRect = icon.GetComponent<RectTransform>();
            ///Temp
            icon.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            ///
            if (!iconWidthSet) {
                iconWidth = iconRect.rect.width;
            }
            if (iconWidth * numberOfIcons > containerWidth) {
                iconWidth = containerWidth / numberOfIcons;
                iconRect.sizeDelta = new Vector2(iconWidth, iconRect.rect.height);
                iconRect.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(iconWidth, iconRect.rect.height);
            }
            else {
                offset = (containerWidth - iconWidth * numberOfIcons)/2;
            }

            icon.transform.parent = monsterIconContainer.transform;
            iconRect.anchoredPosition = new Vector3(conteinerLeft + iconWidth / 2 + iconWidth * iconIndex + offset, 0,0) ;
        }
    }
    private void ClearShop() {
        int childCount = monsterIconContainer.childCount;
        for (int i = 0; i<childCount; i++) {
            Destroy(monsterIconContainer.transform.GetChild(i).gameObject);
        }
    }

}
