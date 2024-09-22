using System;

[Serializable]
public enum AttributeType
{
    MaximumHP,
    LifeRecovery,
    Adrenaline,
    Power,
    PercentageDamage,
    AttackSpeed,
    Range,
    CriticalHitRate,
    Speed,
    Armor,
    Lucky,
    Sunshine,
    GoldCoins,
    Botany
}

[Serializable]
public class UserData
{
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int MaximumHP = 10;

    /// <summary>
    /// 生命恢复
    /// </summary>
    public int LifeRecovery;

    /// <summary>
    /// 肾上腺素
    /// </summary>
    public int Adrenaline;

    /// <summary>
    /// 力量
    /// </summary>
    public int Power;

    /// <summary>
    /// 伤害
    /// </summary>
    public int PercentageDamage;

    /// <summary>
    /// 攻击速度
    /// </summary>
    public int AttackSpeed;

    /// <summary>
    /// 范围
    /// </summary>
    public int Range;

    /// <summary>
    /// 暴击率
    /// </summary>
    public int CriticalHitRate;

    /// <summary>
    /// 移动速度
    /// </summary>
    public int Speed;

    /// <summary>
    /// 护甲
    /// </summary>
    public int Armor;

    /// <summary>
    /// 幸运
    /// </summary>
    public int Lucky;

    /// <summary>
    /// 阳光
    /// </summary>
    public int Sunshine;

    /// <summary>
    /// 金币
    /// </summary>
    public int GoldCoins;

    /// <summary>
    /// 植物学
    /// </summary>
    public int Botany;

    public UserData()
    {
        if (ConfManager.Instance == null)
            return;
        MaximumHP = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("maximumhp").value;
        LifeRecovery = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("liferecovery").value;
        Adrenaline = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("adrenaline").value;
        Power = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("power").value;
        PercentageDamage = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("percentagedamage").value;
        AttackSpeed = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("attackspeed").value;
        Range = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("range").value;
        CriticalHitRate = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("criticalhitrate").value;
        Speed = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("movespeed").value;
        Armor = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("armor").value;
        Lucky = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("lucky").value;
        Sunshine = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("sunshine").value;
        GoldCoins = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("goldcoins").value;
        Botany = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey("botany").value;
    }

    public void AddValue(string key, int value)
    {
        switch (key)
        {
            case "maximumhp":
                MaximumHP += value;
                break;
            case "liferecovery":
                LifeRecovery += value;
                break;
            case "adrenaline":
                Adrenaline += value;
                break;
            case "power":
                Power += value;
                break;
            case "percentagedamage":
                PercentageDamage += value;
                break;
            case "attackspeed":
                AttackSpeed += value;
                break;
            case "range":
                Range += value;
                break;
            case "criticalhitrate":
                CriticalHitRate += value;
                break;
            case "speed":
                Speed += value;
                break;
            case "armor":
                Armor += value;
                break;
            case "lucky":
                Lucky += value;
                break;
            case "sunshine":
                Sunshine += value;
                break;
            case "goldcoins":
                GoldCoins += value;
                break;
            case "botany":
                Botany += value;
                break;
        }
    }
}
