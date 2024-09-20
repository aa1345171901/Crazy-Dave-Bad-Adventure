using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

/// <summary>
/// 战斗中除角色外数据
/// </summary>
public class PurchasedPropsAndPlants
{
    public int WaveIndex;
    public int Money;
    public int Sun;
    public int FlowerPotCount;
    /// <summary>
    /// 本次通过击杀僵尸获取的头数量
    /// </summary>
    public int HeadNum;
    public List<PropCard> PurchasedProps;
    public List<PlantAttribute> PlantAttributes;
    public int MaxSolt = 2;
    public List<int> SoltIndex;  // 卡槽对应在PlantAttributes中,list值对应下标，形成新的引用
    public List<CraterPos> CraterPoses;  // 毁灭菇造成的坑
    public List<string> earth; // 花盆的泥土位置信息
}

public class SpecialData
{
    /// <summary>
    /// 打打僵王模式
    /// </summary>
    public int bossMode;
}

public class SaveManager
{
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SaveManager();
            return _instance;
        }
    }

    public SystemData systemData { get; protected set; } = new SystemData();

    public SpecialData specialData { get; protected set; } = new SpecialData();

    /// <summary>
    /// 局外成长数据
    /// </summary>
    public ExternalGrowthData externalGrowthData { get; protected set; }

    /// <summary>
    ///  判断是否有成功读取，没有则是重新开始
    /// </summary>
    public bool IsLoadUserData { get; set; }

    /// <summary>
    /// 打打僵王模式
    /// </summary>
    public bool IsBossMode { get; set; }

    private readonly string userDataPath = Application.persistentDataPath + "/SaveData/UserData.data";
    private readonly string itemsDataPath = Application.persistentDataPath + "/SaveData/ItemsData.data";
    private readonly string specialDataPath = Application.persistentDataPath + "/SaveData/SpecailData.data";
    private readonly string systemDataPath = Application.persistentDataPath + "/SaveData/SystemData.data";

    private SaveManager()
    {
        LoadData();
    }

    private void LoadData()
    {
        string systemDataStr = FileTool.ReadText(systemDataPath);
        Debug.Log("read systemDataStr:" + systemDataStr);
        if (!string.IsNullOrEmpty(systemDataStr))
        {
            systemData = JsonUtility.FromJson<SystemData>(systemDataStr);
        }
        externalGrowthData = ExternalGrowthData.LoadData();
    }

    public void SaveExternalGrowData()
    {
        ExternalGrowthData.SaveSystemData(externalGrowthData);
    }

    public void SaveSystemData()
    {
        string systemDataStr = JsonUtility.ToJson(systemData);
        Debug.Log("systemDataStr:" + systemDataStr);
        FileTool.WriteText(systemDataPath, systemDataStr);
    }

    public void LoadUserData()
    {
        string specialDataStr = FileTool.ReadText(specialDataPath);
        Debug.Log("read specialDataStr:" + specialDataStr);
        if (!string.IsNullOrEmpty(specialDataStr))
        {
            specialData = JsonUtility.FromJson<SpecialData>(specialDataStr);
        }
        // 打打僵王模式不进行数据读取
        IsBossMode = specialData.bossMode == 1;
        SetBossMode(0);
        if (IsBossMode)
        {
            IsLoadUserData = true;
            return;
        }

        string userDataStr = FileTool.ReadText(userDataPath);
        Debug.Log("read userDataStr:" + userDataStr);
        if (!string.IsNullOrEmpty(userDataStr))
        {
            GameManager.Instance.UserData = JsonUtility.FromJson<UserData>(userDataStr);
            IsLoadUserData = true;
        }

        string saveDataStructStr = FileTool.ReadText(itemsDataPath);
        Debug.Log("read saveDataStructStr:" + saveDataStructStr);
        if (!string.IsNullOrEmpty(saveDataStructStr))
        {
            PurchasedPropsAndPlants saveDataStruct = JsonUtility.FromJson<PurchasedPropsAndPlants>(saveDataStructStr);
            ShopManager.Instance.PurchasedProps = saveDataStruct.PurchasedProps;
            GardenManager.Instance.PlantAttributes = saveDataStruct.PlantAttributes;
            ShopManager.Instance.Money = saveDataStruct.Money;
            GardenManager.Instance.FlowerPotCount = saveDataStruct.FlowerPotCount;
            GardenManager.Instance.WaterFlowerPotCount = ShopManager.Instance.PurchasePropCount("Pot_Water");
            GardenManager.Instance.Sun = saveDataStruct.Sun;
            LevelManager.Instance.IndexWave = saveDataStruct.WaveIndex;
            GameManager.Instance.HeadNum = saveDataStruct.HeadNum;
            GardenManager.Instance.IsLoadPlantData = true;
            GardenManager.Instance.MaxSlot = saveDataStruct.MaxSolt;
            foreach (var item in saveDataStruct.SoltIndex)
            {
                GardenManager.Instance.CardslotPlant.Add(saveDataStruct.PlantAttributes[item]);
            }
            GardenManager.Instance.CraterPoses = saveDataStruct.CraterPoses;
            GardenManager.Instance.earth = saveDataStruct.earth;
            GardenManager.Instance.MaxFlowerPotCount += saveDataStruct.earth.Count;
        }
    }

    public bool JudgeData()
    {
        bool result = false;
        if (FileTool.FileExists(userDataPath))
        {
            result = true;
        }
        return result;
    }

    public void SaveUserData()
    {
        string userDataStr = JsonUtility.ToJson(GameManager.Instance.UserData);
        Debug.Log("userDataStr:" + userDataStr);
        FileTool.WriteText(userDataPath, userDataStr);

        PurchasedPropsAndPlants saveDataStruct = new PurchasedPropsAndPlants();
        saveDataStruct.PurchasedProps = ShopManager.Instance.PurchasedProps;
        saveDataStruct.PlantAttributes = GardenManager.Instance.PlantAttributes;
        saveDataStruct.Money = ShopManager.Instance.Money;
        saveDataStruct.FlowerPotCount = GardenManager.Instance.FlowerPotCount;
        saveDataStruct.HeadNum = GameManager.Instance.HeadNum;
        saveDataStruct.Sun = GardenManager.Instance.Sun;
        saveDataStruct.WaveIndex = LevelManager.Instance.IndexWave;
        saveDataStruct.MaxSolt = GardenManager.Instance.MaxSlot;
        List<int> soltPlantIndex = new List<int>();
        if (GardenManager.Instance.CardslotPlant.Count > 0)
        {
            for (int i = 0; i < GardenManager.Instance.CardslotPlant.Count; i++)
            {
                if (GardenManager.Instance.PlantAttributes.Contains(GardenManager.Instance.CardslotPlant[i]))
                {
                    soltPlantIndex.Add(GardenManager.Instance.PlantAttributes.IndexOf(GardenManager.Instance.CardslotPlant[i]));
                }
            }
        }
        saveDataStruct.SoltIndex = soltPlantIndex;
        saveDataStruct.CraterPoses = GardenManager.Instance.CraterPoses;
        saveDataStruct.earth = GardenManager.Instance.earth;
        string itemsDataStr = JsonUtility.ToJson(saveDataStruct);
        Debug.Log("itemsDataStr:" + itemsDataStr);
        FileTool.WriteText(itemsDataPath, itemsDataStr);
    }

    /// <summary>
    /// 开始游戏时点击重新开始,或者暂停页面，或者死亡则删除存档
    /// </summary>
    public void DeleteUserData()
    {
        // 打打僵王模式不删除原存档
        if (IsBossMode)
            return;
        // 备份战斗存档上一次的一份
        FileTool.FileMove(userDataPath, userDataPath.Replace("UserData.data", "UserData1.data"));
        FileTool.FileMove(itemsDataPath, itemsDataPath.Replace("ItemsData.data", "ItemsData1.data"));
    }

    /// <summary>
    /// 设置打打僵王模式
    /// </summary>
    public void SetBossMode(int bossMode = 1)
    {
        specialData.bossMode = bossMode;
        string specialDataStr = JsonUtility.ToJson(specialData);
        Debug.Log("specialDataStr:" + specialDataStr);
        FileTool.WriteText(specialDataPath, specialDataStr);
    }
}
