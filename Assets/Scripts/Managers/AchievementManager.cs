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
        AchievementData.SaveData(_data);
    }

    public void SetReach(string key)
    {
        _data.SetReach(key);
        AchievementData.SaveData(_data);
    }
}
