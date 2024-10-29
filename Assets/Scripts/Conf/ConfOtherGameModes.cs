using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfOtherGameModes : ConfOtherGameModesBase
{
    Dictionary<int, List<ConfOtherGameModesItem>> dicts = new Dictionary<int, List<ConfOtherGameModesItem>>();
    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            if (!dicts.ContainsKey(item.type))
                dicts[item.type] = new List<ConfOtherGameModesItem>();
            dicts[item.type].Add(item);
        }
    }

    public List<ConfOtherGameModesItem> GetItemsByBattleMode(BattleMode battleMode)
    {
        int type = (int)battleMode;
        if (dicts.ContainsKey(type))
            return dicts[type];
        return new List<ConfOtherGameModesItem>();
    }
}
