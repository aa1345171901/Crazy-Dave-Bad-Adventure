using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfOtherGameModesItem : ConfBaseItem
{
	/// <summary>
	/// 游戏模式枚举       1道具，2植物，3生存
	/// </summary>
	public int type;

	/// <summary>
	/// 背景图片路径
	/// </summary>
	public string imgPath;

	/// <summary>
	/// 名称
	/// </summary>
	public string name;


	public ConfOtherGameModesItem()
	{
	}

	public ConfOtherGameModesItem(int id, int type, string imgPath, string name)
	{
		this.id = id;
		this.type = type;
		this.imgPath = imgPath;
		this.name = name;
	}	

	public ConfOtherGameModesItem Clone()
	{
		ConfOtherGameModesItem item = (ConfOtherGameModesItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfOtherGameModesBase : ConfBase<ConfOtherGameModesItem>
{
    public override void Init()
    {
		confName = "OtherGameModes";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfOtherGameModesItem(1001, 1, "UI/OtherGameModes/1001", "otherGameModeName_1001"));
		AddItem(new ConfOtherGameModesItem(1002, 1, "UI/OtherGameModes/1002", "otherGameModeName_1002"));
		AddItem(new ConfOtherGameModesItem(1003, 1, "UI/OtherGameModes/1003", "otherGameModeName_1003"));
		AddItem(new ConfOtherGameModesItem(1004, 1, "UI/OtherGameModes/1004", "otherGameModeName_1004"));
		AddItem(new ConfOtherGameModesItem(1005, 1, "UI/OtherGameModes/1005", "otherGameModeName_1005"));
		AddItem(new ConfOtherGameModesItem(1006, 1, "UI/OtherGameModes/1006", "otherGameModeName_1006"));
		AddItem(new ConfOtherGameModesItem(1007, 1, "UI/OtherGameModes/1007", "otherGameModeName_1007"));
		AddItem(new ConfOtherGameModesItem(1008, 1, "UI/OtherGameModes/1008", "otherGameModeName_1008"));
		AddItem(new ConfOtherGameModesItem(1009, 1, "UI/OtherGameModes/1009", "otherGameModeName_1009"));
		AddItem(new ConfOtherGameModesItem(1010, 1, "UI/OtherGameModes/1010", "otherGameModeName_1010"));
		AddItem(new ConfOtherGameModesItem(1011, 1, "UI/OtherGameModes/1011", "otherGameModeName_1011"));
		AddItem(new ConfOtherGameModesItem(2001, 2, "UI/OtherGameModes/2001", "otherGameModeName_2001"));
		AddItem(new ConfOtherGameModesItem(2002, 2, "UI/OtherGameModes/2002", "otherGameModeName_2002"));
		AddItem(new ConfOtherGameModesItem(2003, 2, "UI/OtherGameModes/2003", "otherGameModeName_2003"));
		AddItem(new ConfOtherGameModesItem(2004, 2, "UI/OtherGameModes/2004", "otherGameModeName_2004"));
		AddItem(new ConfOtherGameModesItem(2005, 2, "UI/OtherGameModes/2005", "otherGameModeName_2005"));
		AddItem(new ConfOtherGameModesItem(2006, 2, "UI/OtherGameModes/2006", "otherGameModeName_2006"));
		AddItem(new ConfOtherGameModesItem(2007, 2, "UI/OtherGameModes/2007", "otherGameModeName_2007"));
		AddItem(new ConfOtherGameModesItem(2008, 2, "UI/OtherGameModes/2008", "otherGameModeName_2008"));
		AddItem(new ConfOtherGameModesItem(2009, 2, "UI/OtherGameModes/2009", "otherGameModeName_2009"));
		AddItem(new ConfOtherGameModesItem(2010, 2, "UI/OtherGameModes/2010", "otherGameModeName_2010"));
		AddItem(new ConfOtherGameModesItem(2011, 2, "UI/OtherGameModes/2011", "otherGameModeName_2011"));
		AddItem(new ConfOtherGameModesItem(2022, 2, "UI/OtherGameModes/2022", "otherGameModeName_2022"));
		AddItem(new ConfOtherGameModesItem(3001, 3, "UI/OtherGameModes/3001", "otherGameModeName_3001"));
		AddItem(new ConfOtherGameModesItem(3002, 3, "UI/OtherGameModes/3002", "otherGameModeName_3002"));
		AddItem(new ConfOtherGameModesItem(3003, 3, "UI/OtherGameModes/3003", "otherGameModeName_3003"));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfOtherGameModesItem GetItem(int id)
	{
		return GetItemObject<ConfOtherGameModesItem>(id);
	}
	
}
	
