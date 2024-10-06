using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfBasicAttributeItem : ConfBaseItem
{
	/// <summary>
	/// 角色
	/// </summary>
	public string character;

	/// <summary>
	/// 最大生命值
	/// </summary>
	public int MaximumHP;

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


	public ConfBasicAttributeItem()
	{
	}

	public ConfBasicAttributeItem(int id, string character, int MaximumHP, int LifeRecovery, int Adrenaline, int Power, int PercentageDamage, int AttackSpeed, int Range, int CriticalHitRate, int CriticalDamage, int Speed, int Armor, int Lucky, int Sunshine, int GoldCoins, int Botany)
	{
		this.id = id;
		this.character = character;
		this.MaximumHP = MaximumHP;
		this.LifeRecovery = LifeRecovery;
		this.Adrenaline = Adrenaline;
		this.Power = Power;
		this.PercentageDamage = PercentageDamage;
		this.AttackSpeed = AttackSpeed;
		this.Range = Range;
		this.CriticalHitRate = CriticalHitRate;
		this.CriticalDamage = CriticalDamage;
		this.Speed = Speed;
		this.Armor = Armor;
		this.Lucky = Lucky;
		this.Sunshine = Sunshine;
		this.GoldCoins = GoldCoins;
		this.Botany = Botany;
	}	

	public ConfBasicAttributeItem Clone()
	{
		ConfBasicAttributeItem item = (ConfBasicAttributeItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfBasicAttributeBase : ConfBase<ConfBasicAttributeItem>
{
    public override void Init()
    {
		confName = "BasicAttribute";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfBasicAttributeItem(1, "dave", 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfBasicAttributeItem GetItem(int id)
	{
		return GetItemObject<ConfBasicAttributeItem>(id);
	}
	
}
	
