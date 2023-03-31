using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum PlantType
{
    /// <summary>
    /// 豌豆射手
    /// </summary>
    Peashooter,
    /// <summary>
    /// 仙人掌
    /// </summary>
    Cactus,
}

[Serializable]
public class PlantCard
{
    public string plantBgImagePath;
    public string plantImagePath;
    public int defaultPrice;
    public int defaultSun;
    public string info;

    public PlantType plantType;
    [HideInInspector]
    public string plantTypeString;

    public void OnAfterDeserialize()
    {
        PlantType type = (PlantType)System.Enum.Parse(typeof(PlantType), plantTypeString);
        plantType = type;
    }

    public void OnBeforeSerialize()
    {

    }
}

public class PlantCardItem : ShopItem
{
    [Header("植物特有")]
    public Image Bg;
    public Image Plant;
    public Text Sun;

    public void SetPlant(PlantCard plantCard)
    {
        Sprite bg = Resources.Load<Sprite>(plantCard.plantBgImagePath);
        this.Bg.sprite = bg;

        Sprite plantImage = Resources.Load<Sprite>(plantCard.plantImagePath);
        this.Plant.sprite = plantImage;

        this.Sun.text = plantCard.defaultSun.ToString();

        this.Price = plantCard.defaultPrice;
        this.PriceText.text = this.Price.ToString();

        this.Info = string.Format(plantCard.info, plantCard.defaultSun);

        UpdateMoney();
    }
}
