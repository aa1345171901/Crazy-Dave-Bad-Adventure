using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionItem : MonoBehaviour
{
    public int sunPrice;
    public Text SunPriceText;

    private FlowerPotGardenItem flowerPotGardenItem;

    private PlantCard targetPlant;

    private void OnMouseEnter()
    {
        AudioManager.Instance.PlayEffectSoundByName("shopItem", Random.Range(0.8f, 1.2f));
    }

    private void OnMouseDown()
    {
        if (sunPrice < GardenManager.Instance.Sun)
        {
            AudioManager.Instance.PlayEffectSoundByName("PlantLevelUp", Random.Range(0.8f, 1.2f));

            // 能展示则一定有购买，不需要判断
            ShopManager.Instance.PurchasedPlantEvolutionDicts[targetPlant.plantType]--;
            flowerPotGardenItem.Evolution(targetPlant);
        }
        else
        {
            AudioManager.Instance.PlayEffectSoundByName("NoSun", Random.Range(0.8f, 1.2f));
        }
    }

    public void SetTarget(FlowerPotGardenItem flowerPotGardenItem, PlantCard targetPlant)
    {
        this.flowerPotGardenItem = flowerPotGardenItem;
        this.targetPlant = targetPlant;
        this.sunPrice = targetPlant.defaultSun;
        this.SunPriceText.text = sunPrice.ToString();
        if (GardenManager.Instance.Sun < this.sunPrice)
            this.SunPriceText.color = Color.red;
        else
            this.SunPriceText.color = new Color(0.2f, 0.2f, 0.2f);
    }
}
