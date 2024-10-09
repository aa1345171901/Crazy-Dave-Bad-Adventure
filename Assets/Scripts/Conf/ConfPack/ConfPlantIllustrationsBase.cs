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
		AddItem(new ConfPlantIllustrationsItem(1, 1, "Prefabs/Plants/UI/Peashooter", "plantInfo_Peashooter"));
		AddItem(new ConfPlantIllustrationsItem(2, 2, "Prefabs/Plants/UI/Repeater", "plantInfo_Repeater"));
		AddItem(new ConfPlantIllustrationsItem(3, 3, "Prefabs/Plants/UI/Cactus", "plantInfo_Cactus"));
		AddItem(new ConfPlantIllustrationsItem(4, 4, "Prefabs/Plants/UI/Blover", "plantInfo_Blover"));
		AddItem(new ConfPlantIllustrationsItem(5, 5, "Prefabs/Plants/UI/Cattail", "plantInfo_Cattail"));
		AddItem(new ConfPlantIllustrationsItem(6, 6, "Prefabs/Plants/UI/CherryBomb", "plantInfo_CherryBomb"));
		AddItem(new ConfPlantIllustrationsItem(7, 7, "Prefabs/Plants/UI/Chomper", "plantInfo_Chomper"));
		AddItem(new ConfPlantIllustrationsItem(8, 8, "Prefabs/Plants/UI/CoffeeBean", "plantInfo_CoffeeBean"));
		AddItem(new ConfPlantIllustrationsItem(9, 9, "Prefabs/Plants/UI/Cornpult", "plantInfo_CornpultCard"));
		AddItem(new ConfPlantIllustrationsItem(10, 10, "Prefabs/Plants/UI/FumeShroom", "plantInfo_FumeShroom"));
		AddItem(new ConfPlantIllustrationsItem(11, 11, "Prefabs/Plants/UI/GatlingPea", "plantInfo_GatlingPea"));
		AddItem(new ConfPlantIllustrationsItem(12, 12, "Prefabs/Plants/UI/GloomShroom", "plantInfo_GloomShroom"));
		AddItem(new ConfPlantIllustrationsItem(13, 13, "Prefabs/Plants/UI/GoldMagent", "plantInfo_GoldMagent"));
		AddItem(new ConfPlantIllustrationsItem(14, 14, "Prefabs/Plants/UI/LilyPad", "plantInfo_LilyPad"));
		AddItem(new ConfPlantIllustrationsItem(15, 15, "Prefabs/Plants/UI/Gralic", "plantInfo_Gralic"));
		AddItem(new ConfPlantIllustrationsItem(16, 16, "Prefabs/Plants/UI/Gravebuster", "plantInfo_Gravebuster"));
		AddItem(new ConfPlantIllustrationsItem(17, 17, "Prefabs/Plants/UI/HypnoShroom", "plantInfo_HypnoShroom"));
		AddItem(new ConfPlantIllustrationsItem(18, 18, "Prefabs/Plants/UI/MagentShroom", "plantInfo_MagentShroom"));
		AddItem(new ConfPlantIllustrationsItem(19, 19, "Prefabs/Plants/UI/Marigold", "plantInfo_Marigold"));
		AddItem(new ConfPlantIllustrationsItem(20, 20, "Prefabs/Plants/UI/Plantern", "plantInfo_Plantern"));
		AddItem(new ConfPlantIllustrationsItem(21, 21, "Prefabs/Plants/UI/PuffShroom", "plantInfo_PuffShroom"));
		AddItem(new ConfPlantIllustrationsItem(22, 22, "Prefabs/Plants/UI/PumpkinHead", "plantInfo_PumpkinHead"));
		AddItem(new ConfPlantIllustrationsItem(23, 23, "Prefabs/Plants/UI/ScaredyShroom", "plantInfo_ScaredyShroom"));
		AddItem(new ConfPlantIllustrationsItem(24, 24, "Prefabs/Plants/UI/SnowPea", "plantInfo_SnowPea"));
		AddItem(new ConfPlantIllustrationsItem(25, 25, "Prefabs/Plants/UI/Spikerock", "plantInfo_Spikerock"));
		AddItem(new ConfPlantIllustrationsItem(26, 26, "Prefabs/Plants/UI/Spikeweed", "plantInfo_Spikeweed"));
		AddItem(new ConfPlantIllustrationsItem(27, 27, "Prefabs/Plants/UI/SplitPea", "plantInfo_SplitPea"));
		AddItem(new ConfPlantIllustrationsItem(28, 28, "Prefabs/Plants/UI/Starfruit", "plantInfo_Starfruit"));
		AddItem(new ConfPlantIllustrationsItem(29, 29, "Prefabs/Plants/UI/SunFlower", "plantInfo_SunFlower"));
		AddItem(new ConfPlantIllustrationsItem(30, 30, "Prefabs/Plants/UI/TallNut", "plantInfo_TallNut"));
		AddItem(new ConfPlantIllustrationsItem(31, 31, "Prefabs/Plants/UI/Threepeater", "plantInfo_Threepeater"));
		AddItem(new ConfPlantIllustrationsItem(32, 32, "Prefabs/Plants/UI/Torchwood", "plantInfo_Torchwood"));
		AddItem(new ConfPlantIllustrationsItem(33, 33, "Prefabs/Plants/UI/TwinSunflower", "plantInfo_TwinSunflower"));
		AddItem(new ConfPlantIllustrationsItem(34, 34, "Prefabs/Plants/UI/WallNut", "plantInfo_WallNut"));
		AddItem(new ConfPlantIllustrationsItem(35, 35, "Prefabs/Plants/UI/IceShroom", "plantInfo_IceShroom"));
		AddItem(new ConfPlantIllustrationsItem(36, 36, "Prefabs/Plants/UI/Jalapeno", "plantInfo_Jalapeno"));
		AddItem(new ConfPlantIllustrationsItem(37, 37, "Prefabs/Plants/UI/DoomShroom", "plantInfo_DoomShroom"));
		AddItem(new ConfPlantIllustrationsItem(38, 38, "Prefabs/Plants/UI/Squash", "plantInfo_Squash"));
		AddItem(new ConfPlantIllustrationsItem(39, 39, "Prefabs/Plants/UI/PotatoMine", "plantInfo_PotatoMine"));
		AddItem(new ConfPlantIllustrationsItem(40, 40, "Prefabs/Plants/UI/CobCannon", "plantInfo_CobCannon"));
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
	
