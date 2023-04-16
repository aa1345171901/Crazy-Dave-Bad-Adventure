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
    public List<int> SoltIndex;  // ���۶�Ӧ��PlantAttributes��,listֵ��Ӧ�±꣬�γ��µ�����
    public List<CraterPos> CraterPoses;  // ������ɵĿ�
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
    ///  �ж��Ƿ��гɹ���ȡ��û���������¿�ʼ
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
            int index = 0;
            for (int i = 0; i < GardenManager.Instance.PlantAttributes.Count; i++)
            {
                if (GardenManager.Instance.PlantAttributes[i] == GardenManager.Instance.CardslotPlant[index])
                {
                    soltPlantIndex.Add(i);
                    index++;
                }
                if (index >= GardenManager.Instance.CardslotPlant.Count)
                    break;
            }
        }
        saveDataStruct.SoltIndex = soltPlantIndex;
        saveDataStruct.CraterPoses = GardenManager.Instance.CraterPoses;
        PlayerPrefs.SetString("PurchasedPropsAndPlants", JsonUtility.ToJson(saveDataStruct));
    }

    /// <summary>
    /// ��ʼ��Ϸʱ������¿�ʼ,������ͣҳ�棬����������ɾ���浵
    /// </summary>
    public void DeleteUserData()
    {
        PlayerPrefs.DeleteKey("UserData");
        PlayerPrefs.DeleteKey("PurchasedPropsAndPlants");
    }
}
