using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfGameIntParam : ConfGameIntParamBase
{
    Dictionary<string, ConfGameIntParamItem> dict = new Dictionary<string, ConfGameIntParamItem>();

    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            dict[item.key] = item;
        }
    }

    public ConfGameIntParamItem GetItemByKey(string key)
    {
        if (dict.ContainsKey(key))
            return dict[key];
        return null;
    }
}
