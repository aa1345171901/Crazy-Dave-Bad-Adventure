using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfGrowParam : ConfGrowParamBase
{

    Dictionary<int, ConfGrowParamItem> dict = new Dictionary<int, ConfGrowParamItem>();
    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            dict[item.type] = item;
        }
    }

    public int GetGrowPrice(int type)
    {
        if (dict.ContainsKey(type))
            return dict[type].price;
        return 1;
    }
}
