using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExternalGardenSeedItem : MonoBehaviour
{
    public Image bg;
    public Image plantImg;
    public Text seedNum;

    Button button;

    int plantType;
    Action<int, ExternalGardenSeedItem> onClick;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => 
        { 
            onClick?.Invoke(plantType, this); 
        });
    }

    public void InitData(int plantType, Action<int, ExternalGardenSeedItem> call)
    {
        this.plantType = plantType;
        this.onClick = call;

        var confCardItem = ConfManager.Instance.confMgr.plantCards.GetPlantCardByType(plantType);
        Sprite bg = Resources.Load<Sprite>(confCardItem.plantBgImagePath.Replace("Shop/Plants/", "UI/ExternalGardenPanel/").Replace("Bg", "Seed"));
        this.bg.sprite = bg;

        Sprite plantImage = Resources.Load<Sprite>(confCardItem.plantImagePath);
        this.plantImg.sprite = plantImage;

        this.seedNum.text = "x" + SaveManager.Instance.externalGrowthData.GetPlantSeedCount(plantType);
    }
}
