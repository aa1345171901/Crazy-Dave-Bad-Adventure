using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfPlantCardsItem : ConfBaseItem
{
	/// <summary>
	/// 植物名
	/// </summary>
	public string plantName;

	/// <summary>
	/// 卡片背景路径
	/// </summary>
	public string plantBgImagePath;

	/// <summary>
	/// 植物图片路径
	/// </summary>
	public string plantImagePath;

	/// <summary>
	/// 默认价格
	/// </summary>
	public int defaultPrice;

	/// <summary>
	/// 默认消耗阳光
	/// </summary>
	public int defaultSun;

	/// <summary>
	/// 描述
	/// </summary>
	public string info;

	/// <summary>
	/// 植物类型
	/// </summary>
	public int plantType;

	/// <summary>
	/// 品质
	/// </summary>
	public int quality;


	public ConfPlantCardsItem()
	{
	}

	public ConfPlantCardsItem(int id, string plantName, string plantBgImagePath, string plantImagePath, int defaultPrice, int defaultSun, string info, int plantType, int quality)
	{
		this.id = id;
		this.plantName = plantName;
		this.plantBgImagePath = plantBgImagePath;
		this.plantImagePath = plantImagePath;
		this.defaultPrice = defaultPrice;
		this.defaultSun = defaultSun;
		this.info = info;
		this.plantType = plantType;
		this.quality = quality;
	}	

	public ConfPlantCardsItem Clone()
	{
		ConfPlantCardsItem item = (ConfPlantCardsItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfPlantCardsBase : ConfBase<ConfPlantCardsItem>
{
    public override void Init()
    {
		confName = "PlantCards";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfPlantCardsItem(1, "plantName_Peashooter", "Shop/Plants/normalBg", "Shop/Plants/Peashooter_1", 15, 100, "plantInfo_Peashooter", 1, 1));
		AddItem(new ConfPlantCardsItem(2, "plantName_Repeater", "Shop/Plants/strongBg", "Shop/Plants/Repeater", 25, 100, "plantInfo_Repeater", 2, 1));
		AddItem(new ConfPlantCardsItem(3, "plantName_Cactus", "Shop/Plants/normalBg", "Shop/Plants/Cactus", 50, 125, "plantInfo_Cactus", 3, 2));
		AddItem(new ConfPlantCardsItem(4, "plantName_Blover", "Shop/Plants/normalBg", "Shop/Plants/Blover", 45, 100, "plantInfo_Blover", 4, 2));
		AddItem(new ConfPlantCardsItem(5, "plantName_Cattail", "Shop/Plants/strongBg", "Shop/Plants/Cattail", 90, 225, "plantInfo_Cattail", 5, 4));
		AddItem(new ConfPlantCardsItem(6, "plantName_CherryBomb", "Shop/Plants/normalBg", "Shop/Plants/CherryBomb", 70, 150, "plantInfo_CherryBomb", 6, 3));
		AddItem(new ConfPlantCardsItem(7, "plantName_Chomper", "Shop/Plants/normalBg", "Shop/Plants/Chomper", 20, 150, "plantInfo_Chomper", 7, 1));
		AddItem(new ConfPlantCardsItem(8, "plantName_CoffeeBean", "Shop/Plants/normalBg", "Shop/Plants/CoffeeBean", 40, 75, "plantInfo_CoffeeBean", 8, 2));
		AddItem(new ConfPlantCardsItem(9, "plantName_CornpultCard", "Shop/Plants/normalBg", "Shop/Plants/CornpultCard", 30, 100, "plantInfo_CornpultCard", 9, 1));
		AddItem(new ConfPlantCardsItem(10, "plantName_FumeShroom", "Shop/Plants/normalBg", "Shop/Plants/FumeShroom", 40, 75, "plantInfo_FumeShroom", 10, 2));
		AddItem(new ConfPlantCardsItem(11, "plantName_GatlingPea", "Shop/Plants/strongBg", "Shop/Plants/GatlingPea", 35, 250, "plantInfo_GatlingPea", 11, 3));
		AddItem(new ConfPlantCardsItem(12, "plantName_GloomShroom", "Shop/Plants/strongBg", "Shop/Plants/GloomShroom", 80, 150, "plantInfo_GloomShroom", 12, 3));
		AddItem(new ConfPlantCardsItem(13, "plantName_GoldMagent", "Shop/Plants/normalBg", "Shop/Plants/GoldMagent", 50, 150, "plantInfo_GoldMagent", 13, 3));
		AddItem(new ConfPlantCardsItem(14, "plantName_LilyPad", "Shop/Plants/normalBg", "Shop/Plants/LilyPad", 15, 25, "plantInfo_LilyPad", 14, 1));
		AddItem(new ConfPlantCardsItem(15, "plantName_Gralic", "Shop/Plants/normalBg", "Shop/Plants/Gralic", 35, 50, "plantInfo_Gralic", 15, 2));
		AddItem(new ConfPlantCardsItem(16, "plantName_Gravebuster", "Shop/Plants/normalBg", "Shop/Plants/Gravebuster", 70, 75, "plantInfo_Gravebuster", 16, 4));
		AddItem(new ConfPlantCardsItem(17, "plantName_HypnoShroom", "Shop/Plants/normalBg", "Shop/Plants/HypnoShroom", 90, 75, "plantInfo_HypnoShroom", 17, 3));
		AddItem(new ConfPlantCardsItem(18, "plantName_MagentShroom", "Shop/Plants/normalBg", "Shop/Plants/MagentShroom", 35, 100, "plantInfo_MagentShroom", 18, 2));
		AddItem(new ConfPlantCardsItem(19, "plantName_Marigold", "Shop/Plants/normalBg", "Shop/Plants/Marigold", 50, 50, "plantInfo_Marigold", 19, 2));
		AddItem(new ConfPlantCardsItem(20, "plantName_Plantern", "Shop/Plants/normalBg", "Shop/Plants/Plantern", 70, 25, "plantInfo_Plantern", 20, 3));
		AddItem(new ConfPlantCardsItem(21, "plantName_PuffShroom", "Shop/Plants/normalBg", "Shop/Plants/PuffShroom", 15, 0, "plantInfo_PuffShroom", 21, 1));
		AddItem(new ConfPlantCardsItem(22, "plantName_PumpkinHead", "Shop/Plants/normalBg", "Shop/Plants/PumpkinHead", 50, 125, "plantInfo_PumpkinHead", 22, 2));
		AddItem(new ConfPlantCardsItem(23, "plantName_ScaredyShroom", "Shop/Plants/normalBg", "Shop/Plants/ScaredyShroom", 20, 25, "plantInfo_ScaredyShroom", 23, 2));
		AddItem(new ConfPlantCardsItem(24, "plantName_SnowPea", "Shop/Plants/normalBg", "Shop/Plants/SnowPea", 40, 175, "plantInfo_SnowPea", 24, 2));
		AddItem(new ConfPlantCardsItem(25, "plantName_Spikerock", "Shop/Plants/strongBg", "Shop/Plants/Spikerock", 50, 125, "plantInfo_Spikerock", 25, 3));
		AddItem(new ConfPlantCardsItem(26, "plantName_Spikeweed", "Shop/Plants/normalBg", "Shop/Plants/Spikeweed", 20, 100, "plantInfo_Spikeweed", 26, 1));
		AddItem(new ConfPlantCardsItem(27, "plantName_SplitPea", "Shop/Plants/normalBg", "Shop/Plants/SplitPea", 50, 125, "plantInfo_SplitPea", 27, 3));
		AddItem(new ConfPlantCardsItem(28, "plantName_Starfruit", "Shop/Plants/normalBg", "Shop/Plants/Starfruit", 90, 125, "plantInfo_Starfruit", 28, 4));
		AddItem(new ConfPlantCardsItem(29, "plantName_SunFlower", "Shop/Plants/normalBg", "Shop/Plants/SunFlower", 20, 50, "plantInfo_SunFlower", 29, 1));
		AddItem(new ConfPlantCardsItem(30, "plantName_TallNut", "Shop/Plants/normalBg", "Shop/Plants/TallNut", 40, 125, "plantInfo_TallNut", 30, 2));
		AddItem(new ConfPlantCardsItem(31, "plantName_Threepeater", "Shop/Plants/normalBg", "Shop/Plants/Threepeater", 50, 325, "plantInfo_Threepeater", 31, 3));
		AddItem(new ConfPlantCardsItem(32, "plantName_Torchwood", "Shop/Plants/normalBg", "Shop/Plants/Torchwood", 90, 175, "plantInfo_Torchwood", 32, 3));
		AddItem(new ConfPlantCardsItem(33, "plantName_TwinSunflower", "Shop/Plants/strongBg", "Shop/Plants/TwinSunflower", 50, 150, "plantInfo_TwinSunflower", 33, 2));
		AddItem(new ConfPlantCardsItem(34, "plantName_WallNut", "Shop/Plants/normalBg", "Shop/Plants/WallNut", 30, 50, "plantInfo_WallNut", 34, 1));
		AddItem(new ConfPlantCardsItem(35, "plantName_IceShroom", "Shop/Plants/normalBg", "Shop/Plants/IceShroom", 50, 75, "plantInfo_IceShroom", 35, 2));
		AddItem(new ConfPlantCardsItem(36, "plantName_Jalapeno", "Shop/Plants/normalBg", "Shop/Plants/Jalapeno", 100, 125, "plantInfo_Jalapeno", 36, 4));
		AddItem(new ConfPlantCardsItem(37, "plantName_DoomShroom", "Shop/Plants/normalBg", "Shop/Plants/DoomShroom", 120, 125, "plantInfo_DoomShroom", 37, 4));
		AddItem(new ConfPlantCardsItem(38, "plantName_Squash", "Shop/Plants/normalBg", "Shop/Plants/Squash", 30, 50, "plantInfo_Squash", 38, 1));
		AddItem(new ConfPlantCardsItem(39, "plantName_PotatoMine", "Shop/Plants/normalBg", "Shop/Plants/PotatoMine", 30, 25, "plantInfo_PotatoMine", 39, 1));
		AddItem(new ConfPlantCardsItem(40, "plantName_CobCannon", "Shop/Plants/strongBg", "Shop/Plants/CobCannon", 150, 500, "plantInfo_CobCannon", 40, 4));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfPlantCardsItem GetItem(int id)
	{
		return GetItemObject<ConfPlantCardsItem>(id);
	}
	
}
	
