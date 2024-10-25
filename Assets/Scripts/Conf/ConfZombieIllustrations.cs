using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfZombieIllustrations : ConfZombieIllustrationsBase
{
    Dictionary<int, ConfZombieIllustrationsItem> dict = new Dictionary<int, ConfZombieIllustrationsItem>();

    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            dict[item.zombieType] = item;
        }
    }

    public ConfZombieIllustrationsItem GetItemByType(int zombieType)
    {
        if (dict.ContainsKey(zombieType))
            return dict[zombieType];
        return null;
    }
}
