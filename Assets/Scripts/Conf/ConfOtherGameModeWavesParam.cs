using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ConfOtherGameModeWavesParam : ConfOtherGameModeWavesParamBase
{
    Dictionary<int, ConfOtherGameModeWavesParamItem> dict = new Dictionary<int, ConfOtherGameModeWavesParamItem>();

    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            dict[item.gameModeId] = item;
        }
    }

    public ConfOtherGameModeWavesParamItem GetItemById(int id)
    {
        if (dict.ContainsKey(id))
            return dict[id];
        return null;
    }
}
