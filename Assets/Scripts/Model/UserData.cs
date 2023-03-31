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
    /// �������ֵ
    /// </summary>
    public int MaximumHP = 10;

    /// <summary>
    /// �����ָ�
    /// </summary>
    public int LifeRecovery;

    /// <summary>
    /// ��������
    /// </summary>
    public int Adrenaline;

    /// <summary>
    /// ����
    /// </summary>
    public int Power;

    /// <summary>
    /// �˺�
    /// </summary>
    public int PercentageDamage;

    /// <summary>
    /// �����ٶ�
    /// </summary>
    public int AttackSpeed;

    /// <summary>
    /// ��Χ
    /// </summary>
    public int Range;

    /// <summary>
    /// ������
    /// </summary>
    public int CriticalHitRate;

    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    public int Speed;

    /// <summary>
    /// ����
    /// </summary>
    public int Armor;

    /// <summary>
    /// ����
    /// </summary>
    public int Lucky;

    /// <summary>
    /// ����
    /// </summary>
    public int Sunshine;

    /// <summary>
    /// ���
    /// </summary>
    public int GoldCoins;

    /// <summary>
    /// ֲ��ѧ
    /// </summary>
    public int Botany;
}

/// Json ���������ַ�������
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
