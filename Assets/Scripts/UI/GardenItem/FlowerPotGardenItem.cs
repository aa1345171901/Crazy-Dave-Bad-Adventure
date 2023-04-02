using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlantAttribute
{
    public PlantCard plantCard;
    public bool isCultivate;

    public PlantAttribute(PlantCard plantCard)
    {
        this.plantCard = plantCard;
    }
}

public class FlowerPotGardenItem : MonoBehaviour
{
    public PlantAttribute PlantAttribute;
    private Animator animator;
    private PlantUIPrefabInfo plantUIPrefabInfo;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPlant(PlantUIPrefabInfo plantUIPrefabInfo, PlantCard plantCard)
    {
        this.plantUIPrefabInfo = plantUIPrefabInfo;
        this.PlantAttribute = new PlantAttribute(plantCard);
        GameObject.Instantiate(GardenManager.Instance.SeedingPrefab, this.transform);
        GardenManager.Instance.PlantAttributes.Add(this.PlantAttribute);
    }
}
