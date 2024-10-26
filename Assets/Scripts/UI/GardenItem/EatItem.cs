using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class EatItem : MonoBehaviour
{
    public Text Info;

    private FlowerPotGardenItem flowerPotGardenItem;

    private void OnMouseEnter()
    {
        AudioManager.Instance.PlayEffectSoundByName("btnHighlight", Random.Range(0.8f, 1.2f));
    }

    private void OnMouseDown()
    {
        if (EatPlant())
        {
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
        int[] attributes = flowerPotGardenItem.PlantAttribute.attribute;
        switch (flowerPotGardenItem.PlantAttribute.plantCard.plantType)
        {
            case PlantType.CoffeeBean:
                AudioManager.Instance.PlayEffectSoundByName("PlantLevelUp", Random.Range(0.8f, 1.2f));
                result = true;
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
                            userData.LuckyProperties += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
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
                break;
            case PlantType.Gralic:
                AudioManager.Instance.PlayEffectSoundByName("Eat", Random.Range(0.8f, 1.2f));
                result = true;
                for (int i = 0; i < attributes.Length; i++)
                {
                    // 字段映射
                    var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
                    switch (attributes[i])
                    {
                        // 0 生命恢复
                        case 0:
                            userData.LifeRecovery += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                            break;
                        // 1 肾上腺素
                        case 1:
                            userData.Adrenaline += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                            break;
                        // 2 范围
                        case 2:
                            userData.Range += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                            break;
                        // 3 植物学
                        case 3:
                            userData.Botany += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                            break;
                        // 4 最大生命值
                        case 4:
                            userData.MaximumHP += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                            break;
                        // 攻击速度
                        case 5:
                            userData.AttackSpeed += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                            break;
                        // 速度
                        case 6:
                            userData.Speed += (int)fieldInfo.GetValue(flowerPotGardenItem.PlantAttribute);
                            break;
                        default:
                            break;
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
            //case PlantType.CoffeeBean:
            //    if (GameManager.Instance.UserData.MaximumHP <= CoffeeBeanReducedHp)
            //    {
            //        this.Info.color = Color.red;
            //    }
            //    break;
            default:
                break;
        }
    }
}
