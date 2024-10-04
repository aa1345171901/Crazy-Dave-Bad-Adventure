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
    /// <summary>
    /// 大蒜
    /// </summary>
    Gralic,
    /// <summary>
    /// 墓碑吞噬者
    /// </summary>
    Gravebuster,
    /// <summary>
    /// 魅惑菇
    /// </summary>
    HypnoShroom,
    /// <summary>
    /// 磁力菇
    /// </summary>
    MagentShroom,
    /// <summary>
    /// 金盏花
    /// </summary>
    Marigold,
    /// <summary>
    /// 路灯
    /// </summary>
    Plantern,
    /// <summary>
    /// 小喷菇
    /// </summary>
    PuffShroom,
    /// <summary>
    /// 南瓜
    /// </summary>
    PumpkinHead,
    /// <summary>
    /// 胆小菇
    /// </summary>
    ScaredyShroom,
    /// <summary>
    /// 寒冰豌豆
    /// </summary>
    SnowPea,
    /// <summary>
    /// 地刺王
    /// </summary>
    Spikerock,
    /// <summary>
    /// 地刺
    /// </summary>
    Spikeweed,
    /// <summary>
    /// 裂夹豌豆射手
    /// </summary>
    SplitPea,
    /// <summary>
    /// 杨桃
    /// </summary>
    Starfruit,
    /// <summary>
    /// 向日葵
    /// </summary>
    SunFlower,
    /// <summary>
    /// 高坚果
    /// </summary>
    TallNut,
    /// <summary>
    /// 三发豌豆
    /// </summary>
    Threepeater,
    /// <summary>
    /// 火炬树桩
    /// </summary>
    Torchwood,
    /// <summary>
    /// 双子向日葵
    /// </summary>
    TwinSunflower,
    /// <summary>
    /// 坚果
    /// </summary>
    WallNut,
    /// <summary>
    /// 寒冰菇
    /// </summary>
    IceShroom,
    /// <summary>
    /// 火爆辣椒
    /// </summary>
    Jalapeno,
    /// <summary>
    /// 毁灭菇
    /// </summary>
    DoomShroom,
    /// <summary>
    /// 窝瓜
    /// </summary>
    Squash,
    /// <summary>
    /// 土豆雷
    /// </summary>
    PotatoMine,
    /// <summary>
    /// 玉米加农炮
    /// </summary>
    CobCannon
}

[Serializable]
public class PlantCard
{
    public string plantName;
    public string plantBgImagePath;
    public string plantImagePath;
    public int defaultPrice;
    public int defaultSun;
    public string info;
    public PlantType plantType;
}

public class PlantCardItem : ShopItem
{
    [Header("植物特有")]
    public Image Bg;
    public Image Plant;
    public Text Sun;

    public Action CanNotPlanting;
    public Action CanNotPlantingLilypad;

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

        this.Info = string.Format(GameTool.LocalText(plantCard.info), plantCard.defaultSun);

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
                if (plantCard.plantType == PlantType.Lilypad)
                {
                    // 种植的荷叶和香蒲 加上刚刚购买的荷叶小于水花盆数量才能购买
                    if (GardenManager.Instance.GetNoPlantingPlantsLilypadCount() + GardenManager.Instance.GetPlantsCount(PlantType.Lilypad) + GardenManager.Instance.GetPlantsCount(PlantType.Cattail)
                        < GardenManager.Instance.WaterFlowerPotCount + GardenManager.Instance.NotPlacedWaterFlowerPotCount)
                    {
                        ShopManager.Instance.PurchasePlant(plantCard, Price, true);
                        this.gameObject.SetActive(false);
                        this.isDown = false;
                    }
                    else
                    {
                        // 提醒需要种植在水花盆里
                        CanNotPlantingLilypad?.Invoke();
                    }
                }
                else
                {
                    // 种植的加上刚买的数量小于已有花盆 + 未摆放花盆数量才能购买
                    if (GardenManager.Instance.NoPlantingPlants.Count - GardenManager.Instance.GetNoPlantingPlantsLilypadCount() + GardenManager.Instance.PlantAttributes.Count - GardenManager.Instance.GetPlantsCount(PlantType.Lilypad) - GardenManager.Instance.GetPlantsCount(PlantType.Cattail)
                        < GardenManager.Instance.FlowerPotCount + GardenManager.Instance.NotPlacedFlowerPotCount)
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
        }
        else
        {
            CannotAfford?.Invoke();
        }
    }
}
