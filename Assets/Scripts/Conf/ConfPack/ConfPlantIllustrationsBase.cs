using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfPlantIllustrationsItem : ConfBaseItem
{
	/// <summary>
	/// 植物类型
	/// </summary>
	public int plantType;

	/// <summary>
	/// 植物预制体路径
	/// </summary>
	public string prefabPath;

	/// <summary>
	/// 描述
	/// </summary>
	public string info;


	public ConfPlantIllustrationsItem()
	{
	}

	public ConfPlantIllustrationsItem(int id, int plantType, string prefabPath, string info)
	{
		this.id = id;
		this.plantType = plantType;
		this.prefabPath = prefabPath;
		this.info = info;
	}	

	public ConfPlantIllustrationsItem Clone()
	{
		ConfPlantIllustrationsItem item = (ConfPlantIllustrationsItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfPlantIllustrationsBase : ConfBase<ConfPlantIllustrationsItem>
{
    public override void Init()
    {
		confName = "PlantIllustrations";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfPlantIllustrationsItem(1, 1, "Prefabs/Plants/UI/Peashooter", "plantIllustrationsInfo_Peashooter"));
		AddItem(new ConfPlantIllustrationsItem(2, 2, "Prefabs/Plants/UI/Repeater", "plantIllustrationsInfo_Repeater"));
		AddItem(new ConfPlantIllustrationsItem(3, 3, "Prefabs/Plants/UI/Cactus", "plantIllustrationsInfo_Cactus"));
		AddItem(new ConfPlantIllustrationsItem(4, 4, "Prefabs/Plants/UI/Blover", "plantIllustrationsInfo_Blover"));
		AddItem(new ConfPlantIllustrationsItem(5, 5, "Prefabs/Plants/UI/Cattail", "plantIllustrationsInfo_Cattail"));
		AddItem(new ConfPlantIllustrationsItem(6, 6, "Prefabs/Plants/UI/CherryBomb", "plantIllustrationsInfo_CherryBomb"));
		AddItem(new ConfPlantIllustrationsItem(7, 7, "Prefabs/Plants/UI/Chomper", "plantIllustrationsInfo_Chomper"));
		AddItem(new ConfPlantIllustrationsItem(8, 8, "Prefabs/Plants/UI/CoffeeBean", "plantIllustrationsInfo_CoffeeBean"));
		AddItem(new ConfPlantIllustrationsItem(9, 9, "Prefabs/Plants/UI/Cornpult", "plantIllustrationsInfo_Cornpult"));
		AddItem(new ConfPlantIllustrationsItem(10, 10, "Prefabs/Plants/UI/FumeShroom", "plantIllustrationsInfo_FumeShroom"));
		AddItem(new ConfPlantIllustrationsItem(11, 11, "Prefabs/Plants/UI/GatlingPea", "plantIllustrationsInfo_GatlingPea"));
		AddItem(new ConfPlantIllustrationsItem(12, 12, "Prefabs/Plants/UI/GloomShroom", "plantIllustrationsInfo_GloomShroom"));
		AddItem(new ConfPlantIllustrationsItem(13, 13, "Prefabs/Plants/UI/GoldMagent", "plantIllustrationsInfo_GoldMagent"));
		AddItem(new ConfPlantIllustrationsItem(14, 14, "Prefabs/Plants/UI/LilyPad", "plantIllustrationsInfo_LilyPad"));
		AddItem(new ConfPlantIllustrationsItem(15, 15, "Prefabs/Plants/UI/Gralic", "plantIllustrationsInfo_Gralic"));
		AddItem(new ConfPlantIllustrationsItem(16, 16, "Prefabs/Plants/UI/Gravebuster", "plantIllustrationsInfo_Gravebuster"));
		AddItem(new ConfPlantIllustrationsItem(17, 17, "Prefabs/Plants/UI/HypnoShroom", "plantIllustrationsInfo_HypnoShroom"));
		AddItem(new ConfPlantIllustrationsItem(18, 18, "Prefabs/Plants/UI/MagentShroom", "plantIllustrationsInfo_MagentShroom"));
		AddItem(new ConfPlantIllustrationsItem(19, 19, "Prefabs/Plants/UI/Marigold", "plantIllustrationsInfo_Marigold"));
		AddItem(new ConfPlantIllustrationsItem(20, 20, "Prefabs/Plants/UI/Plantern", "plantIllustrationsInfo_Plantern"));
		AddItem(new ConfPlantIllustrationsItem(21, 21, "Prefabs/Plants/UI/PuffShroom", "plantIllustrationsInfo_PuffShroom"));
		AddItem(new ConfPlantIllustrationsItem(22, 22, "Prefabs/Plants/UI/PumpkinHead", "plantIllustrationsInfo_PumpkinHead"));
		AddItem(new ConfPlantIllustrationsItem(23, 23, "Prefabs/Plants/UI/ScaredyShroom", "plantIllustrationsInfo_ScaredyShroom"));
		AddItem(new ConfPlantIllustrationsItem(24, 24, "Prefabs/Plants/UI/SnowPea", "plantIllustrationsInfo_SnowPea"));
		AddItem(new ConfPlantIllustrationsItem(25, 25, "Prefabs/Plants/UI/Spikerock", "plantIllustrationsInfo_Spikerock"));
		AddItem(new ConfPlantIllustrationsItem(26, 26, "Prefabs/Plants/UI/Spikeweed", "plantIllustrationsInfo_Spikeweed"));
		AddItem(new ConfPlantIllustrationsItem(27, 27, "Prefabs/Plants/UI/SplitPea", "plantIllustrationsInfo_SplitPea"));
		AddItem(new ConfPlantIllustrationsItem(28, 28, "Prefabs/Plants/UI/Starfruit", "plantIllustrationsInfo_Starfruit"));
		AddItem(new ConfPlantIllustrationsItem(29, 29, "Prefabs/Plants/UI/SunFlower", "plantIllustrationsInfo_SunFlower"));
		AddItem(new ConfPlantIllustrationsItem(30, 30, "Prefabs/Plants/UI/TallNut", "plantIllustrationsInfo_TallNut"));
		AddItem(new ConfPlantIllustrationsItem(31, 31, "Prefabs/Plants/UI/Threepeater", "plantIllustrationsInfo_Threepeater"));
		AddItem(new ConfPlantIllustrationsItem(32, 32, "Prefabs/Plants/UI/Torchwood", "plantIllustrationsInfo_Torchwood"));
		AddItem(new ConfPlantIllustrationsItem(33, 33, "Prefabs/Plants/UI/TwinSunflower", "plantIllustrationsInfo_TwinSunflower"));
		AddItem(new ConfPlantIllustrationsItem(34, 34, "Prefabs/Plants/UI/WallNut", "plantIllustrationsInfo_WallNut"));
		AddItem(new ConfPlantIllustrationsItem(35, 35, "Prefabs/Plants/UI/IceShroom", "plantIllustrationsInfo_IceShroom"));
		AddItem(new ConfPlantIllustrationsItem(36, 36, "Prefabs/Plants/UI/Jalapeno", "plantIllustrationsInfo_Jalapeno"));
		AddItem(new ConfPlantIllustrationsItem(37, 37, "Prefabs/Plants/UI/DoomShroom", "plantIllustrationsInfo_DoomShroom"));
		AddItem(new ConfPlantIllustrationsItem(38, 38, "Prefabs/Plants/UI/Squash", "plantIllustrationsInfo_Squash"));
		AddItem(new ConfPlantIllustrationsItem(39, 39, "Prefabs/Plants/UI/PotatoMine", "plantIllustrationsInfo_PotatoMine"));
		AddItem(new ConfPlantIllustrationsItem(40, 40, "Prefabs/Plants/UI/CobCannon", "plantIllustrationsInfo_CobCannon"));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfPlantIllustrationsItem GetItem(int id)
	{
		return GetItemObject<ConfPlantIllustrationsItem>(id);
	}
	
}
	
