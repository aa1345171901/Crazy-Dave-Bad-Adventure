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


	public ConfPlantCardsItem()
	{
	}

	public ConfPlantCardsItem(int id, string plantName, string plantBgImagePath, string plantImagePath, int defaultPrice, int defaultSun, string info, int plantType)
	{
		this.id = id;
		this.plantName = plantName;
		this.plantBgImagePath = plantBgImagePath;
		this.plantImagePath = plantImagePath;
		this.defaultPrice = defaultPrice;
		this.defaultSun = defaultSun;
		this.info = info;
		this.plantType = plantType;
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
		AddItem(new ConfPlantCardsItem(1, "豌豆射手", "Shop/Plants/normalBg", "Shop/Plants/Peashooter_1", 15, 100, "<color=#00ff00>豌豆射手</color>\r\n购买后使用{0}阳光培养便能随我杀敌\r\n很划算的啦", 1));
		AddItem(new ConfPlantCardsItem(2, "双发豌豆", "Shop/Plants/strongBg", "Shop/Plants/Repeater", 25, 100, "<color=#9932CD>双发豌豆射手</color>\r\n购买后使用{0}阳光可将豌豆射手进化成双发射手\r\n一次性可以发射两枚豌豆", 2));
		AddItem(new ConfPlantCardsItem(3, "仙人掌", "Shop/Plants/normalBg", "Shop/Plants/Cactus", 50, 125, "<color=#00ff00>仙人掌</color>\r\n购买后使用{0}阳光培养\r\n可以戳破气球僵尸的气球\r\n这很重要", 3));
		AddItem(new ConfPlantCardsItem(4, "三叶草", "Shop/Plants/normalBg", "Shop/Plants/Blover", 45, 100, "<color=#00ff00>三叶草</color>\r\n购买后使用{0}阳光培养\r\n培养成功应该能增加幸运值\r\n而且可以吹风", 4));
		AddItem(new ConfPlantCardsItem(5, "香蒲", "Shop/Plants/strongBg", "Shop/Plants/Cattail", 90, 225, "<color=#9932CD>香蒲</color>\r\n购买后可以使用{0}阳光在<color=#00ff00>荷叶</color>上培养\r\n必须要有荷叶!!!\r\n能进行一个全屏的攻击，而且能戳破气球", 5));
		AddItem(new ConfPlantCardsItem(6, "樱桃炸弹", "Shop/Plants/normalBg", "Shop/Plants/CherryBomb", 70, 150, "<color=#ff0000>樱桃炸弹</color>\r\n购买后可以使用{0}阳光培养\r\n培养完可以在战斗中使用\r\n把僵尸炸成渣渣", 6));
		AddItem(new ConfPlantCardsItem(7, "大嘴花", "Shop/Plants/normalBg", "Shop/Plants/Chomper", 20, 150, "<color=#00ff00>大嘴花</color>\r\n购买后可以使用{0}阳光培养\r\n她可以把僵尸进行一整个的吞\r\n把僵尸消化干净", 7));
		AddItem(new ConfPlantCardsItem(8, "咖啡豆", "Shop/Plants/normalBg", "Shop/Plants/CoffeeBean", 40, 75, "<color=#00ff00>咖啡豆</color>\r\n购买后可以使用{0}阳光培养\r\n虽然不能消灭僵尸\r\n但是吃下可以增加我的基础属性\r\n<color=#ff0000>会减少8最大生命值和5护甲</color>", 8));
		AddItem(new ConfPlantCardsItem(9, "玉米投手", "Shop/Plants/normalBg", "Shop/Plants/CornpultCard", 30, 100, "<color=#00ff00>玉米投手</color>\r\n购买后可以使用{0}阳光培养\r\n概率投出黄油，使僵尸不能动弹\r\n最重要的是她能合成<color=#9932CD>玉米加农炮</color>", 9));
		AddItem(new ConfPlantCardsItem(10, "大喷菇", "Shop/Plants/normalBg", "Shop/Plants/FumeShroom", 40, 75, "<color=#00ff00>大喷菇</color>\r\n购买后可以使用{0}阳光培养\r\n能攻击一条线的敌人\r\n她能合成<color=#9932CD>忧郁蘑菇</color>", 10));
		AddItem(new ConfPlantCardsItem(11, "加特林豌豆射手", "Shop/Plants/strongBg", "Shop/Plants/GatlingPea", 35, 250, "<color=#9932CD>加特林豌豆射手</color>\r\n可以使用{0}阳光使双发射手进化成加特林射手\r\n能一次发射4颗豌豆\r\n用豌豆塞满僵尸的嘴", 11));
		AddItem(new ConfPlantCardsItem(12, "多嘴小蘑菇", "Shop/Plants/strongBg", "Shop/Plants/GloomShroom", 80, 150, "<color=#9932CD>多嘴小蘑菇</color>\r\n可以使用{0}阳光使大喷菇进化\r\n围绕四周释放大量延误\r\n烟雾能造成多段伤害", 12));
		AddItem(new ConfPlantCardsItem(13, "吸金菇", "Shop/Plants/normalBg", "Shop/Plants/GoldMagent", 50, 150, "<color=#00ff00>吸金菇</color>\r\n可以使用{0}阳光培养\r\n有概率吸取场上掉落的金币\r\n这样就不要一个一个收集钱币了", 13));
		AddItem(new ConfPlantCardsItem(14, "荷叶", "Shop/Plants/normalBg", "Shop/Plants/LilyPad", 15, 25, "<color=#00ff00>荷叶</color>\r\n可以使用{0}阳光培养\r\n有了荷叶就能种植香蒲了", 14));
		AddItem(new ConfPlantCardsItem(15, "大蒜", "Shop/Plants/normalBg", "Shop/Plants/Gralic", 35, 50, "<color=#00ff00>大蒜</color>\r\n可以使用{0}阳光培养\r\n虽然不能消灭僵尸\r\n但是吃下可以增加我的基础属性\r\n<color=#ff0000>会减少10伤害，5幸运</color>", 15));
		AddItem(new ConfPlantCardsItem(16, "墓碑吞噬者", "Shop/Plants/normalBg", "Shop/Plants/Gravebuster", 70, 75, "<color=#00ff00>墓碑吞噬者</color>\r\n可以使用{0}阳光培养\r\n有一定概率吞噬掉僵尸的墓碑，使之不能出场", 16));
		AddItem(new ConfPlantCardsItem(17, "魅惑菇", "Shop/Plants/normalBg", "Shop/Plants/HypnoShroom", 90, 75, "<color=#00ff00>魅惑菇</color>\r\n可以使用{0}阳光培养\r\n可以魅惑一只僵尸攻击同类", 17));
		AddItem(new ConfPlantCardsItem(18, "磁力菇", "Shop/Plants/normalBg", "Shop/Plants/MagentShroom", 35, 100, "<color=#00ff00>磁力菇</color>\r\n可以使用{0}阳光培养\r\n可以吸收铁制品换钱~", 18));
		AddItem(new ConfPlantCardsItem(19, "金盏花", "Shop/Plants/normalBg", "Shop/Plants/Marigold", 50, 50, "<color=#00ff00>金盏花</color>\r\n可以使用{0}阳光培养\r\n每过一段时间会产生钱币", 19));
		AddItem(new ConfPlantCardsItem(20, "路灯", "Shop/Plants/normalBg", "Shop/Plants/Plantern", 70, 25, "<color=#00ff00>路灯</color>\r\n可以使用{0}阳光培养\r\n在她附近我变强了很多", 20));
		AddItem(new ConfPlantCardsItem(21, "小喷菇", "Shop/Plants/normalBg", "Shop/Plants/PuffShroom", 15, 0, "<color=#00ff00>小喷菇</color>\r\n可以使用{0}阳光培养\r\n可以无限的成长，而且培养需要的资源非常少", 21));
		AddItem(new ConfPlantCardsItem(22, "南瓜", "Shop/Plants/normalBg", "Shop/Plants/PumpkinHead", 50, 125, "<color=#00ff00>南瓜</color>\r\n可以使用{0}阳光培养\r\n可以保护我的脑子不被僵尸吃掉\r\n重生效果不能叠加", 22));
		AddItem(new ConfPlantCardsItem(23, "胆小菇", "Shop/Plants/normalBg", "Shop/Plants/ScaredyShroom", 20, 25, "<color=#00ff00>胆小菇</color>\r\n可以使用{0}阳光培养\r\n默认射程很远，但是很胆小\r\n僵尸靠近她时会缩到地底下用菇头进行攻击", 23));
		AddItem(new ConfPlantCardsItem(24, "寒冰豌豆", "Shop/Plants/normalBg", "Shop/Plants/SnowPea", 40, 175, "<color=#00ff00>寒冰豌豆</color>\r\n可以使用{0}阳光培养\r\n能够冰冻住僵尸\r\n虽然温度对僵尸毫无影响，但是冻住能减缓他们的移动速度", 24));
		AddItem(new ConfPlantCardsItem(25, "地刺王", "Shop/Plants/strongBg", "Shop/Plants/Spikerock", 50, 125, "<color=#9932CD>地刺王</color>\r\n可以使用{0}阳光将地刺进化\r\n更加坚硬，可以破坏的载具数量增多了", 25));
		AddItem(new ConfPlantCardsItem(26, "地刺", "Shop/Plants/normalBg", "Shop/Plants/Spikeweed", 20, 100, "<color=#00ff00>地刺</color>\r\n可以使用{0}阳光培养\r\n可以破坏载具\r\n僵尸踩上去会难以行动", 26));
		AddItem(new ConfPlantCardsItem(27, "裂荚豌豆", "Shop/Plants/normalBg", "Shop/Plants/SplitPea", 50, 125, "<color=#00ff00>裂荚豌豆</color>\r\n可以使用{0}阳光培养\r\n有两个头的豌豆，能同时攻击两侧", 27));
		AddItem(new ConfPlantCardsItem(28, "杨桃", "Shop/Plants/normalBg", "Shop/Plants/Starfruit", 90, 125, "<color=#00ff00>杨桃</color>\r\n可以使用{0}阳光培养\r\n能同时向五个方向发射子弹，非常强", 28));
		AddItem(new ConfPlantCardsItem(29, "向日葵", "Shop/Plants/normalBg", "Shop/Plants/SunFlower", 20, 50, "<color=#00ff00>向日葵</color>\r\n可以使用{0}阳光培养\r\n每隔一段时间能产生阳光", 29));
		AddItem(new ConfPlantCardsItem(30, "高坚果", "Shop/Plants/normalBg", "Shop/Plants/TallNut", 40, 125, "<color=#00ff00>高坚果</color>\r\n可以使用{0}阳光培养\r\n有高坚果时对僵尸有嘲讽作用", 30));
		AddItem(new ConfPlantCardsItem(31, "三发豌豆", "Shop/Plants/normalBg", "Shop/Plants/Threepeater", 50, 325, "<color=#00ff00>三发豌豆</color>\r\n可以使用{0}阳光培养\r\n能向一个方向发射三枚豌豆", 31));
		AddItem(new ConfPlantCardsItem(32, "火炬树桩", "Shop/Plants/normalBg", "Shop/Plants/Torchwood", 90, 175, "<color=#00ff00>火炬树桩</color>\r\n可以使用{0}阳光培养\r\n能加强所有豌豆的伤害，非常强力", 32));
		AddItem(new ConfPlantCardsItem(33, "双子向日葵", "Shop/Plants/strongBg", "Shop/Plants/TwinSunflower", 50, 150, "<color=#00ff00>双子向日葵</color>\r\n可以使用{0}阳光培养\r\n两个头的向日葵，每次产生两阳光", 33));
		AddItem(new ConfPlantCardsItem(34, "坚果", "Shop/Plants/normalBg", "Shop/Plants/WallNut", 30, 50, "<color=#00ff00>坚果</color>\r\n可以使用{0}阳光培养\r\n像保龄球一样砸飞僵尸", 34));
		AddItem(new ConfPlantCardsItem(35, "寒冰菇", "Shop/Plants/normalBg", "Shop/Plants/IceShroom", 50, 75, "<color=#ff0000>寒冰菇</color>\r\n可以使用{0}阳光培养\r\n可以冰冻全场的僵尸", 35));
		AddItem(new ConfPlantCardsItem(36, "火爆辣椒", "Shop/Plants/normalBg", "Shop/Plants/Jalapeno", 100, 125, "<color=#ff0000>火爆辣椒</color>\r\n可以使用{0}阳光培养\r\n可以将一片僵尸烧为灰烬", 36));
		AddItem(new ConfPlantCardsItem(37, "毁灭菇", "Shop/Plants/normalBg", "Shop/Plants/DoomShroom", 120, 125, "<color=#ff0000>毁灭菇</color>\r\n可以使用{0}阳光培养\r\n可以对所有已出现的僵尸造成伤害\r\n对环境会有不可逆的破坏,请小心使用", 37));
		AddItem(new ConfPlantCardsItem(38, "窝瓜", "Shop/Plants/normalBg", "Shop/Plants/Squash", 30, 50, "<color=#ff0000>窝瓜</color>\r\n可以使用{0}阳光培养\r\n可以一屁股将僵尸压扁，压成僵尸干", 38));
		AddItem(new ConfPlantCardsItem(39, "土豆雷", "Shop/Plants/normalBg", "Shop/Plants/PotatoMine", 30, 25, "<color=#ff0000>土豆雷</color>\r\n可以使用{0}阳光培养\r\n可以放置土豆地雷，僵尸踩上去会产生范围爆炸", 39));
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
	
