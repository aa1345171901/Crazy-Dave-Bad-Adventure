using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementItemData
{
    public string key;
    public int process;
    public bool isReach;

    public AchievementItemData(string key, int process, bool isReach)
    {
        this.key = key;
        this.process = process;
        this.isReach = isReach;
    }
}

public class AchievementData
{
    private static readonly string achievementDataPath = Application.persistentDataPath + "/SaveData/Achievement.data";

    public List<AchievementItemData> lists = new List<AchievementItemData>();
    public Dictionary<string, AchievementItemData> dicts = new Dictionary<string, AchievementItemData>();

    public int GetProcess(string key)
    {
        if (dicts.ContainsKey(key))
            return dicts[key].process;
        return 0;
    }

    public bool GetReach(string key)
    {
        if (dicts.ContainsKey(key))
            return dicts[key].isReach;
        else
        {
            Debug.LogError("没有找到成就：" + key);
            return false;
        }
    }

    /// <summary>
    /// 设置成就进度，返回是否达成成就
    /// </summary>
    /// <param name="key"></param>
    /// <param name="process"></param>
    /// <returns></returns>
    public bool SetProcess(string key, int process)
    {
        bool isReach = false;
        if (dicts.ContainsKey(key))
        {
            dicts[key].process = process;
            var confItem = ConfManager.Instance.confMgr.achievement.GetItemByKey(key);
            if (confItem != null) 
            {
                isReach = process >= confItem.process;
            }
        }
        else
        {
            Debug.LogError("没有找到成就：" + key);
        }
        return isReach;
    }

    public void SetReach(string key)
    {
        if (dicts.ContainsKey(key))
            dicts[key].isReach = true;
        else
        {
            Debug.LogError("没有找到成就：" + key);
        }
    }

    public static AchievementData LoadData()
    {
        AchievementData achievementData = new AchievementData();
        string achievementDataStr = FileTool.ReadText(achievementDataPath);
        Debug.Log("read achievementDataStr:" + achievementDataStr);
        if (!string.IsNullOrEmpty(achievementDataStr))
        {
            achievementData = JsonUtility.FromJson<AchievementData>(achievementDataStr);
            foreach (var item in achievementData.lists)
            {
                achievementData.dicts[item.key] = item;
            }
            foreach (var item in ConfManager.Instance.confMgr.achievement.items)
            {
                if (!achievementData.dicts.ContainsKey(item.achievementId))
                {
                    var achievementItem = new AchievementItemData(item.achievementId, 0, false);
                    achievementData.lists.Add(achievementItem);
                    achievementData.dicts[achievementItem.key] = achievementItem;
                }
            }
        }
        return achievementData;
    }

    public static void SaveData(AchievementData achievementData)
    {
        string achievementDataStr = JsonUtility.ToJson(achievementData);
        Debug.Log("achievementDataStr:" + achievementDataStr);
        FileTool.WriteText(achievementDataPath, achievementDataStr);
    }
}
