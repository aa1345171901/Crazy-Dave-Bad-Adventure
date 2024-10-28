using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager
{
    private static AchievementManager _instance;
    public static AchievementManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new AchievementManager();
            return _instance;
        }
    }

    AchievementData _data;

    public AchievementManager()
    {
        _data = AchievementData.LoadData();
    }

    public int GetProcess(string key)
    {
        return _data.GetProcess(key);
    }

    public bool GetReach(string key) 
    {
        return _data.GetReach(key);
    }

    public void SetProcess(string key, int value)
    {
        _data.SetProcess(key, value);
    }

    public void SetReach(string key)
    {
        _data.SetReach(key);
    }

    /// <summary>
    /// 达成所有成就
    /// </summary>
    public bool AllReach()
    {
        return _data.AllReach();
    }

    public void SaveData()
    {
        AchievementData.SaveData(_data);
    }

    /// <summary>
    /// 增加成就类型1，冒险次数
    /// </summary>
    public void SetAchievementType1()
    {
        _data.adventureCount++;
        if (_data.typeDicts.ContainsKey(1))
        {
            foreach (var itemData in _data.typeDicts[1])
            {
                SetProcess(itemData.key, _data.adventureCount);
            }
        }
        SaveData();
    }

    /// <summary>
    /// 增加成就类型2，总共收集僵尸头
    /// </summary>
    public void SetAchievementType2(int headSum)
    {
        if (_data.typeDicts.ContainsKey(2))
        {
            foreach (var itemData in _data.typeDicts[2])
            {
                SetProcess(itemData.key, headSum);
            }
        }
    }

    /// <summary>
    /// 增加成就类型3，本局收集僵尸头
    /// </summary>
    public void SetAchievementType3(int headSum)
    {
        if (_data.typeDicts.ContainsKey(3))
        {
            foreach (var itemData in _data.typeDicts[3])
            {
                SetProcess(itemData.key, headSum);
            }
        }
    }

    /// <summary>
    /// 增加成就类型4，死亡次数
    /// </summary>
    public void SetAchievementType4()
    {
        _data.deadCount++;
        if (_data.typeDicts.ContainsKey(4))
        {
            foreach (var itemData in _data.typeDicts[4])
            {
                SetProcess(itemData.key, _data.deadCount);
            }
        }
        SaveData();
    }

    /// <summary>
    /// 增加成就类型5，培养植物, type 植物类型
    /// </summary>
    public void SetAchievementType5(int type)
    {
        SaveManager.Instance.externalGrowthData.AddPlantCount(type);
        if (_data.typeDicts.ContainsKey(5))
        {
            foreach (var itemData in _data.typeDicts[5])
            {
                var confItem = ConfManager.Instance.confMgr.achievement.GetItemByKey(itemData.key);
                if (int.TryParse(confItem.param, out int plantType))
                {
                    if (plantType == type)
                        SetProcess(itemData.key, itemData.process + 1);
                }
            }
        }
    }

    /// <summary>
    /// 增加成就类型6，购买商品, type 植物类型
    /// </summary>
    public void SetAchievementType6(string key)
    {
        if (_data.typeDicts.ContainsKey(6))
        {
            foreach (var itemData in _data.typeDicts[6])
            {
                var confItem = ConfManager.Instance.confMgr.achievement.GetItemByKey(itemData.key);
                // 为空购买任意商品，不为空购买指定商品
                if (string.IsNullOrEmpty(confItem.param) || string.Equals(confItem.param, key))
                {
                    SetProcess(itemData.key, itemData.process + 1);
                }
            }
        }
    }

    /// <summary>
    /// 增加成就类型7，植物击杀僵尸  type 伤害类型
    /// </summary>
    public void SetAchievementType7(int type)
    {
        if (_data.typeDicts.ContainsKey(7))
        {
            foreach (var itemData in _data.typeDicts[7])
            {
                var confItem = ConfManager.Instance.confMgr.achievement.GetItemByKey(itemData.key);
                if (int.TryParse(confItem.param, out int damageType))
                {
                    if (damageType == type)
                        SetProcess(itemData.key, itemData.process + 1);
                }
            }
        }
    }

    /// <summary>
    /// 增加成就类型8，击杀僵尸  type 僵尸类型
    /// </summary>
    public void SetAchievementType8(int type)
    {
        if (_data.typeDicts.ContainsKey(8))
        {
            foreach (var itemData in _data.typeDicts[8])
            {
                var confItem = ConfManager.Instance.confMgr.achievement.GetItemByKey(itemData.key);
                if (int.TryParse(confItem.param, out int zombieType))
                {
                    if (zombieType == type)
                        SetProcess(itemData.key, itemData.process + 1);
                }
            }
        }
    }

    /// <summary>
    /// 增加成就类型9，使用手动植物或特殊道具
    /// </summary>
    public void SetAchievementType9(int type)
    {
        if (_data.typeDicts.ContainsKey(9))
        {
            foreach (var itemData in _data.typeDicts[9])
            {
                var confItem = ConfManager.Instance.confMgr.achievement.GetItemByKey(itemData.key);
                if (int.TryParse(confItem.param, out int zombieType))
                {
                    if (zombieType == type)
                        SetProcess(itemData.key, itemData.process + 1);
                }
            }
        }
    }

    /// <summary>
    /// 增加成就类型10，属性数值
    /// </summary>
    public void SetAchievementType10(string type, int value)
    {
        if (_data.typeDicts.ContainsKey(10))
        {
            foreach (var itemData in _data.typeDicts[10])
            {
                var confItem = ConfManager.Instance.confMgr.achievement.GetItemByKey(itemData.key);
                if (string.Equals(confItem.param, type))
                {
                    SetProcess(itemData.key, value);
                }
            }
        }
    }
}
