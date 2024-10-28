using UnityEngine;

/// <summary>
/// 游玩模式
/// </summary>
public enum BattleMode
{
    /// <summary>
    /// 普通冒险模式
    /// </summary>
    None,
    /// <summary>
    /// 道具模式
    /// </summary>
    PropMode,
    /// <summary>
    /// 植物模式
    /// </summary>
    PlantMode,
    /// <summary>
    /// 角色生存模式
    /// </summary>
    PlayerMode,
}

public class SpecialData
{
    /// <summary>
    /// 模式
    /// </summary>
    public int modeInt;
    /// <summary>
    /// 模式参数
    /// </summary>
    public int modeValue;

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