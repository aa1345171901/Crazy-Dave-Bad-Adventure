using UnityEngine;

public class ExternalGrowthData
{
    /// <summary>
    /// 现在head的数量
    /// </summary>
    public int headNum;

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
