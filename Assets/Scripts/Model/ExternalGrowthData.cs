using System.Collections.Generic;
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

public class ExternalGrowthData
{
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
