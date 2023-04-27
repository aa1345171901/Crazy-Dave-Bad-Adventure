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
    /// �㶹����
    /// </summary>
    Peashooter,
    /// <summary>
    /// ˫���㶹����
    /// </summary>
    Repeater,
    /// <summary>
    /// ������
    /// </summary>
    Cactus,
    /// <summary>
    /// ��Ҷ��
    /// </summary>
    Blover,
    /// <summary>
    /// ����
    /// </summary>
    Cattail,
    /// <summary>
    /// ӣ��ը��
    /// </summary>
    CherryBomb,
    /// <summary>
    /// ���컨
    /// </summary>
    Chomper,
    /// <summary>
    /// ���ȶ�
    /// </summary>
    CoffeeBean,
    /// <summary>
    /// ����Ͷ��
    /// </summary>
    Cornpult,
    /// <summary>
    /// ���繽
    /// </summary>
    FumeShroom,
    /// <summary>
    /// �������㶹
    /// </summary>
    GatlingPea,
    /// <summary>
    /// ����
    /// </summary>
    GloomShroom,
    /// <summary>
    /// �������
    /// </summary>
    GoldMagent,
    /// <summary>
    /// ��Ҷ
    /// </summary>
    Lilypad,
    /// <summary>
    /// ����
    /// </summary>
    Gralic,
    /// <summary>
    /// Ĺ��������
    /// </summary>
    Gravebuster,
    /// <summary>
    /// �Ȼ�
    /// </summary>
    HypnoShroom,
    /// <summary>
    /// ������
    /// </summary>
    MagentShroom,
    /// <summary>
    /// ��յ��
    /// </summary>
    Marigold,
    /// <summary>
    /// ·��
    /// </summary>
    Plantern,
    /// <summary>
    /// С�繽
    /// </summary>
    PuffShroom,
    /// <summary>
    /// �Ϲ�
    /// </summary>
    PumpkinHead,
    /// <summary>
    /// ��С��
    /// </summary>
    ScaredyShroom,
    /// <summary>
    /// �����㶹
    /// </summary>
    SnowPea,
    /// <summary>
    /// �ش���
    /// </summary>
    Spikerock,
    /// <summary>
    /// �ش�
    /// </summary>
    Spikeweed,
    /// <summary>
    /// �Ѽ��㶹����
    /// </summary>
    SplitPea,
    /// <summary>
    /// ����
    /// </summary>
    Starfruit,
    /// <summary>
    /// ���տ�
    /// </summary>
    SunFlower,
    /// <summary>
    /// �߼��
    /// </summary>
    TallNut,
    /// <summary>
    /// �����㶹
    /// </summary>
    Threepeater,
    /// <summary>
    /// �����׮
    /// </summary>
    Torchwood,
    /// <summary>
    /// ˫�����տ�
    /// </summary>
    TwinSunflower,
    /// <summary>
    /// ���
    /// </summary>
    WallNut,
    /// <summary>
    /// ������
    /// </summary>
    IceShroom,
    /// <summary>
    /// ������
    /// </summary>
    Jalapeno,
    /// <summary>
    /// ����
    /// </summary>
    DoomShroom,
    /// <summary>
    /// �ѹ�
    /// </summary>
    Squash,
    /// <summary>
    /// ������
    /// </summary>
    PotatoMine,
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
    [Header("ֲ������")]
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

        this.Info = string.Format(plantCard.info, plantCard.defaultSun);

        UpdateMoney();
    }

    protected override void OnClick()
    {
        if (ShopManager.Instance.Money >= Price)
        {
            // ����Ľ�����
            if (ShopManager.Instance.PurchasePlant(plantCard, Price))
            {
                this.gameObject.SetActive(false);
                this.isDown = false;
            }
            else
            {
                if (plantCard.plantType == PlantType.Lilypad)
                {
                    // ��ֲ�ĺ�Ҷ������ ���ϸոչ���ĺ�ҶС��ˮ�����������ܹ���
                    if (GardenManager.Instance.GetNoPlantingPlantsLilypadCount() + GardenManager.Instance.GetPlantsCount(PlantType.Lilypad) + GardenManager.Instance.GetPlantsCount(PlantType.Cattail)
                        < GardenManager.Instance.WaterFlowerPotCount + GardenManager.Instance.NotPlacedWaterFlowerPotCount)
                    {
                        ShopManager.Instance.PurchasePlant(plantCard, Price, true);
                        this.gameObject.SetActive(false);
                        this.isDown = false;
                    }
                    else
                    {
                        // ������Ҫ��ֲ��ˮ������
                        CanNotPlantingLilypad?.Invoke();
                    }
                }
                else
                {
                    // ��ֲ�ļ��ϸ��������С�����л��� + δ�ڷŻ����������ܹ���
                    if (GardenManager.Instance.NoPlantingPlants.Count - GardenManager.Instance.GetNoPlantingPlantsLilypadCount() + GardenManager.Instance.PlantAttributes.Count - GardenManager.Instance.GetPlantsCount(PlantType.Lilypad) - GardenManager.Instance.GetPlantsCount(PlantType.Cattail)
                        < GardenManager.Instance.FlowerPotCount + GardenManager.Instance.NotPlacedFlowerPotCount)
                    {
                        ShopManager.Instance.PurchasePlant(plantCard, Price, true);
                        this.gameObject.SetActive(false);
                        this.isDown = false;
                    }
                    else
                    {
                        // ���ѻ��費��
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
