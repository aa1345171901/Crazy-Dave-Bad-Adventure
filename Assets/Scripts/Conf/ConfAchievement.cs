using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfAchievement : ConfAchievementBase
{
    Dictionary<string, ConfAchievementItem> dict = new Dictionary<string, ConfAchievementItem>();

    Dictionary<int, ConfAchievementItem> plantDict = new Dictionary<int, ConfAchievementItem>();
    Dictionary<string, ConfAchievementItem> propDict = new Dictionary<string, ConfAchievementItem>();

    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            dict[item.achievementId] = item;
            string[] strs = item.unlockParam.Split('_');
            if (strs.Length > 1)
            {
                if (strs[0].Equals("1"))
                {
                    var plantType = int.Parse(strs[1]);
                    plantDict[plantType] = item;
                }
                else if (strs[0].Equals("2"))
                {
                    var propName = strs[1];
                    propDict[propName] = item;
                }
            }
        }
    }

    public ConfAchievementItem GetItemByKey(string key)
    {
        if (dict.ContainsKey(key))
            return dict[key];
        return null;
    }

    public ConfAchievementItem GetPlantItemByPlantType(int plantType)
    {
        if (plantDict.ContainsKey(plantType))
            return plantDict[plantType];
        return null;
    }

    public ConfAchievementItem GetPropItemByName(string propName)
    {
        if (propDict.ContainsKey(propName))
            return propDict[propName];
        return null;
    }
}
