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
    CriticalDamage,
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
    /// 暴击伤害
    /// </summary>
    public int CriticalDamage;

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

    public string characterName;

    public UserData(string characterName)
    {
        if (ConfManager.Instance == null)
            return;

        this.characterName = characterName;
        var confItem = ConfManager.Instance.confMgr.basicAttribute.GetItemByKey(characterName);
        MaximumHP = confItem.MaximumHP;
        LifeRecovery = confItem.LifeRecovery;
        Adrenaline = confItem.Adrenaline;
        Power = confItem.Power;
        PercentageDamage = confItem.PercentageDamage;
        AttackSpeed = confItem.AttackSpeed;
        Range = confItem.Range;
        CriticalHitRate = confItem.CriticalHitRate;
        CriticalDamage = confItem.CriticalDamage;
        Speed = confItem.Speed;
        Armor = confItem.Armor;
        Lucky = confItem.Lucky;
        Sunshine = confItem.Sunshine;
        GoldCoins = confItem.GoldCoins;
        Botany = confItem.Botany;
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
            case "criticaldamage":
                CriticalDamage += value;
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
