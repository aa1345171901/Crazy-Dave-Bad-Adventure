using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfPlantIllustrations : ConfPlantIllustrationsBase
{
    Dictionary<int, ConfPlantIllustrationsItem> plantDict = new Dictionary<int, ConfPlantIllustrationsItem>();

    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            plantDict[item.plantType] = item;
        }
    }

    public ConfPlantIllustrationsItem GetPlantItemType(int plantType)
    {
        if (plantDict.ContainsKey(plantType))
            return plantDict[plantType];
        return null;
    }
}
