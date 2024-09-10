using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfLocalText : ConfLocalTextBase
{
    public Dictionary<int, ConfLocalTextItem> dict = new Dictionary<int, ConfLocalTextItem>();

    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            var key = item.key.GetHashCode();
            dict[key] = item;
        }
    }

    public string GetText(string key)
    {
        var hashCode = key.GetHashCode();
        if (!ConfManager.Instance.confMgr.localText.dict.ContainsKey(hashCode))
            return key;
        if (ConfManager.Instance.language == "cn")
            return ConfManager.Instance.confMgr.localText.dict[hashCode].cn;
        else
            return ConfManager.Instance.confMgr.localText.dict[hashCode].en;
    }
}
