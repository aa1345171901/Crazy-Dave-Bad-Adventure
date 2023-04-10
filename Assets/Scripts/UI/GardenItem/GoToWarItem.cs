using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class GoToWarItem : MonoBehaviour
{
    public Text Info;
    public PlantCardPage gardenPlantCardPage;

    private FlowerPotGardenItem flowerPotGardenItem;

    private readonly string GoToWar = "出战";
    private readonly string Cancel = "取消出战";

    private void OnMouseEnter()
    {
        AudioManager.Instance.PlayEffectSoundByName("btnHighlight", Random.Range(0.8f, 1.2f));
    }

    private void OnMouseDown()
    {
        AudioManager.Instance.PlayEffectSoundByName("btnPressed", Random.Range(0.8f, 1.2f));
        if (GardenManager.Instance.CardslotPlant.Contains(flowerPotGardenItem.PlantAttribute))
        {
            flowerPotGardenItem.PlantAttribute.isGoToWar = false;
            this.Info.text = GoToWar;
            GardenManager.Instance.CardslotPlant.Remove(flowerPotGardenItem.PlantAttribute);
            gardenPlantCardPage.SetCard();
        }
        else
        {
            if (GardenManager.Instance.MaxSlot > GardenManager.Instance.CardslotPlant.Count)
            {
                flowerPotGardenItem.PlantAttribute.isGoToWar = true;
                this.Info.text = Cancel;
                GardenManager.Instance.CardslotPlant.Add(flowerPotGardenItem.PlantAttribute);
                gardenPlantCardPage.SetCard();
            }
            else
            {
                AudioManager.Instance.PlayEffectSoundByName("NoSun", Random.Range(0.8f, 1.2f));
            }
        }
    }

    public void SetTarget(FlowerPotGardenItem flowerPotGardenItem)
    {
        this.flowerPotGardenItem = flowerPotGardenItem;
        if (GardenManager.Instance.CardslotPlant.Contains(flowerPotGardenItem.PlantAttribute))
            this.Info.text = Cancel;
        else
            this.Info.text = GoToWar;
        if (this.flowerPotGardenItem.PlantAttribute.isManual)
            this.gameObject.SetActive(true);
    }
}
