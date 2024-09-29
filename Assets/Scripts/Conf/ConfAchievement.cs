using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfAchievement : ConfAchievementBase
{
    Dictionary<string, ConfAchievementItem> dict = new Dictionary<string, ConfAchievementItem>();

    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            dict[item.achievementId] = item;
        }
    }

    public ConfAchievementItem GetItemByKey(string key)
    {
        if (dict.ContainsKey(key))
            return dict[key];
        return null;
    }
}
