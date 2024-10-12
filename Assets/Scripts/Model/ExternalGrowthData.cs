using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 局外成长类型
/// </summary>
public enum GrowType
{
    None,
    /// <summary>
    /// 基础属性
    /// </summary>
    Attribute,
    /// <summary>
    /// 卡槽数量
    /// </summary>
    SlotNum,
    /// <summary>
    /// 初始道具
    /// </summary>
    StartProp,
    /// <summary>
    /// 初始植物
    /// </summary>
    StartPlant,
    /// <summary>
    /// 初始阳光
    /// </summary>
    StartSun,
    /// <summary>
    /// 初始金币
    /// </summary>
    StartGold,
    /// <summary>
    /// 复活次数
    /// </summary>
    LifeTime,
    /// <summary>
    /// 诅咒-怪物增多%比例
    /// </summary>
    Curse,
    /// <summary>
    /// 跑步体力恢复
    /// </summary>
    PhysicalRecovery,
    /// <summary>
    /// 跑步时间
    /// </summary>
    RunTime,
    /// <summary>
    /// 冲刺次数
    /// </summary>
    DashTime,
    /// <summary>
    /// 冲刺恢复
    /// </summary>
    DashRecovery,
}

[Serializable]
public class TypeIntData
{
    public int key;
    public int value;

    public TypeIntData(int key, int value)
    {
        this.key = key;
        this.value = value;
    }
}

/// <summary>
/// 局外数据
/// </summary>
public class ExternalGrowthData
{

    #region 僵尸头相关数据  主要祭坛成长 用到
    /// <summary>
    /// 现在head的数量
    /// </summary>
    public int headNum;
    public int HeadNum
    {
        get
        {
            return headNum;
        }
        set
        {
            headNum = value;
            AchievementManager.Instance.SetAchievementType2(GetSumHeadCount());
        }
    }

    /// <summary>
    /// 成长升级
    /// </summary>
    public List<int> levels = new List<int>();
    /// <summary>
    /// key值，对于levels的index
    /// </summary>
    public List<string> keys = new List<string>();

    public int GetLevelByKey(string key)
    {
        int index = keys.IndexOf(key);
        return index > -1 ? levels[index] : 0;
    }

    public void SetGrowLevel(string key, int level)
    {
        int index = keys.IndexOf(key);
        if (index == -1)
        {
            keys.Add(key);
            levels.Add(level);
        }
        else
        {
            levels[index] = level;
        }
        if (AllLevelMax())
            AchievementManager.Instance.SetReach("10038");
    }

    public int GetGrowSumValueByKey(string key)
    {
        var confItem = ConfManager.Instance.confMgr.externlGrow.GetItemByKey(key);
        var level = SaveManager.Instance.externalGrowthData.GetLevelByKey(key);
        int sum = 0;
        for (int i = 0; i < level; i++)
        {
            sum += confItem.levelAdd[i];
        }
        return sum;
    }

    public void Reduction()
    {
        int sum = 0;
        foreach (var item in keys)
        {
            var confItem = ConfManager.Instance.confMgr.externlGrow.GetItemByKey(item);
            int index = keys.IndexOf(item);
            int level = levels[index];
            for (int i = 0; i < level; i++)
            {
                sum += confItem.cost[i];
            }
        }
        headNum += sum;
        levels.Clear();
        keys.Clear();
    }

    public int GetSumHeadCount()
    {
        int sum = 0;
        foreach (var item in keys)
        {
            var confItem = ConfManager.Instance.confMgr.externlGrow.GetItemByKey(item);
            int index = keys.IndexOf(item);
            int level = levels[index];
            for (int i = 0; i < level; i++)
            {
                sum += confItem.cost[i];
            }
        }
        return sum + headNum;
    }

    public bool AllLevelMax()
    {
        bool result = true;
        foreach (var item in ConfManager.Instance.confMgr.externlGrow.items)
        {
            if (keys.Contains(item.key))
            {
                int index = keys.IndexOf(item.key);
                int level = levels[index];
                if (level < item.cost.Length)
                {
                    result = false;
                    break;
                }
            }
            else
            {
                result = false;
                break;
            }
        }
        return result;
    }

    #endregion


    #region 培养植物  遇到僵尸数据 主要图鉴开启用到
    /// <summary>
    /// 培养的植物类型
    /// </summary>
    public List<TypeIntData> plantType = new List<TypeIntData>();

    /// <summary>
    /// 遇到僵尸类型
    /// </summary>
    public List<TypeIntData> zombieType = new List<TypeIntData>();

    /// <summary>
    /// 获取植物培养类型数量
    /// </summary>
    /// <returns></returns>
    public int GetPlantCount(int type)
    {
        var list = plantType.Where((e) => e.key == type);
        var data = list.Count() == 0 ? null : list.First();
        return data != null ? data.value : 0;
    }

    public void AddPlantCount(int type)
    {
        var list = plantType.Where((e) => e.key == type);
        var data = list.Count() == 0 ? null : list.First();
        if (data == null)
        {
            plantType.Add(new TypeIntData(type, 1));
        }
        else
        {
            data.value++;
        }
    }

    /// <summary>
    /// 获取僵尸击杀类型数量
    /// </summary>
    /// <returns></returns>
    public int GetZombieCount(int type)
    {
        var list = zombieType.Where((e) => e.key == type);
        var data = list.Count() == 0 ? null : list.First();
        return data != null ? data.value : 0;
    }

    public void AddZombieCount(int type)
    {
        var list = zombieType.Where((e) => e.key == type);
        var data = list.Count() == 0 ? null : list.First();
        if (data == null)
        {
            zombieType.Add(new TypeIntData(type, 1));
        }
        else
        {
            data.value++;
        }
    }
    #endregion

    #region 外部花园种植，外部花园植物种子
    /// <summary>
    /// 获取的植物种子
    /// </summary>
    public List<TypeIntData> plantSeeds = new List<TypeIntData>();

    /// <summary>
    /// 外部花园安放植物 key 对应 pos, value对应 plantType
    /// </summary>
    public List<TypeIntData> plantPlace = new List<TypeIntData>();

    /// <summary>
    /// 获取植物培养类型数量
    /// </summary>
    /// <returns></returns>
    public int GetPlantSeedCount(int type)
    {
        var list = plantSeeds.Where((e) => e.key == type);
        var data = list.Count() == 0 ? null : list.First();
        return data != null ? data.value : 0;
    }

    /// <summary>
    /// 获得植物种子
    /// </summary>
    public void AddPlantSeedCount(int type)
    {
        var list = plantSeeds.Where((e) => e.key == type);
        var data = list.Count() == 0 ? null : list.First();
        if (data == null)
        {
            plantType.Add(new TypeIntData(type, 1));
        }
        else
        {
            data.value++;
        }
    }

    /// <summary>
    /// 种植植物
    /// </summary>
    public bool PlacePlantSeed(int pos, int type)
    {
        var list = plantSeeds.Where((e) => e.key == type);
        var data = list.Count() == 0 ? null : list.First();
        // 没有植物时不执行
        if (data == null || data.value == 0)
        {
            Debug.LogError("没有植物:" + type);
            return false;
        }
        var posList = plantPlace.Where((e) => e.key == pos);
        if (posList.Count() != 0)
        {
            Debug.LogError($"位置{pos},已经有植物！");
            return false;
        }
        plantPlace.Add(new TypeIntData(pos, type));
        data.value--;
        return true;
    }

    /// <summary>
    /// 铲掉植物
    /// </summary>
    public void ShovelPlantSeed(int pos)
    {
        var posList = plantPlace.Where((e) => e.key == pos);
        var data = posList.Count() == 0 ? null : posList.First();
        if (data != null)
            plantPlace.Remove(data);
    }
    #endregion

    private static readonly string externalGrowthPath = Application.persistentDataPath + "/SaveData/ExternalGrowthData.data";

    public static ExternalGrowthData LoadData()
    {
        ExternalGrowthData externalGrowthData = new ExternalGrowthData();
        string externalGrowthStr = FileTool.ReadText(externalGrowthPath);
        Debug.Log("read externalGrowthStr:" + externalGrowthStr);
        if (!string.IsNullOrEmpty(externalGrowthStr))
        {
            externalGrowthData = JsonUtility.FromJson<ExternalGrowthData>(externalGrowthStr);
        }
        return externalGrowthData;
    }

    public static void SaveData(ExternalGrowthData externalGrowthData)
    {
        string externalGrowthStr = JsonUtility.ToJson(externalGrowthData);
        Debug.Log("externalGrowthStr:" + externalGrowthStr);
        FileTool.WriteText(externalGrowthPath, externalGrowthStr);
    }
}
