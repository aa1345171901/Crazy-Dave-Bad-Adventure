using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum PlantType
{
    None,
    /// <summary>
    /// 豌豆射手
    /// </summary>
    Peashooter,
    /// <summary>
    /// 双发豌豆射手
    /// </summary>
    Repeater,
    /// <summary>
    /// 仙人掌
    /// </summary>
    Cactus,
    /// <summary>
    /// 三叶草
    /// </summary>
    Blover,
    /// <summary>
    /// 香蒲
    /// </summary>
    Cattail,
    /// <summary>
    /// 樱桃炸弹
    /// </summary>
    CherryBomb,
    /// <summary>
    /// 大嘴花
    /// </summary>
    Chomper,
    /// <summary>
    /// 咖啡豆
    /// </summary>
    CoffeeBean,
    /// <summary>
    /// 玉米投手
    /// </summary>
    Cornpult,
    /// <summary>
    /// 大喷菇
    /// </summary>
    FumeShroom,
    /// <summary>
    /// 加特林豌豆
    /// </summary>
    GatlingPea,
    /// <summary>
    /// 曾哥
    /// </summary>
    GloomShroom,
    /// <summary>
    /// 金磁力菇
    /// </summary>
    GoldMagent,
    /// <summary>
    /// 荷叶
    /// </summary>
    Lilypad,
}

[Serializable]
public class PlantCard : ISerializationCallbackReceiver
{
    public string plantName;
    public string plantBgImagePath;
    public string plantImagePath;
    public int defaultPrice;
    public int defaultSun;
    public string info;

    public PlantType plantType;
    [HideInInspector]
    public string plantTypeString = "Peashooter";

    public void OnAfterDeserialize()
    {
        if (!string.IsNullOrEmpty(plantTypeString))
        {
            PlantType type = (PlantType)System.Enum.Parse(typeof(PlantType), plantTypeString);
            plantType = type;
        }
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

    public Action CanNotPlanting;

    private PlantCard plantCard;

    public void SetPlant(PlantCard plantCard)
    {
        this.plantCard = plantCard;

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

    protected override void OnClick()
    {
        if (ShopManager.Instance.Money >= Price)
        {
            // 购买的进化卡
            if (ShopManager.Instance.PurchasePlant(plantCard, Price))
            {
                this.gameObject.SetActive(false);
                this.isDown = false;
            }
            else
            {
                // 种植的加上刚买的数量小于已有花盆 + 未摆放花盆数量才能购买
                if (GardenManager.Instance.NoPlantingPlants.Count + GardenManager.Instance.PlantAttributes.Count < GardenManager.Instance.FlowerPotCount + GardenManager.Instance.NotPlacedFlowerPotCount)
                {
                    ShopManager.Instance.PurchasePlant(plantCard, Price, true);
                    this.gameObject.SetActive(false);
                    this.isDown = false;
                }
                else
                {
                    // 提醒花盆不足
                    CanNotPlanting?.Invoke();
                }
            }
        }
        else
        {
            CannotAfford?.Invoke();
        }
    }
}
