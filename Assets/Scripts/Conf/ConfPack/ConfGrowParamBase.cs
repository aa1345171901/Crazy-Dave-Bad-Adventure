using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfGrowParamItem : ConfBaseItem
{
	/// <summary>
	/// 僵尸类型
	/// </summary>
	public int type;

	/// <summary>
	/// 死亡掉落成长道具数量
	/// </summary>
	public int price;


	public ConfGrowParamItem()
	{
	}

	public ConfGrowParamItem(int id, int type, int price)
	{
		this.id = id;
		this.type = type;
		this.price = price;
	}	

	public ConfGrowParamItem Clone()
	{
		ConfGrowParamItem item = (ConfGrowParamItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfGrowParamBase : ConfBase<ConfGrowParamItem>
{
    public override void Init()
    {
		confName = "GrowParam";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfGrowParamItem(1, 0, 1));
		AddItem(new ConfGrowParamItem(2, 1, 3));
		AddItem(new ConfGrowParamItem(3, 2, 4));
		AddItem(new ConfGrowParamItem(4, 3, 4));
		AddItem(new ConfGrowParamItem(5, 4, 2));
		AddItem(new ConfGrowParamItem(6, 5, 5));
		AddItem(new ConfGrowParamItem(7, 6, 2));
		AddItem(new ConfGrowParamItem(8, 7, 1));
		AddItem(new ConfGrowParamItem(9, 8, 1));
		AddItem(new ConfGrowParamItem(10, 9, 5));
		AddItem(new ConfGrowParamItem(11, 10, 5));
		AddItem(new ConfGrowParamItem(12, 11, 50));
		AddItem(new ConfGrowParamItem(13, 12, 100));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfGrowParamItem GetItem(int id)
	{
		return GetItemObject<ConfGrowParamItem>(id);
	}
	
}
	
