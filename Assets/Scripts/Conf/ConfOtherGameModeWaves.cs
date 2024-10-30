using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfOtherGameModeWaves : ConfOtherGameModeWavesBase
{
    Dictionary<int, List<ConfOtherGameModeWavesItem>> dict = new Dictionary<int, List<ConfOtherGameModeWavesItem>>();
    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            if (!dict.ContainsKey(item.gameModeId))
                dict[item.gameModeId] = new List<ConfOtherGameModeWavesItem>();
            dict[item.gameModeId].Add(item);
        }
    }

    public List<ConfOtherGameModeWavesItem> GetListsById(int id)
    {
        if (dict.ContainsKey(id))
            return dict[id];
        return new List<ConfOtherGameModeWavesItem>();
    }
}
