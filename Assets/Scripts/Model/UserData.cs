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
}

/// Json 属性增加字符串复制
/*
                {
                "attributeTypeString": "MaximumHP",
                "increment": "1"
                },

                {
                "attributeTypeString": "LifeRecovery",
                "increment": "1"
                },

                {
                "attributeTypeString": "Adrenaline",
                "increment": "1"
                },

                {
                "attributeTypeString": "Power",
                "increment": "1"
                },

                {
                "attributeTypeString": "PercentageDamage",
                "increment": "1"
                },

                {
                "attributeTypeString": "AttackSpeed",
                "increment": "1"
                },

                {
                "attributeTypeString": "Range",
                "increment": "1"
                },

                {
                "attributeTypeString": "CriticalHitRate",
                "increment": "1"
                },

                {
                "attributeTypeString": "Speed",
                "increment": "1"
                },

                {
                "attributeTypeString": "Armor",
                "increment": "1"
                },

                {
                "attributeTypeString": "Lucky",
                "increment": "1"
                },

                {
                "attributeTypeString": "Sunshine",
                "increment": "1"
                },

                {
                "attributeTypeString": "GoldCoins",
                "increment": "1"
                },

                {
                "attributeTypeString": "Botany",
                "increment": "1"
                }
 */
