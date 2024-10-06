using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfBasicAttribute : ConfBasicAttributeBase
{
    public Dictionary<string, ConfBasicAttributeItem> dict = new Dictionary<string, ConfBasicAttributeItem>();

    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            dict[item.character] = item;
        }
    }

    public ConfBasicAttributeItem GetItemByKey(string key)
    {
        if (dict.ContainsKey(key))
            return dict[key];
        return null;
    }
}
