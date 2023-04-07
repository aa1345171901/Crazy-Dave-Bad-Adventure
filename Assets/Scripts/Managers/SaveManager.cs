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
            GardenManager.Instance.Sun = saveDataStruct.Sun;
            LevelManager.Instance.IndexWave = saveDataStruct.WaveIndex;
            GardenManager.Instance.IsLoadPlantData = true;
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
        PlayerPrefs.SetString("PurchasedPropsAndPlants", JsonUtility.ToJson(saveDataStruct));
    }

    /// <summary>
    /// 开始游戏时点击重新开始,或者暂停页面，或者死亡则删除存档
    /// </summary>
    public void DeleteUserData()
    {
        PlayerPrefs.DeleteKey("UserData");
        PlayerPrefs.DeleteKey("PurchasedPropsAndPlants");
    }
}
