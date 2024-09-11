using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfWave : ConfWaveBase
{
    public Dictionary<int, List<ConfWaveItem>> waves = new Dictionary<int, List<ConfWaveItem>>();

    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            if (!waves.ContainsKey(item.waveIndex))
                waves[item.waveIndex] = new List<ConfWaveItem>();
            waves[item.waveIndex].Add(item);
        }
    }
}
