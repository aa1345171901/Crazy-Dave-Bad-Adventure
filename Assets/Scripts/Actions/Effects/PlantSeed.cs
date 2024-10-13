using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PlantSeed : MoneyClick
{
    public Transform effectRoot;

    public int plantType { get; set; }
    public int quality { get; set; }

    private void Start()
    {
        var effectGo = Resources.Load("Prefabs/Effects/plantSeed/guangzhu_" + quality);
        GameObject.Instantiate(effectGo, effectRoot);
    }

    private void OnMouseEnter()
    {

    }

    private void OnMouseDown()
    {
        OnClick();
    }

    protected override void NumAdd()
    {
        SaveManager.Instance.externalGrowthData.AddPlantSeedCount(plantType);
        var confPlant = ConfManager.Instance.confMgr.plantCards.GetPlantCardByType(plantType);
        string text = string.Format(GameTool.LocalText("getSeed"), GameTool.LocalText(confPlant.plantName));
        GameManager.Instance.Player.Health.SetHUDText(text);
    }
}
