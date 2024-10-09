using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfZombieIllustrationsItem : ConfBaseItem
{
	/// <summary>
	/// 僵尸类型类型
	/// </summary>
	public int zombieType;

	/// <summary>
	/// 僵尸UI预制体路径
	/// </summary>
	public string prefabPath;

	/// <summary>
	/// 僵尸图片路径
	/// </summary>
	public string plantImagePath;

	/// <summary>
	/// 描述
	/// </summary>
	public string info;


	public ConfZombieIllustrationsItem()
	{
	}

	public ConfZombieIllustrationsItem(int id, int zombieType, string prefabPath, string plantImagePath, string info)
	{
		this.id = id;
		this.zombieType = zombieType;
		this.prefabPath = prefabPath;
		this.plantImagePath = plantImagePath;
		this.info = info;
	}	

	public ConfZombieIllustrationsItem Clone()
	{
		ConfZombieIllustrationsItem item = (ConfZombieIllustrationsItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfZombieIllustrationsBase : ConfBase<ConfZombieIllustrationsItem>
{
    public override void Init()
    {
		confName = "ZombieIllustrations";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfZombieIllustrationsItem(1, 0, "Prefabs/Zombie/Illustrations/ZombieNormal", "", ""));
		AddItem(new ConfZombieIllustrationsItem(2, 1, "Prefabs/Zombie/Illustrations/ZombieCone", "", ""));
		AddItem(new ConfZombieIllustrationsItem(3, 2, "Prefabs/Zombie/Illustrations/ZombieBucket", "", ""));
		AddItem(new ConfZombieIllustrationsItem(4, 3, "Prefabs/Zombie/Illustrations/ZombieScreendoor", "", ""));
		AddItem(new ConfZombieIllustrationsItem(5, 4, "Prefabs/Zombie/Illustrations/ZombieFlag", "", ""));
		AddItem(new ConfZombieIllustrationsItem(6, 5, "Prefabs/Zombie/Illustrations/ZombieFootball", "", ""));
		AddItem(new ConfZombieIllustrationsItem(7, 6, "Prefabs/Zombie/Illustrations/ZombiePaper", "", ""));
		AddItem(new ConfZombieIllustrationsItem(8, 7, "Prefabs/Zombie/Illustrations/ZombiePolevaulter", "", ""));
		AddItem(new ConfZombieIllustrationsItem(9, 8, "Prefabs/Zombie/Illustrations/ZombieBalloon", "", ""));
		AddItem(new ConfZombieIllustrationsItem(10, 9, "Prefabs/Zombie/Illustrations/ZombieZamboni", "", ""));
		AddItem(new ConfZombieIllustrationsItem(11, 10, "Prefabs/Zombie/Illustrations/ZombieCatapult", "", ""));
		AddItem(new ConfZombieIllustrationsItem(12, 11, "Prefabs/Zombie/Illustrations/ZombieGargantuan", "", ""));
		AddItem(new ConfZombieIllustrationsItem(13, 12, "Prefabs/Zombie/Illustrations/ZombieBoss", "", ""));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfZombieIllustrationsItem GetItem(int id)
	{
		return GetItemObject<ConfZombieIllustrationsItem>(id);
	}
	
}
	
