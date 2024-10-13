using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfSeedWeightItem : ConfBaseItem
{
	/// <summary>
	/// 掉落植物种子类型，0不掉落
	/// </summary>
	public int plantType;

	/// <summary>
	/// 掉落权重
	/// </summary>
	public int weight;

	/// <summary>
	/// 品质
	/// </summary>
	public int quality;


	public ConfSeedWeightItem()
	{
	}

	public ConfSeedWeightItem(int id, int plantType, int weight, int quality)
	{
		this.id = id;
		this.plantType = plantType;
		this.weight = weight;
		this.quality = quality;
	}	

	public ConfSeedWeightItem Clone()
	{
		ConfSeedWeightItem item = (ConfSeedWeightItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfSeedWeightBase : ConfBase<ConfSeedWeightItem>
{
    public override void Init()
    {
		confName = "SeedWeight";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfSeedWeightItem(1, 0, 390000, 0));
		AddItem(new ConfSeedWeightItem(2, 1, 500, 1));
		AddItem(new ConfSeedWeightItem(3, 2, 300, 2));
		AddItem(new ConfSeedWeightItem(4, 3, 250, 2));
		AddItem(new ConfSeedWeightItem(5, 4, 275, 2));
		AddItem(new ConfSeedWeightItem(6, 5, 100, 4));
		AddItem(new ConfSeedWeightItem(7, 6, 200, 2));
		AddItem(new ConfSeedWeightItem(8, 7, 500, 1));
		AddItem(new ConfSeedWeightItem(9, 8, 225, 2));
		AddItem(new ConfSeedWeightItem(10, 9, 400, 2));
		AddItem(new ConfSeedWeightItem(11, 10, 300, 2));
		AddItem(new ConfSeedWeightItem(12, 11, 150, 3));
		AddItem(new ConfSeedWeightItem(13, 12, 100, 3));
		AddItem(new ConfSeedWeightItem(14, 13, 175, 3));
		AddItem(new ConfSeedWeightItem(15, 14, 0, 0));
		AddItem(new ConfSeedWeightItem(16, 15, 225, 1));
		AddItem(new ConfSeedWeightItem(17, 16, 175, 3));
		AddItem(new ConfSeedWeightItem(18, 17, 125, 3));
		AddItem(new ConfSeedWeightItem(19, 18, 300, 2));
		AddItem(new ConfSeedWeightItem(20, 19, 250, 2));
		AddItem(new ConfSeedWeightItem(21, 20, 200, 3));
		AddItem(new ConfSeedWeightItem(22, 21, 500, 1));
		AddItem(new ConfSeedWeightItem(23, 22, 250, 2));
		AddItem(new ConfSeedWeightItem(24, 23, 425, 1));
		AddItem(new ConfSeedWeightItem(25, 24, 300, 2));
		AddItem(new ConfSeedWeightItem(26, 25, 250, 3));
		AddItem(new ConfSeedWeightItem(27, 26, 350, 1));
		AddItem(new ConfSeedWeightItem(28, 27, 250, 3));
		AddItem(new ConfSeedWeightItem(29, 28, 125, 3));
		AddItem(new ConfSeedWeightItem(30, 29, 450, 1));
		AddItem(new ConfSeedWeightItem(31, 30, 150, 2));
		AddItem(new ConfSeedWeightItem(32, 31, 225, 3));
		AddItem(new ConfSeedWeightItem(33, 32, 150, 3));
		AddItem(new ConfSeedWeightItem(34, 33, 250, 2));
		AddItem(new ConfSeedWeightItem(35, 34, 400, 1));
		AddItem(new ConfSeedWeightItem(36, 35, 250, 2));
		AddItem(new ConfSeedWeightItem(37, 36, 125, 4));
		AddItem(new ConfSeedWeightItem(38, 37, 100, 4));
		AddItem(new ConfSeedWeightItem(39, 38, 300, 1));
		AddItem(new ConfSeedWeightItem(40, 39, 300, 1));
		AddItem(new ConfSeedWeightItem(41, 40, 100, 4));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfSeedWeightItem GetItem(int id)
	{
		return GetItemObject<ConfSeedWeightItem>(id);
	}
	
}
	
