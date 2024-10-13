using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfSeedWeight : ConfSeedWeightBase
{
    /// <summary>
    /// 在总权重中的区间
    /// </summary>
    List<int> weights = new List<int>();
    int sumWeight = 0;

    public override void OnInit()
    {
        base.OnInit();
        int index = 0;
        foreach (var item in items)
        {
            weights.Add(index);
            index += item.weight;
            sumWeight += item.weight;
        }
    }

    /// <summary>
    /// 根据权重随机植物种子
    /// </summary>
    /// <returns></returns>
    public ConfSeedWeightItem GetPlantSeedRandom()
    {
        int random = Random.Range(0, sumWeight);
        for (int i = 0; i < weights.Count - 1; i++)
        {
            // 在权重区间内
            if (random >= weights[i] && random < weights[i + 1])
            {
                return items[i];
            }
        }
        return items.Last();
    }
}
