using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfMoneyParamItem : ConfBaseItem
{
	/// <summary>
	/// 键值
	/// </summary>
	public string key;

	/// <summary>
	/// 价值
	/// </summary>
	public int price;

	/// <summary>
	/// 掉落基础权重不算幸运，总3000
	/// </summary>
	public int weight;

	/// <summary>
	/// 存在时间
	/// </summary>
	public float time;


	public ConfMoneyParamItem()
	{
	}

	public ConfMoneyParamItem(int id, string key, int price, int weight, float time)
	{
		this.id = id;
		this.key = key;
		this.price = price;
		this.weight = weight;
		this.time = time;
	}	

	public ConfMoneyParamItem Clone()
	{
		ConfMoneyParamItem item = (ConfMoneyParamItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfMoneyParamBase : ConfBase<ConfMoneyParamItem>
{
    public override void Init()
    {
		confName = "MoneyParam";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfMoneyParamItem(1, "sliverCoin", 10, 120, 15f));
		AddItem(new ConfMoneyParamItem(2, "goldCoin", 50, 5, 20f));
		AddItem(new ConfMoneyParamItem(3, "diamond", 150, 1, 25f));
		AddItem(new ConfMoneyParamItem(4, "sun", 25, 100, 15f));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfMoneyParamItem GetItem(int id)
	{
		return GetItemObject<ConfMoneyParamItem>(id);
	}
	
}
	
