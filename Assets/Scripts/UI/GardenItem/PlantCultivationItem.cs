using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 培养的属性样式，对应修改PlantAttribute中的属性
/// </summary>
public enum CultivateAttributeType
{
    /// <summary>
    /// 培育
    /// </summary>
    Cultivate = 0,
    First,
    Second,
    Three,
}

public class PlantCultivationItem : MonoBehaviour
{
    public Text SunPrice;
    public Text ItemInfo;
    public Text Level;

    private int sunPrice;
    private FlowerPotGardenItem flowerPotGardenItem;
    private CultivateAttributeType cultivateAttributeType;
    private readonly int defalutLevelUpPrice = 25;

    public void SetInfo(CultivateAttributeType cultivateAttributeType, FlowerPotGardenItem flowerPotGardenItem, string info)
    {
        this.cultivateAttributeType = cultivateAttributeType;
        this.flowerPotGardenItem = flowerPotGardenItem;

        UpdateSunPrice();
        this.ItemInfo.text = info;
    }

    private void OnMouseDown()
    {
        if (sunPrice <= GardenManager.Instance.Sun)
        {
            int maxLevel = flowerPotGardenItem.PlantAttribute.maxLevel;
            switch (cultivateAttributeType)
            {
                case CultivateAttributeType.Cultivate:
                    GardenManager.Instance.Sun -= sunPrice;
                    flowerPotGardenItem.CultivatePlant();
                    AudioManager.Instance.PlayEffectSoundByName("PlantLevelUp", Random.Range(0.8f, 1.2f));
                    break;
                case CultivateAttributeType.First:
                    if (flowerPotGardenItem.PlantAttribute.level1 < maxLevel)
                    {
                        GardenManager.Instance.Sun -= sunPrice;
                        flowerPotGardenItem.PlantAttribute.level1++;
                        flowerPotGardenItem.PlantAttribute.AddAttribute(0);
                        flowerPotGardenItem.UpdateSunPrice();
                        AudioManager.Instance.PlayEffectSoundByName("PlantLevelUp", Random.Range(0.8f, 1.2f));
                    }
                    else
                    {
                        AudioManager.Instance.PlayEffectSoundByName("NoSun", Random.Range(0.8f, 1.2f));
                    }
                    break;
                case CultivateAttributeType.Second:
                    if (flowerPotGardenItem.PlantAttribute.level2 < maxLevel)
                    {
                        GardenManager.Instance.Sun -= sunPrice;
                        flowerPotGardenItem.PlantAttribute.level2++;
                        flowerPotGardenItem.PlantAttribute.AddAttribute(1);
                        flowerPotGardenItem.UpdateSunPrice();
                        AudioManager.Instance.PlayEffectSoundByName("PlantLevelUp", Random.Range(0.8f, 1.2f));
                    }
                    else
                    {
                        AudioManager.Instance.PlayEffectSoundByName("NoSun", Random.Range(0.8f, 1.2f));
                    }
                    break;
                case CultivateAttributeType.Three:
                    if (flowerPotGardenItem.PlantAttribute.level3 < maxLevel)
                    {
                        GardenManager.Instance.Sun -= sunPrice;
                        flowerPotGardenItem.PlantAttribute.level3++;
                        flowerPotGardenItem.PlantAttribute.AddAttribute(2);
                        flowerPotGardenItem.UpdateSunPrice();
                        AudioManager.Instance.PlayEffectSoundByName("PlantLevelUp", Random.Range(0.8f, 1.2f));
                    }
                    else
                    {
                        AudioManager.Instance.PlayEffectSoundByName("NoSun", Random.Range(0.8f, 1.2f));
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            AudioManager.Instance.PlayEffectSoundByName("NoSun", Random.Range(0.8f, 1.2f));
        }
    }

    private void OnMouseEnter()
    {
        AudioManager.Instance.PlayEffectSoundByName("shopItem", Random.Range(0.8f, 1.2f));
    }

    public void UpdateSunPrice()
    {
        int level = flowerPotGardenItem.PlantAttribute.level1 + flowerPotGardenItem.PlantAttribute.level2 + flowerPotGardenItem.PlantAttribute.level3 + 1;
        if (!flowerPotGardenItem.PlantAttribute.isCultivate)
            this.sunPrice = flowerPotGardenItem.PlantAttribute.plantCard.defaultSun;
        else
            this.sunPrice = level * defalutLevelUpPrice;
        // 吃的植物培养阳光消耗值不变
        switch (flowerPotGardenItem.PlantAttribute.plantCard.plantType)
        {
            case PlantType.CoffeeBean:
                this.sunPrice = defalutLevelUpPrice;
                break;
            default:
                break;
        }
        this.SunPrice.text = sunPrice.ToString();
        if (GardenManager.Instance.Sun < this.sunPrice)
            this.SunPrice.color = Color.red;
        else
            this.SunPrice.color = new Color(0.2f, 0.2f, 0.2f);

        int maxLevel = flowerPotGardenItem.PlantAttribute.maxLevel;
        switch (cultivateAttributeType)
        {
            case CultivateAttributeType.Cultivate:
                Level.text = "0/1";
                Level.color = Color.green;
                break;
            case CultivateAttributeType.First:
                SetLevel(flowerPotGardenItem.PlantAttribute.level1 + "/" + maxLevel, flowerPotGardenItem.PlantAttribute.level1 < maxLevel);
                break;
            case CultivateAttributeType.Second:
                SetLevel(flowerPotGardenItem.PlantAttribute.level2 + "/" + maxLevel, flowerPotGardenItem.PlantAttribute.level2 < maxLevel);
                break;
            case CultivateAttributeType.Three:
                SetLevel(flowerPotGardenItem.PlantAttribute.level3 + "/" + maxLevel, flowerPotGardenItem.PlantAttribute.level3 < maxLevel);
                break;
            default:
                break;
        }
    }

    private void SetLevel(string str, bool isNoMax)
    {
        Level.text = str;
        if (isNoMax)
            Level.color = Color.green;
        else
        {
            Level.color = Color.red;
            SunPrice.text = "+∞";
            SunPrice.color = Color.red;
        }
    }
}
