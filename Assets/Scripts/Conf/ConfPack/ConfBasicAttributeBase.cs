using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfBasicAttributeItem : ConfBaseItem
{
	/// <summary>
	/// 属性
	/// </summary>
	public string attribute;

	/// <summary>
	/// 基础数值
	/// </summary>
	public int value;


	public ConfBasicAttributeItem()
	{
	}

	public ConfBasicAttributeItem(int id, string attribute, int value)
	{
		this.id = id;
		this.attribute = attribute;
		this.value = value;
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
		AddItem(new ConfBasicAttributeItem(1, "maximumhp", 20));
		AddItem(new ConfBasicAttributeItem(2, "liferecovery", 0));
		AddItem(new ConfBasicAttributeItem(3, "adrenaline", 0));
		AddItem(new ConfBasicAttributeItem(4, "power", 0));
		AddItem(new ConfBasicAttributeItem(5, "percentagedamage", 0));
		AddItem(new ConfBasicAttributeItem(6, "attackspeed", 0));
		AddItem(new ConfBasicAttributeItem(7, "range", 0));
		AddItem(new ConfBasicAttributeItem(8, "criticalhitrate", 0));
		AddItem(new ConfBasicAttributeItem(9, "movespeed", 0));
		AddItem(new ConfBasicAttributeItem(10, "armor", 0));
		AddItem(new ConfBasicAttributeItem(11, "lucky", 0));
		AddItem(new ConfBasicAttributeItem(12, "sunshine", 0));
		AddItem(new ConfBasicAttributeItem(13, "goldcoins", 0));
		AddItem(new ConfBasicAttributeItem(14, "botany", 0));
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
	
