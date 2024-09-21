using System.Collections.Generic;
using UnityEngine;

public class ExternalGrowthData
{
    /// <summary>
    /// 现在head的数量
    /// </summary>
    public int headNum;

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

    public static void SaveSystemData(ExternalGrowthData externalGrowthData)
    {
        string externalGrowthStr = JsonUtility.ToJson(externalGrowthData);
        Debug.Log("externalGrowthStr:" + externalGrowthStr);
        FileTool.WriteText(externalGrowthPath, externalGrowthStr);
    }
}
