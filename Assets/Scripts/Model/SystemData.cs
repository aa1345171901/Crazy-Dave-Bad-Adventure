using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemData
{
    /// <summary>
    /// 背景音乐大小
    /// </summary>
    public float MusicVolume = 1;

    /// <summary>
    /// 音效大小
    /// </summary>
    public float SoundEffectVolume = 1;

    /// <summary>
    /// 是否开启伤害显示
    /// </summary>
    public bool IsHUD = true;

    public string language;

    private static readonly string systemDataPath = Application.persistentDataPath + "/SaveData/SystemData.data";

    public static SystemData LoadData()
    {
        SystemData systemData = new SystemData();
        string systemDataStr = FileTool.ReadText(systemDataPath);
        Debug.Log("read systemDataStr:" + systemDataStr);
        if (!string.IsNullOrEmpty(systemDataStr))
        {
            systemData = JsonUtility.FromJson<SystemData>(systemDataStr);
        }
        return systemData;
    }

    public static void SaveData(SystemData systemData)
    {
        string systemDataStr = JsonUtility.ToJson(systemData);
        Debug.Log("systemDataStr:" + systemDataStr);
        FileTool.WriteText(systemDataPath, systemDataStr);
    }
}
