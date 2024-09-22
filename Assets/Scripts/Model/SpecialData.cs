using UnityEngine;

/// <summary>
/// 游玩模式
/// </summary>
public enum BattleMode
{
    None,
    BossMode,
}

public class SpecialData
{
    /// <summary>
    /// 模式
    /// </summary>
    public int modeInt;

    public BattleMode battleMode;

    private static readonly string specialDataPath = Application.persistentDataPath + "/SaveData/SpecailData.data";

    public static SpecialData LoadData()
    {
        SpecialData specialData = new SpecialData();
        string specialDataStr = FileTool.ReadText(specialDataPath);
        Debug.Log("read specialDataStr:" + specialDataStr);
        if (!string.IsNullOrEmpty(specialDataStr))
        {
            specialData = JsonUtility.FromJson<SpecialData>(specialDataStr);
            specialData.battleMode = (BattleMode)specialData.modeInt;
        }
        return specialData;
    }

    public static void SaveData(SpecialData specialData)
    {
        string specialDataStr = JsonUtility.ToJson(specialData);
        Debug.Log("specialDataStr:" + specialDataStr);
        FileTool.WriteText(specialDataPath, specialDataStr);
    }
}