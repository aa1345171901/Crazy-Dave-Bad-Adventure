using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfGameIntParamItem : ConfBaseItem
{
	/// <summary>
	/// 键值
	/// </summary>
	public string key;

	/// <summary>
	/// 值
	/// </summary>
	public int value;


	public ConfGameIntParamItem()
	{
	}

	public ConfGameIntParamItem(int id, string key, int value)
	{
		this.id = id;
		this.key = key;
		this.value = value;
	}	

	public ConfGameIntParamItem Clone()
	{
		ConfGameIntParamItem item = (ConfGameIntParamItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfGameIntParamBase : ConfBase<ConfGameIntParamItem>
{
    public override void Init()
    {
		confName = "GameIntParam";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfGameIntParamItem(1, "maxSlot", 10));
		AddItem(new ConfGameIntParamItem(2, "defaultSolt", 2));
		AddItem(new ConfGameIntParamItem(3, "sellRate", 70));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfGameIntParamItem GetItem(int id)
	{
		return GetItemObject<ConfGameIntParamItem>(id);
	}
	
}
	
