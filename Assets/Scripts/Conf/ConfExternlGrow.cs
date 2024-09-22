using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfExternlGrow : ConfExternlGrowBase
{
    Dictionary<string, ConfExternlGrowItem> dict = new Dictionary<string, ConfExternlGrowItem>();
    Dictionary<int, List<ConfExternlGrowItem>> typeDict = new Dictionary<int, List<ConfExternlGrowItem>>();
    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            dict[item.key] = item;
            if (!typeDict.ContainsKey(item.growType))
            {
                typeDict[item.growType] = new List<ConfExternlGrowItem>();
            }
            typeDict[item.growType].Add(item);
        }
    }

    public ConfExternlGrowItem GetItemByKey(string key)
    {
        if (dict.ContainsKey(key))
            return dict[key];
        return null;
    }

    public List<ConfExternlGrowItem> GetItemsByType(GrowType growType)
    {
        if (typeDict.ContainsKey((int)growType))
            return typeDict[(int)growType];
        return new List<ConfExternlGrowItem>();
    }
}
