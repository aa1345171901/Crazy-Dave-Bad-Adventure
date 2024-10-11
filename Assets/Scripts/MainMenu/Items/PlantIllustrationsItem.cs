using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantIllustrationsItem : MonoBehaviour
{
    public Image bg;
    public Image plantImg;
    public Text sunCost;
    public Text moneyCost;
    public GameObject mask;

    ConfPlantIllustrationsItem confItem;

    Button button;
    Action<ConfPlantIllustrationsItem> onClick;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => 
        { 
            onClick?.Invoke(confItem); 
        });
    }

    public void InitData(ConfPlantIllustrationsItem confItem, Action<ConfPlantIllustrationsItem> call)
    {
        this.confItem = confItem;
        this.onClick = call;

        var confCardItem = ConfManager.Instance.confMgr.plantCards.GetPlantCardByType(confItem.plantType);
        Sprite bg = Resources.Load<Sprite>(confCardItem.plantBgImagePath);
        this.bg.sprite = bg;

        Sprite plantImage = Resources.Load<Sprite>(confCardItem.plantImagePath);
        this.plantImg.sprite = plantImage;

        if (SaveManager.Instance.externalGrowthData.GetPlantCount(confItem.plantType) > 0)
        {
            this.sunCost.text = confCardItem.defaultSun.ToString();
            this.moneyCost.text = "$" + confCardItem.defaultPrice.ToString();
            mask.SetActive(false);
        }
        else
        {
            this.sunCost.text = "? ?";
            this.moneyCost.text = "? ?";
            mask.SetActive(true);
        }
    }
}
