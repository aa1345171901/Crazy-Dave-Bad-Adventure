using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfExternlGrow : ConfExternlGrowBase
{
    Dictionary<string, ConfExternlGrowItem> dict = new Dictionary<string, ConfExternlGrowItem>();
    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            dict[item.key] = item;
        }
    }

    public ConfExternlGrowItem GetItemByKey(string key)
    {
        if (dict.ContainsKey(key))
            return dict[key];
        return null;
    }
}
