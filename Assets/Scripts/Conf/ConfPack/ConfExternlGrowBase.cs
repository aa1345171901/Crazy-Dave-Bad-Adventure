using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfExternlGrowItem : ConfBaseItem
{
	/// <summary>
	/// 键值
	/// </summary>
	public string key;

	/// <summary>
	/// 养成名称
	/// </summary>
	public string name;

	/// <summary>
	/// 升级消耗,数组长度代表等级
	/// </summary>
	public int[] cost;

	/// <summary>
	/// 升级成长数值
	/// </summary>
	public int[] levelAdd;

	/// <summary>
	/// 描述
	/// </summary>
	public string desc;

	/// <summary>
	/// 图片位置,显示的icon
	/// </summary>
	public string imgPath;

	/// <summary>
	/// 成长类型
	/// </summary>
	public int growType;


	public ConfExternlGrowItem()
	{
	}

	public ConfExternlGrowItem(int id, string key, string name, int[] cost, int[] levelAdd, string desc, string imgPath, int growType)
	{
		this.id = id;
		this.key = key;
		this.name = name;
		this.cost = cost;
		this.levelAdd = levelAdd;
		this.desc = desc;
		this.imgPath = imgPath;
		this.growType = growType;
	}	

	public ConfExternlGrowItem Clone()
	{
		ConfExternlGrowItem item = (ConfExternlGrowItem)this.MemberwiseClone();
		item.cost = new int[this.cost.Length];
		for (int i = 0; i < this.cost.Length; i++)
        {
			item.cost[i] = this.cost[i];
		}
		item.levelAdd = new int[this.levelAdd.Length];
		for (int i = 0; i < this.levelAdd.Length; i++)
        {
			item.levelAdd[i] = this.levelAdd[i];
		}

		return item;
	}
}
public class ConfExternlGrowBase : ConfBase<ConfExternlGrowItem>
{
    public override void Init()
    {
		confName = "ExternlGrow";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfExternlGrowItem(1, "maximumhp", "property_MaximumHP", new int[]{ 30, 60, 180, 240, 600 }, new int[]{ 5, 5, 10, 10, 20 }, "mainmenu_grow1", "UI/GrowItem/Grow-1", 1));
		AddItem(new ConfExternlGrowItem(2, "liferecovery", "property_LifeRecovery", new int[]{ 25, 150, 500 }, new int[]{ 5, 5, 10 }, "mainmenu_grow2", "UI/GrowItem/Grow-2", 1));
		AddItem(new ConfExternlGrowItem(3, "adrenaline", "property_Adrenaline", new int[]{ 30, 180, 600 }, new int[]{ 10, 20, 40 }, "mainmenu_grow3", "UI/GrowItem/Grow-3", 1));
		AddItem(new ConfExternlGrowItem(4, "power", "property_Power", new int[]{ 50, 100, 150, 400, 800 }, new int[]{ 1, 1, 1, 2, 2 }, "mainmenu_grow4", "UI/GrowItem/Grow-4", 1));
		AddItem(new ConfExternlGrowItem(5, "percentagedamage", "property_PercentageDamage", new int[]{ 30, 60, 180, 360, 720 }, new int[]{ 2, 2, 4, 4, 5 }, "mainmenu_grow5", "UI/GrowItem/Grow-5", 1));
		AddItem(new ConfExternlGrowItem(6, "attackspeed", "property_AttackSpeed", new int[]{ 100, 250, 500 }, new int[]{ 10, 10, 10 }, "mainmenu_grow6", "UI/GrowItem/Grow-6", 1));
		AddItem(new ConfExternlGrowItem(7, "range", "property_Range", new int[]{ 30, 60, 180, 400 }, new int[]{ 5, 5, 10, 10 }, "mainmenu_grow7", "UI/GrowItem/Grow-7", 1));
		AddItem(new ConfExternlGrowItem(8, "criticalhitrate", "property_CriticalHitRate", new int[]{ 150, 999 }, new int[]{ 5, 10 }, "mainmenu_grow8", "UI/GrowItem/Grow-8", 1));
		AddItem(new ConfExternlGrowItem(9, "movespeed", "property_MoveSpeed", new int[]{ 100, 250, 500 }, new int[]{ 10, 10, 10 }, "mainmenu_grow9", "UI/GrowItem/Grow-9", 1));
		AddItem(new ConfExternlGrowItem(10, "armor", "property_Armor", new int[]{ 40, 80, 150, 400, 800 }, new int[]{ 2, 2, 2, 4, 5 }, "mainmenu_grow10", "UI/GrowItem/Grow-10", 1));
		AddItem(new ConfExternlGrowItem(11, "lucky", "property_Lucky", new int[]{ 80, 250, 444 }, new int[]{ 2, 4, 4 }, "mainmenu_grow11", "UI/GrowItem/Grow-11", 1));
		AddItem(new ConfExternlGrowItem(12, "sunshine", "property_Sunshine", new int[]{ 10, 40, 90, 160, 250 }, new int[]{ 25, 50, 75, 100, 125 }, "mainmenu_grow12", "UI/GrowItem/Grow-12", 1));
		AddItem(new ConfExternlGrowItem(13, "goldcoins", "property_GoldCoins", new int[]{ 20, 40, 120, 160, 300 }, new int[]{ 5, 5, 10, 10, 20 }, "mainmenu_grow13", "UI/GrowItem/Grow-13", 1));
		AddItem(new ConfExternlGrowItem(14, "botany", "property_Botany", new int[]{ 30, 60, 180, 360, 720 }, new int[]{ 2, 2, 4, 4, 5 }, "mainmenu_grow14", "UI/GrowItem/Grow-14", 1));
		AddItem(new ConfExternlGrowItem(15, "slotNum", "grow_slotNum", new int[]{ 100, 222, 444 }, new int[]{ 1, 1, 1 }, "mainmenu_grow15", "UI/GrowItem/Grow-15", 2));
		AddItem(new ConfExternlGrowItem(16, "addProp", "grow_addProp", new int[]{ 222, 777, 1666 }, new int[]{ 0, 0, 0 }, "mainmenu_grow16", "UI/GrowItem/Grow-16", 3));
		AddItem(new ConfExternlGrowItem(17, "addPlant", "grow_addPlant", new int[]{ 100, 500, 1000, 3000 }, new int[]{ 0, 0, 0, 0 }, "mainmenu_grow17", "UI/GrowItem/Grow-17", 4));
		AddItem(new ConfExternlGrowItem(18, "addSun", "grow_addSun", new int[]{ 20, 100, 200, 400, 666 }, new int[]{ 50, 50, 100, 100, 150 }, "mainmenu_grow18", "UI/GrowItem/Grow-18", 5));
		AddItem(new ConfExternlGrowItem(19, "addMoney", "grow_addMoney", new int[]{ 50, 125, 300, 666, 1444 }, new int[]{ 10, 10, 20, 20, 30 }, "mainmenu_grow19", "UI/GrowItem/Grow-19", 6));
		AddItem(new ConfExternlGrowItem(20, "resurrection", "grow_resurrection", new int[]{ 555, 2222 }, new int[]{ 1, 1 }, "mainmenu_grow20", "UI/GrowItem/Grow-20", 7));
		AddItem(new ConfExternlGrowItem(21, "curse", "grow_curse", new int[]{ 100, 222, 444, 888, 1444 }, new int[]{ 10, 10, 10, 10, 10 }, "mainmenu_grow21", "UI/GrowItem/Grow-21", 8));
		AddItem(new ConfExternlGrowItem(22, "physicalRecovery", "grow_physicalRecovery", new int[]{ 111, 222, 333, 444, 555 }, new int[]{ 20, 30, 40, 50, 60 }, "mainmenu_grow22", "UI/GrowItem/Grow-22", 9));
		AddItem(new ConfExternlGrowItem(23, "runTime", "grow_runTime", new int[]{ 200, 400, 666, 1111 }, new int[]{ 1, 1, 1, 1 }, "mainmenu_grow23", "UI/GrowItem/Grow-23", 10));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfExternlGrowItem GetItem(int id)
	{
		return GetItemObject<ConfExternlGrowItem>(id);
	}
	
}
	
