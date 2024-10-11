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
	public string zombieImagePath;

	/// <summary>
	/// 僵尸名称
	/// </summary>
	public string zombieName;

	/// <summary>
	/// 描述
	/// </summary>
	public string info;


	public ConfZombieIllustrationsItem()
	{
	}

	public ConfZombieIllustrationsItem(int id, int zombieType, string prefabPath, string zombieImagePath, string zombieName, string info)
	{
		this.id = id;
		this.zombieType = zombieType;
		this.prefabPath = prefabPath;
		this.zombieImagePath = zombieImagePath;
		this.zombieName = zombieName;
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
		AddItem(new ConfZombieIllustrationsItem(1, 0, "Prefabs/Zombie/Illustrations/ZombieNormal", "UI/Illustrations/ZombieNormal", "zombieName_ZombieNormal", "zombieInfo_ZombieNormal"));
		AddItem(new ConfZombieIllustrationsItem(2, 1, "Prefabs/Zombie/Illustrations/ZombieCone", "UI/Illustrations/ZombieCone", "zombieName_ZombieCone", "zombieInfo_ZombieCone"));
		AddItem(new ConfZombieIllustrationsItem(3, 2, "Prefabs/Zombie/Illustrations/ZombieBucket", "UI/Illustrations/ZombieBucket", "zombieName_ZombieBucket", "zombieInfo_ZombieBucket"));
		AddItem(new ConfZombieIllustrationsItem(4, 3, "Prefabs/Zombie/Illustrations/ZombieScreendoor", "UI/Illustrations/ZombieScreendoor", "zombieName_ZombieScreendoor", "zombieInfo_ZombieScreendoor"));
		AddItem(new ConfZombieIllustrationsItem(5, 4, "Prefabs/Zombie/Illustrations/ZombieFlag", "UI/Illustrations/ZombieFlag", "zombieName_ZombieFlag", "zombieInfo_ZombieFlag"));
		AddItem(new ConfZombieIllustrationsItem(6, 5, "Prefabs/Zombie/Illustrations/ZombieFootball", "UI/Illustrations/ZombieFootball", "zombieName_ZombieFootball", "zombieInfo_ZombieFootball"));
		AddItem(new ConfZombieIllustrationsItem(7, 6, "Prefabs/Zombie/Illustrations/ZombiePaper", "UI/Illustrations/ZombiePaper", "zombieName_ZombiePaper", "zombieInfo_ZombiePaper"));
		AddItem(new ConfZombieIllustrationsItem(8, 7, "Prefabs/Zombie/Illustrations/ZombiePolevaulter", "UI/Illustrations/ZombiePolevaulter", "zombieName_ZombiePolevaulter", "zombieInfo_ZombiePolevaulter"));
		AddItem(new ConfZombieIllustrationsItem(9, 8, "Prefabs/Zombie/Illustrations/ZombieBalloon", "UI/Illustrations/ZombieBalloon", "zombieName_ZombieBalloon", "zombieInfo_ZombieBalloon"));
		AddItem(new ConfZombieIllustrationsItem(10, 9, "Prefabs/Zombie/Illustrations/ZombieZamboni", "UI/Illustrations/ZombieZamboni", "zombieName_ZombieZamboni", "zombieInfo_ZombieZamboni"));
		AddItem(new ConfZombieIllustrationsItem(11, 10, "Prefabs/Zombie/Illustrations/ZombieCatapult", "UI/Illustrations/ZombieCatapult", "zombieName_ZombieCatapult", "zombieInfo_ZombieCatapult"));
		AddItem(new ConfZombieIllustrationsItem(12, 11, "Prefabs/Zombie/Illustrations/ZombieGargantuan", "UI/Illustrations/ZombieGargantuan", "zombieName_ZombieGargantuan", "zombieInfo_ZombieGargantuan"));
		AddItem(new ConfZombieIllustrationsItem(13, 12, "Prefabs/Zombie/Illustrations/ZombieBoss", "UI/Illustrations/ZombieBoss", "zombieName_ZombieBoss", "zombieInfo_ZombieBoss"));
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
	
