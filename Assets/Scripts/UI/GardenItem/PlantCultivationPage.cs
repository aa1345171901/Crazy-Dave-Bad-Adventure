using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantCultivationPage : MonoBehaviour
{
    public List<PlantCultivationItem> plantCultivationItems;
    public Text InfoText;

    private readonly string CultivateInfo = "����";
    private readonly string CultivateBasicDamage = "�����˺�";
    private readonly string CultivatePercentageDamage = "�ٷֱ��˺�";
    private readonly string CultivateRange = "������Χ";

    public void SetPlantAttribute(FlowerPotGardenItem flowerPotGardenItem)
    {
        this.InfoText.text = flowerPotGardenItem.PlantAttribute.plantCard.plantName;
        // ��δ��������
        if (!flowerPotGardenItem.PlantAttribute.isCultivate)
        {
            var plantCultivateItem = plantCultivationItems[0];
            foreach (var item in plantCultivationItems)
            {
                item.gameObject.SetActive(false);
            }
            plantCultivateItem.gameObject.SetActive(true);
            plantCultivateItem.SetInfo(CultivateAttributeType.Cultivate, flowerPotGardenItem, CultivateInfo);
        }
        else
        {
            switch (flowerPotGardenItem.PlantAttribute.plantCard.plantType)
            {
                case PlantType.Peashooter:
                    SetItemInfo(flowerPotGardenItem, new string[] {CultivateBasicDamage, CultivatePercentageDamage, CultivateRange});
                    break;
                case PlantType.Repeater:
                    break;
                case PlantType.Cactus:
                    break;
                case PlantType.Blover:
                    break;
                case PlantType.Cattail:
                    break;
                case PlantType.CherryBomb:
                    break;
                case PlantType.Chomper:
                    break;
                case PlantType.CoffeeBean:
                    break;
                case PlantType.Cornpult:
                    break;
                case PlantType.FumeShroom:
                    break;
                case PlantType.GatlingPea:
                    break;
                case PlantType.GloomShroom:
                    break;
                case PlantType.GoldMagent:
                    break;
                default:
                    break;
            }
        }
    }

    private void SetItemInfo(FlowerPotGardenItem flowerPotGardenItem, string[] infos)
    {
        for (int i = 0; i < plantCultivationItems.Count; i++)
        {
            plantCultivationItems[i].gameObject.SetActive(true);
            plantCultivationItems[i].SetInfo((CultivateAttributeType)(i + 1), flowerPotGardenItem, infos[i]);
        }
    }

    public void UpdateSunPrice()
    {
        foreach (var item in plantCultivationItems)
        {
            item.UpdateSunPrice();
        }
    }
}
