using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfMoneyParam : ConfMoneyParamBase
{
    public Dictionary<string, ConfMoneyParamItem> dict = new Dictionary<string, ConfMoneyParamItem>();
    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            dict[item.key] = item;
        }
    }

    public int GetWeightByKey(string key)
    {
        if (dict.ContainsKey(key))
            return dict[key].weight;
        return 0;
    }
}
