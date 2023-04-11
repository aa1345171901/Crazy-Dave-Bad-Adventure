using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class EatItem : MonoBehaviour
{
    public Text Info;

    private FlowerPotGardenItem flowerPotGardenItem;

    private readonly int CoffeeBeanReducedHp = 8;
    private readonly int CoffeeBeanReducedArmor = 5;

    private void OnMouseEnter()
    {
        AudioManager.Instance.PlayEffectSoundByName("btnHighlight", Random.Range(0.8f, 1.2f));
    }

    private void OnMouseDown()
    {
        if (EatPlant())
        {
            AudioManager.Instance.PlayEffectSoundByName("PlantLevelUp", Random.Range(0.8f, 1.2f));
            flowerPotGardenItem.Eat();
        }
        else
        {
            AudioManager.Instance.PlayEffectSoundByName("NoSun", Random.Range(0.8f, 1.2f));
        }
    }

    private bool EatPlant()
    {
        bool result = false;
        var userData = GameManager.Instance.UserData;
        switch (flowerPotGardenItem.PlantAttribute.plantCard.plantType)
        {
            case PlantType.CoffeeBean:
                if (userData.MaximumHP > CoffeeBeanReducedHp)
                {
                    result = true;
                    userData.MaximumHP -= CoffeeBeanReducedHp;
                    userData.Armor -= CoffeeBeanReducedArmor;
                    int[] attributes = flowerPotGardenItem.PlantAttribute.attribute;
                    for (int i = 0; i < attributes.Length; i++)
                    {
                        // 字段映射
                        var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
                        switch (attributes[i])
                        {
                            // 0 肾上腺素
                            case 0:
                                userData.Adrenaline += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                                break;
                            // 1 幸运
                            case 1:
                                userData.Lucky += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                                break;
                            // 2 植物学
                            case 2:
                                userData.Botany += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                                break;
                            // 3 范围
                            case 3:
                                userData.Range += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                                break;
                            // 4 伤害
                            case 4:
                                userData.PercentageDamage += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                                break;
                            // 攻击速度
                            case 5:
                                userData.AttackSpeed += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                                break;
                            // 速度
                            case 6:
                                userData.Speed += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                                break;
                            // 力量
                            case 7:
                                userData.Power += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                                break;
                            default:
                                break;
                        }
                    }
                }
                break;
            default:
                break;
        }
        return result;
    }

    public void SetTarget(FlowerPotGardenItem flowerPotGardenItem)
    {
        this.gameObject.SetActive(true);
        this.flowerPotGardenItem = flowerPotGardenItem;
        this.Info.color = Color.black;
        switch (flowerPotGardenItem.PlantAttribute.plantCard.plantType)
        {
            case PlantType.CoffeeBean:
                if (GameManager.Instance.UserData.MaximumHP <= CoffeeBeanReducedHp)
                {
                    this.Info.color = Color.red;
                }
                break;
            default:
                break;
        }
    }
}
