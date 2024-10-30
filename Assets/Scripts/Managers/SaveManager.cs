using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopDownPlate;
using UnityEngine;

/// <summary>
/// 战斗中除角色外数据
/// </summary>
public class BattleData
{
    public UserData userData;

    public int WaveIndex;
    public int Money;
    public int Sun;
    public int FlowerPotCount;
    /// <summary>
    /// 本次通过击杀僵尸获取的头数量
    /// </summary>
    public int HeadNum;
    public List<PropCard> PurchasedProps;
    public List<int> PurchasedPlantEvolutionKeys;
    public List<int> PurchasedPlantEvolutionValues;
    ///// <summary>
    ///// 购买的进化卡
    ///// </summary>
    //public Dictionary<PlantType, int> PurchasedPlantEvolutionDicts;
    public List<PlantAttribute> PlantAttributes;
    public int SlotNum = 2;
    public List<int> SoltIndex;  // 卡槽对应在PlantAttributes中,list值对应下标，形成新的引用
    public List<CraterPos> CraterPoses;  // 毁灭菇造成的坑
    public List<string> earth; // 花盆的泥土位置信息

    /// <summary>
    /// 已经使用的复活次数
    /// </summary>
    public int resurrection;

    /// <summary>
    /// 伤害统计， key damageType，伤害类型， value 数值
    /// </summary>
    public List<TypeIntData> damageStatistics;

    /// <summary>
    /// 承受伤害统计， key < 1000 zombieType，僵尸伤害，>1000 damageType + 1000  value 数值
    /// </summary>
    public List<TypeIntData> takingDamageStatistics;
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

    /// <summary>
    /// 系统数据
    /// </summary>
    public SystemData systemData { get; protected set; } = new SystemData();

    /// <summary>
    /// 特殊关卡数据
    /// </summary>
    public SpecialData specialData { get; protected set; } = new SpecialData();

    /// <summary>
    /// 局外成长数据
    /// </summary>
    public ExternalGrowthData externalGrowthData { get; protected set; }

    /// <summary>
    ///  判断是否有成功读取，没有则是重新开始
    /// </summary>
    public bool IsLoadUserData { get; set; }

    private readonly string battleDataPath = Application.persistentDataPath + "/SaveData/BattleData.data";

    private SaveManager()
    {
        LoadData();
    }

    /// <summary>
    /// 读取系统数据、外部成长数据、特殊模式数据
    /// </summary>
    private void LoadData()
    {
        systemData = SystemData.LoadData();

        externalGrowthData = ExternalGrowthData.LoadData();

        specialData = SpecialData.LoadData();
    }

    /// <summary>
    /// 保存外部成长数据
    /// </summary>
    public void SaveExternalGrowData()
    {
        ExternalGrowthData.SaveData(externalGrowthData);
    }

    /// <summary>
    /// 保存系统数据
    /// </summary>
    public void SaveSystemData()
    {
        SystemData.SaveData(systemData);
    }

    /// <summary>
    /// 开始游戏时点击重新开始,或者暂停页面，或者死亡则删除存档
    /// </summary>
    public void DeleteUserData()
    {
        switch (specialData.battleMode)
        {
            case BattleMode.None:
                // 备份战斗存档上一次的一份
                if (FileTool.FileExists(battleDataPath))
                    GameManager.Instance.canBackInTime = true;
                FileTool.FileMove(battleDataPath, battleDataPath.Replace("BattleData.data", "BattleData_(beifen).data"));
                break;
            case BattleMode.PropMode:
                break;
            case BattleMode.PlantMode:
                break;
            case BattleMode.PlayerMode:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 回档
    /// </summary>
    public void BackInTime()
    {
        FileTool.FileMove(battleDataPath.Replace("BattleData.data", "BattleData_(beifen).data"), battleDataPath);
    }

    /// <summary>
    /// 设置游戏模式
    /// </summary>
    public void SetSpecialMode(BattleMode battleMode, int modeValue = 0)
    {
        specialData.modeInt = (int)battleMode;
        specialData.battleMode = battleMode;
        specialData.modeValue = modeValue;
        //SpecialData.SaveData(specialData);
    }

    /// <summary>
    /// 读取选择的模式
    /// </summary>
    public BattleMode LoadSpecialData()
    {
        damageStatistics = new List<TypeIntData>();
        takingDamageStatistics = new List<TypeIntData>();

        var nowMode = specialData.battleMode;
        GameManager.Instance.canBackInTime = false;
        switch (nowMode)
        {
            case BattleMode.None:
                LoadUserData();
                break;
            default:
                break;
        }
        return nowMode;
    }

    /// <summary>
    /// 读取普通冒险的战斗数据
    /// </summary>
    public void LoadUserData()
    {
        string saveDataStructStr = FileTool.ReadText(battleDataPath);
        Debug.Log("read battleDataStructStr:" + saveDataStructStr);
        if (!string.IsNullOrEmpty(saveDataStructStr))
        {
            BattleData saveDataStruct = JsonUtility.FromJson<BattleData>(saveDataStructStr);

            IsLoadUserData = true;
            GameManager.Instance.UserData = saveDataStruct.userData;

            ShopManager.Instance.PurchasedProps = saveDataStruct.PurchasedProps;
            GardenManager.Instance.PlantAttributes = saveDataStruct.PlantAttributes;
            ShopManager.Instance.Money = saveDataStruct.Money;
            GardenManager.Instance.FlowerPotCount = saveDataStruct.FlowerPotCount;
            GardenManager.Instance.WaterFlowerPotCount = ShopManager.Instance.PurchasePropCount("Pot_Water");
            GardenManager.Instance.Sun = saveDataStruct.Sun;
            LevelManager.Instance.IndexWave = saveDataStruct.WaveIndex;
            GameManager.Instance.HeadNum = saveDataStruct.HeadNum;
            GardenManager.Instance.IsLoadPlantData = true;
            GardenManager.Instance.SlotNum = saveDataStruct.SlotNum;
            foreach (var item in saveDataStruct.SoltIndex)
            {
                GardenManager.Instance.CardslotPlant.Add(saveDataStruct.PlantAttributes[item]);
            }
            GardenManager.Instance.CraterPoses = saveDataStruct.CraterPoses;
            GardenManager.Instance.earth = saveDataStruct.earth;
            GardenManager.Instance.MaxFlowerPotCount += saveDataStruct.earth.Count;

            // 购买的进化卡
            foreach (var item in saveDataStruct.PurchasedPlantEvolutionKeys)
            {
                var key = (PlantType)item;
                int index = saveDataStruct.PurchasedPlantEvolutionKeys.IndexOf(item);
                if (index != -1)
                    ShopManager.Instance.PurchasedPlantEvolutionDicts[key] = saveDataStruct.PurchasedPlantEvolutionValues[index];
            }

            GameManager.Instance.resurrection = saveDataStruct.resurrection;
            
            damageStatistics = saveDataStruct.damageStatistics;
            takingDamageStatistics = saveDataStruct.takingDamageStatistics;
        }
    }

    /// <summary>
    /// 判断是否有普通冒险的战斗数据
    /// </summary>
    /// <returns></returns>
    public bool JudgeData()
    {
        bool result = false;
        if (FileTool.FileExists(battleDataPath))
        {
            result = true;
        }
        return result;
    }

    /// <summary>
    /// 普通冒险的战斗数据
    /// </summary>
    public void SaveUserData()
    {
        if (specialData.battleMode != BattleMode.None)
            return;

        BattleData saveDataStruct = new BattleData();

        saveDataStruct.userData = GameManager.Instance.UserData;

        saveDataStruct.PurchasedProps = ShopManager.Instance.PurchasedProps;
        saveDataStruct.PlantAttributes = GardenManager.Instance.PlantAttributes;
        saveDataStruct.Money = ShopManager.Instance.Money;
        saveDataStruct.FlowerPotCount = GardenManager.Instance.FlowerPotCount;
        saveDataStruct.HeadNum = GameManager.Instance.HeadNum;
        saveDataStruct.Sun = GardenManager.Instance.Sun;
        saveDataStruct.WaveIndex = LevelManager.Instance.IndexWave;
        saveDataStruct.SlotNum = GardenManager.Instance.SlotNum;
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

        // 购买的进化卡
        var keys = new List<int>();
        var values = new List<int>();
        foreach (var item in ShopManager.Instance.PurchasedPlantEvolutionDicts)
        {
            keys.Add((int)item.Key);
            values.Add((int)item.Value);
        }
        saveDataStruct.PurchasedPlantEvolutionKeys = keys;
        saveDataStruct.PurchasedPlantEvolutionValues = values;

        saveDataStruct.CraterPoses = GardenManager.Instance.CraterPoses;
        saveDataStruct.earth = GardenManager.Instance.earth;

        saveDataStruct.resurrection = GameManager.Instance.resurrection;

        saveDataStruct.damageStatistics = damageStatistics;
        saveDataStruct.takingDamageStatistics = takingDamageStatistics;

        string battleDataPathStr = JsonUtility.ToJson(saveDataStruct);
        Debug.Log("battleDataPathStr:" + battleDataPathStr);
        FileTool.WriteText(battleDataPath, battleDataPathStr);
    }

    #region 伤害统计
    /// <summary>
    /// 伤害统计， key damageType，伤害类型， value 数值
    /// </summary>
    public List<TypeIntData> damageStatistics;

    /// <summary>
    /// 承受伤害统计， key < 1000 zombieType，僵尸伤害，>1000 damageType + 1000  value 数值
    /// </summary>
    public List<TypeIntData> takingDamageStatistics;

    public void AddDamageValue(int damageType, int value)
    {
        var list = damageStatistics.Where((e) => e.key == damageType);
        var data = list.Count() == 0 ? null : list.First();
        if (data == null)
        {
            damageStatistics.Add(new TypeIntData(damageType, value));
        }
        else
        {
            data.value += value;
        }
    }

    public void AddTakingDamageValue(int zombieType, int value)
    {
        var list = takingDamageStatistics.Where((e) => e.key == zombieType);
        var data = list.Count() == 0 ? null : list.First();
        if (data == null)
        {
            takingDamageStatistics.Add(new TypeIntData(zombieType, value));
        }
        else
        {
            data.value += value;
        }
    }
    #endregion
}
