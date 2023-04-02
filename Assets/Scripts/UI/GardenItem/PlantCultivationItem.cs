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
        if (sunPrice < GardenManager.Instance.Sun)
        {
            GardenManager.Instance.Sun -= sunPrice;
            switch (cultivateAttributeType)
            {
                case CultivateAttributeType.Cultivate:
                    flowerPotGardenItem.CultivatePlant();
                    break;
                case CultivateAttributeType.First:
                    flowerPotGardenItem.PlantAttribute.value1++;
                    flowerPotGardenItem.UpdateSunPrice();
                    break;
                case CultivateAttributeType.Second:
                    flowerPotGardenItem.PlantAttribute.value2++;
                    flowerPotGardenItem.UpdateSunPrice();
                    break;
                case CultivateAttributeType.Three:
                    flowerPotGardenItem.PlantAttribute.value3++;
                    flowerPotGardenItem.UpdateSunPrice();
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayEffectSoundByName("PlantLevelUp", Random.Range(0.8f, 1.2f));
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
        int level = flowerPotGardenItem.PlantAttribute.value1 + flowerPotGardenItem.PlantAttribute.value2 + flowerPotGardenItem.PlantAttribute.value3;
        if (!flowerPotGardenItem.PlantAttribute.isCultivate)
            this.sunPrice = flowerPotGardenItem.PlantAttribute.plantCard.defaultSun;
        else
            this.sunPrice = level * defalutLevelUpPrice;
        this.SunPrice.text = sunPrice.ToString();
        if (GardenManager.Instance.Sun < this.sunPrice)
            this.SunPrice.color = Color.red;
        else
            this.SunPrice.color = new Color(0.2f, 0.2f, 0.2f);
    }
}
