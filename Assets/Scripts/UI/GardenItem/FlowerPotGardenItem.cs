using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPotGardenItem : MonoBehaviour
{
    /// <summary>
    /// 花盆中对应的植物
    /// </summary>
    public PlantUIPrefabInfo PlantUIPrefabInfo { get; set; }

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPlant(PlantUIPrefabInfo plantUIPrefabInfo)
    {
        GameObject.Instantiate(plantUIPrefabInfo.plantPrefab, this.transform);
    }
}
