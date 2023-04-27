using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PurchasedPropsAndPlants
{
    public int WaveIndex;
    public int Money;
    public int Sun;
    public int FlowerPotCount;
    public List<PropCard> PurchasedProps;
    public List<PlantAttribute> PlantAttributes;
    public int MaxSolt = 2;
    public List<int> SoltIndex;  // 卡槽对应在PlantAttributes中,list值对应下标，形成新的引用
    public List<CraterPos> CraterPoses;  // 毁灭菇造成的坑
    public List<string> earth; // 花盆的泥土位置信息
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

    public SystemData SystemData { get; protected set; } = new SystemData();

    /// <summary>
    ///  判断是否有成功读取，没有则是重新开始
    /// </summary>
    public bool IsLoadUserData { get; set; }

    /// <summary>
    /// 打打僵王模式
    /// </summary>
    public bool IsBossMode { get; set; }

    private SaveManager()
    {
        LoadData();
    }

    private void LoadData()
    {
        string soundsVolumeDataStr = PlayerPrefs.GetString("SystemData");
        if (!string.IsNullOrEmpty(soundsVolumeDataStr))
        {
            SystemData = JsonUtility.FromJson<SystemData>(soundsVolumeDataStr);
        }
    }

    public void SaveSystemData()
    {
        PlayerPrefs.SetString("SystemData", JsonUtility.ToJson(SystemData));
    }

    public void LoadUserData()
    {
        // 打打僵王模式不进行数据读取
        IsBossMode = PlayerPrefs.GetInt("BossMode", 0) == 1;
        DeleteBossMode();
        if (IsBossMode)
        {
            IsLoadUserData = true;
            return;
        }

        string userDataStr = PlayerPrefs.GetString("UserData");
        if (!string.IsNullOrEmpty(userDataStr))
        {
            GameManager.Instance.UserData = JsonUtility.FromJson<UserData>(userDataStr);
            IsLoadUserData = true;
        }

        string saveDataStructStr = PlayerPrefs.GetString("PurchasedPropsAndPlants");
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
        string userDataStr = PlayerPrefs.GetString("UserData");
        if (!string.IsNullOrEmpty(userDataStr))
        {
            result = true;
        }
        return result;
    }

    public void SaveUserData()
    {
        PlayerPrefs.SetString("UserData", JsonUtility.ToJson(GameManager.Instance.UserData));

        PurchasedPropsAndPlants saveDataStruct = new PurchasedPropsAndPlants();
        saveDataStruct.PurchasedProps = ShopManager.Instance.PurchasedProps;
        saveDataStruct.PlantAttributes = GardenManager.Instance.PlantAttributes;
        saveDataStruct.Money = ShopManager.Instance.Money;
        saveDataStruct.FlowerPotCount = GardenManager.Instance.FlowerPotCount;
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
        PlayerPrefs.SetString("PurchasedPropsAndPlants", JsonUtility.ToJson(saveDataStruct));
    }

    /// <summary>
    /// 开始游戏时点击重新开始,或者暂停页面，或者死亡则删除存档
    /// </summary>
    public void DeleteUserData()
    {
        // 打打僵王模式不删除原存档
        if (IsBossMode)
            return;
        PlayerPrefs.DeleteKey("UserData");
        PlayerPrefs.DeleteKey("PurchasedPropsAndPlants");
    }

    /// <summary>
    /// 设置打打僵王模式
    /// </summary>
    public void SetBossMode()
    {
        PlayerPrefs.SetInt("BossMode", 1);
    }

    /// <summary>
    /// 加载完之后就关闭打打僵王模式
    /// </summary>
    public void DeleteBossMode()
    {
        PlayerPrefs.DeleteKey("BossMode");
    }
}
