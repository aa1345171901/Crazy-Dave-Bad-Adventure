using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfLocalTextItem : ConfBaseItem
{
	/// <summary>
	/// 键值
	/// </summary>
	public string key;

	/// <summary>
	/// 中文文本
	/// </summary>
	public string cn;

	/// <summary>
	/// 英文
	/// </summary>
	public string en;


	public ConfLocalTextItem()
	{
	}

	public ConfLocalTextItem(int id, string key, string cn, string en)
	{
		this.id = id;
		this.key = key;
		this.cn = cn;
		this.en = en;
	}	

	public ConfLocalTextItem Clone()
	{
		ConfLocalTextItem item = (ConfLocalTextItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfLocalTextBase : ConfBase<ConfLocalTextItem>
{
    public override void Init()
    {
		confName = "LocalText";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfLocalTextItem(1, "plantName_Peashooter", "豌豆射手", "Peashooter"));
		AddItem(new ConfLocalTextItem(2, "plantName_Repeater", "双发豌豆", "Repeater"));
		AddItem(new ConfLocalTextItem(3, "plantName_Cactus", "仙人掌", "Cactus"));
		AddItem(new ConfLocalTextItem(4, "plantName_Blover", "三叶草", "Blover"));
		AddItem(new ConfLocalTextItem(5, "plantName_Cattail", "香蒲", "Cattail"));
		AddItem(new ConfLocalTextItem(6, "plantName_CherryBomb", "樱桃炸弹", "CherryBomb"));
		AddItem(new ConfLocalTextItem(7, "plantName_Chomper", "大嘴花", "Chomper"));
		AddItem(new ConfLocalTextItem(8, "plantName_CoffeeBean", "咖啡豆", "CoffeeBean"));
		AddItem(new ConfLocalTextItem(9, "plantName_CornpultCard", "玉米投手", "CornpultCard"));
		AddItem(new ConfLocalTextItem(10, "plantName_FumeShroom", "大喷菇", "FumeShroom"));
		AddItem(new ConfLocalTextItem(11, "plantName_GatlingPea", "加特林豌豆射手", "GatlingPea"));
		AddItem(new ConfLocalTextItem(12, "plantName_GloomShroom", "多嘴小蘑菇", "GloomShroom"));
		AddItem(new ConfLocalTextItem(13, "plantName_GoldMagent", "吸金菇", "GoldMagent"));
		AddItem(new ConfLocalTextItem(14, "plantName_LilyPad", "荷叶", "LilyPad"));
		AddItem(new ConfLocalTextItem(15, "plantName_Gralic", "大蒜", "Gralic"));
		AddItem(new ConfLocalTextItem(16, "plantName_Gravebuster", "墓碑吞噬者", "Gravebuster"));
		AddItem(new ConfLocalTextItem(17, "plantName_HypnoShroom", "魅惑菇", "HypnoShroom"));
		AddItem(new ConfLocalTextItem(18, "plantName_MagentShroom", "磁力菇", "MagentShroom"));
		AddItem(new ConfLocalTextItem(19, "plantName_Marigold", "金盏花", "Marigold"));
		AddItem(new ConfLocalTextItem(20, "plantName_Plantern", "路灯", "Plantern"));
		AddItem(new ConfLocalTextItem(21, "plantName_PuffShroom", "小喷菇", "PuffShroom"));
		AddItem(new ConfLocalTextItem(22, "plantName_PumpkinHead", "南瓜", "PumpkinHead"));
		AddItem(new ConfLocalTextItem(23, "plantName_ScaredyShroom", "胆小菇", "ScaredyShroom"));
		AddItem(new ConfLocalTextItem(24, "plantName_SnowPea", "寒冰豌豆", "SnowPea"));
		AddItem(new ConfLocalTextItem(25, "plantName_Spikerock", "地刺王", "Spikerock"));
		AddItem(new ConfLocalTextItem(26, "plantName_Spikeweed", "地刺", "Spikeweed"));
		AddItem(new ConfLocalTextItem(27, "plantName_SplitPea", "裂荚豌豆", "SplitPea"));
		AddItem(new ConfLocalTextItem(28, "plantName_Starfruit", "杨桃", "Starfruit"));
		AddItem(new ConfLocalTextItem(29, "plantName_SunFlower", "向日葵", "SunFlower"));
		AddItem(new ConfLocalTextItem(30, "plantName_TallNut", "高坚果", "TallNut"));
		AddItem(new ConfLocalTextItem(31, "plantName_Threepeater", "三发豌豆", "Threepeater"));
		AddItem(new ConfLocalTextItem(32, "plantName_Torchwood", "火炬树桩", "Torchwood"));
		AddItem(new ConfLocalTextItem(33, "plantName_TwinSunflower", "双子向日葵", "TwinSunflower"));
		AddItem(new ConfLocalTextItem(34, "plantName_WallNut", "坚果", "WallNut"));
		AddItem(new ConfLocalTextItem(35, "plantName_IceShroom", "寒冰菇", "IceShroom"));
		AddItem(new ConfLocalTextItem(36, "plantName_Jalapeno", "火爆辣椒", "Jalapeno"));
		AddItem(new ConfLocalTextItem(37, "plantName_DoomShroom", "毁灭菇", "DoomShroom"));
		AddItem(new ConfLocalTextItem(38, "plantName_Squash", "窝瓜", "Squash"));
		AddItem(new ConfLocalTextItem(39, "plantName_PotatoMine", "土豆雷", "PotatoMine"));
		AddItem(new ConfLocalTextItem(40, "plantInfo_Peashooter", "<color=#00ff00>豌豆射手</color>\r\n购买后使用{0}阳光培养便能随我杀敌\r\n很划算的啦", "<color=#00ff00>Peashooterr</color>\r\nAfter purchasing, use {0}Sunlight Training to train it and it will be able to kill enemies with me\r\nIt's a good deal"));
		AddItem(new ConfLocalTextItem(41, "plantInfo_Repeater", "<color=#9932CD>双发豌豆射手</color>\r\n购买后使用{0}阳光可将豌豆射手进化成双发射手\r\n一次性可以发射两枚豌豆", "<color=#9932CD>Repeater Shooter</color>\r\nAfter purchase, use {0} sunlight to evolve the Pea Shooter into a Dual Shooter\r\nCan shoot two peas at a time"));
		AddItem(new ConfLocalTextItem(42, "plantInfo_Cactus", "<color=#00ff00>仙人掌</color>\r\n购买后使用{0}阳光培养\r\n可以戳破气球僵尸的气球\r\n这很重要", "<color=#00ff00>Cactus</color>\r\nAfter purchase, use {0}sunlight to cultivate\r\nYou can pop the balloon of the Balloon Zombie\r\nThis is very important"));
		AddItem(new ConfLocalTextItem(43, "plantInfo_Blover", "<color=#00ff00>三叶草</color>\r\n购买后使用{0}阳光培养\r\n培养成功应该能增加幸运值\r\n而且可以吹风", "<color=#00ff00>Blover</color>\r\nAfter purchase, use {0}sunlight to cultivate\r\nSuccessful cultivation should increase Lucky\r\nAnd can blow wind"));
		AddItem(new ConfLocalTextItem(44, "plantInfo_Cattail", "<color=#9932CD>香蒲</color>\r\n购买后可以使用{0}阳光在<color=#00ff00>荷叶</color>上培养\r\n必须要有荷叶!!!\r\n能进行一个全屏的攻击，而且能戳破气球", "<color=#9932CD>Cattail</color>\r\nAfter purchase, you can use {0} sunlight to cultivate it on <color=#00ff00>Lotus Leaf</color>\r\nMust have Lotus Leaf!!!\r\nCan perform a full-screen attack and can pop balloons"));
		AddItem(new ConfLocalTextItem(45, "plantInfo_CherryBomb", "<color=#ff0000>樱桃炸弹</color>\r\n购买后可以使用{0}阳光培养\r\n培养完可以在战斗中使用\r\n把僵尸炸成渣渣", "<color=#ff0000>CherryBombb</color>\r\nAfter purchase, you can use {0} sunlight to cultivate\r\nAfter cultivation, you can use it in battle\r\nBlast the zombies into slag"));
		AddItem(new ConfLocalTextItem(46, "plantInfo_Chomper", "<color=#00ff00>大嘴花</color>\r\n购买后可以使用{0}阳光培养\r\n她可以把僵尸进行一整个的吞\r\n把僵尸消化干净", "<color=#00ff00>Chomperth Flower</color>\r\nAfter purchase, you can use {0} sunlight to cultivate\r\nShe can swallow zombies whole\r\nDigest the zombies completely"));
		AddItem(new ConfLocalTextItem(47, "plantInfo_CoffeeBean", "<color=#00ff00>咖啡豆</color>\r\n购买后可以使用{0}阳光培养\r\n虽然不能消灭僵尸\r\n但是吃下可以增加我的基础属性\r\n<color=#ff0000>会减少8最大生命值和5护甲</color>", "<color=#00ff00>CoffeeBeanns</color>\r\nAfter purchase, you can use {0} sunlight to cultivate\r\nAlthough you can't kill zombies\r\nBut eating them can increase my basic attributes\r\n<color=#ff0000>will reduce 8 MaximumHP and 5 armor</color>"));
		AddItem(new ConfLocalTextItem(48, "plantInfo_CornpultCard", "<color=#00ff00>玉米投手</color>\r\n购买后可以使用{0}阳光培养\r\n概率投出黄油，使僵尸不能动弹\r\n最重要的是她能合成<color=#9932CD>玉米加农炮</color>", "<color=#00ff00>CornpultCard</color>\r\nAfter purchase, you can use {0} sunlight to cultivate\r\nIt has a chance to throw butter, making zombies unable to move\r\nThe most important thing is that she can synthesize <color=#9932CD>Corn Cannon</color>"));
		AddItem(new ConfLocalTextItem(49, "plantInfo_FumeShroom", "<color=#00ff00>大喷菇</color>\r\n购买后可以使用{0}阳光培养\r\n能攻击一条线的敌人\r\n她能合成<color=#9932CD>忧郁蘑菇</color>", "<color=#00ff00>FumeShroomon Mushroom</color>\r\nAfter purchase, it can be cultivated with {0} sunlight\r\nIt can attack enemies in a line\r\nIt can synthesize <color=#9932CD>Melancholy Mushroom</color>"));
		AddItem(new ConfLocalTextItem(50, "plantInfo_GatlingPea", "<color=#9932CD>加特林豌豆射手</color>\r\n可以使用{0}阳光使双发射手进化成加特林射手\r\n能一次发射4颗豌豆\r\n用豌豆塞满僵尸的嘴", "<color=#9932CD>GatlingPeaa Shooter</color>\r\nYou can use {0} sunlight to evolve the Dual Shooter into a Gatling Shooter\r\nCan shoot 4 peas at a time\r\nFill the zombies' mouths with peas"));
		AddItem(new ConfLocalTextItem(51, "plantInfo_GloomShroom", "<color=#9932CD>多嘴小蘑菇</color>\r\n可以使用{0}阳光使大喷菇进化\r\n围绕四周释放大量延误\r\n烟雾能造成多段伤害", "<color=#9932CD>GloomShroomroom</color>\r\nYou can use {0} sunlight to evolve the Big Ejection Mushroom\r\nRelease a large number of delays around\r\nThe smoke can cause multiple PercentageDamage"));
		AddItem(new ConfLocalTextItem(52, "plantInfo_GoldMagent", "<color=#00ff00>吸金菇</color>\r\n可以使用{0}阳光培养\r\n有概率吸取场上掉落的金币\r\n这样就不要一个一个收集钱币了", "<color=#00ff00>GoldMagentbing Mushroom</color>\r\nYou can use {0} sunlight to cultivate\r\nThere is a chance to absorb the gold coins dropped on the field\r\nIn this way, you don't have to collect coins one by one"));
		AddItem(new ConfLocalTextItem(53, "plantInfo_LilyPad", "<color=#00ff00>荷叶</color>\r\n可以使用{0}阳光培养\r\n有了荷叶就能种植香蒲了", "<color=#00ff00>LilyPadeaf</color>\r\nYou can use {0} sunlight to cultivate\r\nWith lotus leaves, you can plant cattails"));
		AddItem(new ConfLocalTextItem(54, "plantInfo_Gralic", "<color=#00ff00>大蒜</color>\r\n可以使用{0}阳光培养\r\n虽然不能消灭僵尸\r\n但是吃下可以增加我的基础属性\r\n<color=#ff0000>会减少10伤害，5幸运</color>", "<color=#00ff00>Gralic</color>\r\nCan be cultivated using {0} sunlight\r\nAlthough it cannot kill zombies\r\nBut eating it can increase my basic attributes\r\n<color=#ff0000>will reduce 10 PercentageDamage, 5 Lucky</color>"));
		AddItem(new ConfLocalTextItem(55, "plantInfo_Gravebuster", "<color=#00ff00>墓碑吞噬者</color>\r\n可以使用{0}阳光培养\r\n有一定概率吞噬掉僵尸的墓碑，使之不能出场", "<color=#00ff00>Gravebusterevourer</color>\r\nCan be cultivated using {0} sunlight\r\nThere is a certain probability that it will devour the tombstone of a zombie, preventing it from appearing"));
		AddItem(new ConfLocalTextItem(56, "plantInfo_HypnoShroom", "<color=#00ff00>魅惑菇</color>\r\n可以使用{0}阳光培养\r\n可以魅惑一只僵尸攻击同类", "<color=#00ff00>HypnoShroomshroom</color>\r\nCan be cultivated with {0} sunlight\r\nCan charm a zombie to attack the same kind"));
		AddItem(new ConfLocalTextItem(57, "plantInfo_MagentShroom", "<color=#00ff00>磁力菇</color>\r\n可以使用{0}阳光培养\r\n可以吸收铁制品换钱~", "<color=#00ff00>MagentShroomhroom</color>\r\nCan be cultivated with {0} sunlight\r\nCan absorb iron products in exchange for money~"));
		AddItem(new ConfLocalTextItem(58, "plantInfo_Marigold", "<color=#00ff00>金盏花</color>\r\n可以使用{0}阳光培养\r\n每过一段时间会产生钱币", "<color=#00ff00>Marigold</color>\r\nCan be cultivated with {0} sunlight\r\nCoins will be generated every once in a while"));
		AddItem(new ConfLocalTextItem(59, "plantInfo_Plantern", "<color=#00ff00>路灯</color>\r\n可以使用{0}阳光培养\r\n在她附近我变强了很多", "<color=#00ff00>Planternight</color>\r\nCan be cultivated with {0} sunlight\r\nI became much stronger near her"));
		AddItem(new ConfLocalTextItem(60, "plantInfo_PuffShroom", "<color=#00ff00>小喷菇</color>\r\n可以使用{0}阳光培养\r\n可以无限的成长，而且培养需要的资源非常少", "<color=#00ff00>PuffShroomy Mushroom</color>\r\nCan be cultivated using {0} sunlight\r\nCan grow infinitely, and requires very few resources to cultivate"));
		AddItem(new ConfLocalTextItem(61, "plantInfo_PumpkinHead", "<color=#00ff00>南瓜</color>\r\n可以使用{0}阳光培养\r\n可以保护我的脑子不被僵尸吃掉\r\n重生效果不能叠加", "<color=#00ff00>PumpkinHeadlor>\r\nCan be cultivated using {0} sunlight\r\nCan protect my brain from being eaten by zombies\r\nRebirth effects cannot be stacked"));
		AddItem(new ConfLocalTextItem(62, "plantInfo_ScaredyShroom", "<color=#00ff00>胆小菇</color>\r\n可以使用{0}阳光培养\r\n默认射程很远，但是很胆小\r\n僵尸靠近她时会缩到地底下用菇头进行攻击", "<color=#00ff00>ScaredyShroomm</color>\r\nCan be cultivated using {0} sunlight\r\nThe default range is very long, but it is very timid\r\nWhen zombies approach her, they will shrink underground and attack with their mushroom heads"));
		AddItem(new ConfLocalTextItem(63, "plantInfo_SnowPea", "<color=#00ff00>寒冰豌豆</color>\r\n可以使用{0}阳光培养\r\n能够冰冻住僵尸\r\n虽然温度对僵尸毫无影响，但是冻住能减缓他们的移动速度", "<color=#00ff00>SnowPea</color>\r\nCan be cultivated using {0} sunlight\r\nCan freeze zombies\r\nAlthough temperature has no effect on zombies, freezing them can slow down their movement speed"));
		AddItem(new ConfLocalTextItem(64, "plantInfo_Spikerock", "<color=#9932CD>地刺王</color>\r\n可以使用{0}阳光将地刺进化\r\n更加坚硬，可以破坏的载具数量增多了", "<color=#9932CD>Spikerockke King</color>\r\nYou can use {0} sunlight to evolve the Earth Spike\r\nIt is harder and can destroy more vehicles"));
		AddItem(new ConfLocalTextItem(65, "plantInfo_Spikeweed", "<color=#00ff00>地刺</color>\r\n可以使用{0}阳光培养\r\n可以破坏载具\r\n僵尸踩上去会难以行动", "<color=#00ff00>Spikeweedikes</color>\r\nCan be cultivated with {0} sunlight\r\nCan destroy vehicles\r\nZombies will have difficulty moving if they step on them"));
		AddItem(new ConfLocalTextItem(66, "plantInfo_SplitPea", "<color=#00ff00>裂荚豌豆</color>\r\n可以使用{0}阳光培养\r\n有两个头的豌豆，能同时攻击两侧", "<color=#00ff00>SplitPeaa</color>\r\nCan be cultivated with {0} sunlight\r\nA pea with two heads, which can attack both sides at the same time"));
		AddItem(new ConfLocalTextItem(67, "plantInfo_Starfruit", "<color=#00ff00>杨桃</color>\r\n可以使用{0}阳光培养\r\n能同时向五个方向发射子弹，非常强", "<color=#00ff00>Starfruit</color>\r\nCan be cultivated with {0} sunlight\r\nCan fire bullets in five directions at the same time, very powerful"));
		AddItem(new ConfLocalTextItem(68, "plantInfo_SunFlower", "<color=#00ff00>向日葵</color>\r\n可以使用{0}阳光培养\r\n每隔一段时间能产生阳光", "<color=#00ff00>SunFlower</color>\r\nCan be cultivated with {0} sunlight\r\nCan generate sunlight every once in a while"));
		AddItem(new ConfLocalTextItem(69, "plantInfo_TallNut", "<color=#00ff00>高坚果</color>\r\n可以使用{0}阳光培养\r\n有高坚果时对僵尸有嘲讽作用", "<color=#00ff00>TallNutts</color>\r\nCan be cultivated with {0} sunlight\r\nHigh Nuts have a taunting effect on zombies"));
		AddItem(new ConfLocalTextItem(70, "plantInfo_Threepeater", "<color=#00ff00>三发豌豆</color>\r\n可以使用{0}阳光培养\r\n能向一个方向发射三枚豌豆", "<color=#00ff00>Threepeater</color>\r\nCan be cultivated with {0} sunlight\r\nCan shoot three peas in one direction"));
		AddItem(new ConfLocalTextItem(71, "plantInfo_Torchwood", "<color=#00ff00>火炬树桩</color>\r\n可以使用{0}阳光培养\r\n能加强所有豌豆的伤害，非常强力", "<color=#00ff00>Torchwoodmp</color>\r\nCan be cultivated using {0} sunlight\r\nCan increase the PercentageDamage of all peas, very powerful"));
		AddItem(new ConfLocalTextItem(72, "plantInfo_TwinSunflower", "<color=#00ff00>双子向日葵</color>\r\n可以使用{0}阳光培养\r\n两个头的向日葵，每次产生两阳光", "<color=#00ff00>TwinSunflowerrs</color>\r\nCan use {0} sunlight to cultivate\r\nTwo heads of sunflowers, each producing two sunlight"));
		AddItem(new ConfLocalTextItem(73, "plantInfo_WallNut", "<color=#00ff00>坚果</color>\r\n可以使用{0}阳光培养\r\n像保龄球一样砸飞僵尸", "<color=#00ff00>WallNutolor>\r\nCan be cultivated with {0} sunlight\r\nSmash zombies like a bowling ball"));
		AddItem(new ConfLocalTextItem(74, "plantInfo_IceShroom", "<color=#ff0000>寒冰菇</color>\r\n可以使用{0}阳光培养\r\n可以冰冻全场的僵尸", "<color=#ff0000>IceShroomoom</color>\r\nCan be cultivated using {0} sunlight\r\nCan freeze all zombies in the field"));
		AddItem(new ConfLocalTextItem(75, "plantInfo_Jalapeno", "<color=#ff0000>火爆辣椒</color>\r\n可以使用{0}阳光培养\r\n可以将一片僵尸烧为灰烬", "<color=#ff0000>Jalapenoili</color>\r\nCan be cultivated using {0} sunlight\r\nCan burn a group of zombies to ashes"));
		AddItem(new ConfLocalTextItem(76, "plantInfo_DoomShroom", "<color=#ff0000>毁灭菇</color>\r\n可以使用{0}阳光培养\r\n可以对所有已出现的僵尸造成伤害\r\n对环境会有不可逆的破坏,请小心使用", "<color=#ff0000>DoomShroomn Mushroom</color>\r\nCan be cultivated using {0} sunlight\r\nCan cause PercentageDamage to all zombies that have appeared\r\nWill cause irreversible PercentageDamage to the environment, please use with caution"));
		AddItem(new ConfLocalTextItem(77, "plantInfo_Squash", "<color=#ff0000>窝瓜</color>\r\n可以使用{0}阳光培养\r\n可以一屁股将僵尸压扁，压成僵尸干", "<color=#ff0000>Squash/color>\r\nCan use {0} sunlight to cultivate\r\nCan crush zombies with one butt, turning them into zombies"));
		AddItem(new ConfLocalTextItem(78, "plantInfo_PotatoMine", "<color=#ff0000>土豆雷</color>\r\n可以使用{0}阳光培养\r\n可以放置土豆地雷，僵尸踩上去会产生范围爆炸", "<color=#ff0000>PotatoMinee</color>\r\nCan be cultivated using {0} sunlight\r\nPotato mines can be placed, zombies stepping on them will cause a range explosion"));
		AddItem(new ConfLocalTextItem(79, "propInfo_jaw", "一个下巴？\r\n<color=#00ff00>+2肾上腺素</color>\r\n<color=#00ff00>+2生命恢复</color>\r\n<color=#00ff00>+5攻击速度</color>\r\n<color=#ff0000>-2最大生命值</color>\r\n看起来令人毛骨悚然", "A chin? \r\n<color=#00ff00>+2 Adrenaline</color>\r\n<color=#00ff00>+2 LifeRecovery</color>\r\n<color=#00ff00>+5 Attack Speed</color>\r\n<color=#ff0000>-2 MaximumHP</color>\r\nLooks pretty creepy."));
		AddItem(new ConfLocalTextItem(80, "propInfo_sadheart", "正在哭泣的红心\r\n<color=#00ff00>+5最大生命值</color>\r\n<color=#ff0000>-3肾上腺素</color>\r\n她看起来好可怜", "Crying Red Heart\r\n<color=#00ff00>+5 Max HP</color>\r\n<color=#ff0000>-3 Adrenaline</color>\r\nShe looks so pitiful"));
		AddItem(new ConfLocalTextItem(81, "propInfo_magnetic", "牵引金币的磁铁\r\n<color=#ff0000>-3速度</color>\r\n有了它我就不需要一个一个点击收集<color=#ff00ff>金币</color>了\r\n但是相互作用力之下，我的速度也收到了影响", "Magnet that pulls gold coins\r\n<color=#ff0000>-3 speed</color>\r\nWith it, I don't need to click one by one to collect <color=#ff00ff>gold coins</color>\r\nBut under the interaction force, my speed is also affected"));
		AddItem(new ConfLocalTextItem(82, "propInfo_blackhole", "吸收阳光的黑洞\r\n<color=#ff0000>-4植物学</color>\r\n有了它我就不需要一个一个点击收集<color=#ff00ff>阳光</color>了\r\n植物好像非常害怕它", "Black hole that absorbs Sunshine\r\n<color=#ff0000>-4 Botany</color>\r\nWith it, I don't need to click one by one to collect <color=#ff00ff>Sunshine</color>\r\nPlants seem to be very afraid of it"));
		AddItem(new ConfLocalTextItem(83, "propInfo_plate", "金属盘子\r\n<color=#00ff00>+2最大生命值</color>\r\n<color=#00ff00>+10金币</color>\r\n<color=#00ff00>+1护甲</color>\r\n<color=#ff0000>-5肾上腺素</color>\r\n可以用来干饭", "Metal Plate\r\n<color=#00ff00>+2 Max HP</color>\r\n<color=#00ff00>+10 Gold</color>\r\n<color=#00ff00>+1 Armor</color>\r\n<color=#ff0000>-5 Adrenaline</color>\r\nCan be used to eat"));
		AddItem(new ConfLocalTextItem(84, "propInfo_throat", "长牙齿的咽喉？\r\n<color=#00ff00>+5最大生命值</color>\r\n<color=#00ff00>+5肾上腺素</color>\r\n<color=#ff0000>-5生命恢复</color>\r\n看起来更像一条蛇", "A throat with teeth? \r\n<color=#00ff00>+5 Max Health</color>\r\n<color=#00ff00>+5 Adrenaline</color>\r\n<color=#ff0000>-5 Health Regeneration</color>\r\nLooks more like a snake"));
		AddItem(new ConfLocalTextItem(85, "propInfo_SeaShroom_head", "小海菇的帽子\r\n<color=#00ff00>+2幸运</color>\r\n<color=#00ff00>+3植物学</color>\r\n<color=#00ff00>+5肾上腺素</color>\r\n<color=#ff0000>-3最大生命值</color>\r\n可以好好研究一下", "Little Mushroom's Hat\r\n<color=#00ff00>+2 Lucky</color>\r\n<color=#00ff00>+3 Botany</color>\r\n<color=#00ff00>+5 Adrenaline</color>\r\n<color=#ff0000>-3 MaximumHP</color>\r\nYou can study it carefully."));
		AddItem(new ConfLocalTextItem(86, "propInfo_SunShroom_head", "阳光菇帽子\r\n<color=#00ff00>+50阳光</color>\r\n<color=#00ff00>+3植物学</color>\r\n<color=#00ff00>+4最大生命值</color>\r\n<color=#ff0000>-2力量</color>\r\n可以好好研究一下", "Sunshine Mushroom Hat\r\n<color=#00ff00>+50 Sunshine</color>\r\n<color=#00ff00>+3 Botany</color>\r\n<color=#00ff00>+4 MaximumHP</color>\r\n<color=#ff0000>-2 Power</color>\r\nYou can study it carefully."));
		AddItem(new ConfLocalTextItem(87, "propInfo_beard", "戴夫的胡须\r\n<color=#00ff00>+2护甲</color>\r\n<color=#00ff00>+1幸运</color>\r\n<color=#ff0000>-3肾上腺素</color>\r\n这是我的胡子", "Dave's Beard\r\n<color=#00ff00>+2 Armor</color>\r\n<color=#00ff00>+1 Lucky</color>\r\n<color=#ff0000>-3 Adrenaline</color>\r\nThis is my beard"));
		AddItem(new ConfLocalTextItem(88, "propInfo_glasses", "眼镜\r\n<color=#00ff00>+5范围</color>\r\n<color=#00ff00>+1暴击率</color>\r\n戴上后视野会变得清晰", "Glasses\r\n<color=#00ff00>+5 range</color>\r\n<color=#00ff00>+1 critical hit rate</color>\r\nAfter wearing them, your vision will become clear"));
		AddItem(new ConfLocalTextItem(89, "propInfo_leaf", "叶子\r\n<color=#00ff00>+3植物学</color>\r\n<color=#00ff00>+1幸运</color>\r\n不知道属于哪个植物", "Leaf\r\n<color=#00ff00>+3 Botany</color>\r\n<color=#00ff00>+1 Lucky</color>\r\nUnknown plant"));
		AddItem(new ConfLocalTextItem(90, "propInfo_smallflower", "小花\r\n<color=#00ff00>+4最大生命值</color>\r\n看起来很眼熟", "Small Flower\r\n<color=#00ff00>+4 MaximumHP</color>\r\nLooks familiar"));
		AddItem(new ConfLocalTextItem(91, "propInfo_fertilizer", "肥料\r\n<color=#00ff00>+4植物学</color>\r\n应该能帮到植物们", "Fertilizer\r\n<color=#00ff00>+4 Botany</color>\r\nShould help the plants"));
		AddItem(new ConfLocalTextItem(92, "propInfo_key", "车钥匙\r\n<color=#9932CD>可以免费刷新一次商品</color>", "Car Key\r\n<color=#9932CD>You can refresh the item once for free</color>"));
		AddItem(new ConfLocalTextItem(93, "propInfo_wallet", "钱袋\r\n<color=#00ff00>+10金币</color>\r\n里面能装不少钱", "Money bag\r\n<color=#00ff00>+10 gold coins</color>\r\nIt can hold a lot of money"));
		AddItem(new ConfLocalTextItem(94, "propInfo_Diamond", "<color=#0000ff>钻石</color>\r\n<color=#00ff00>+30金币</color>\r\n<color=#00ff00>+4幸运</color>\r\n亮闪亮闪的很漂亮", "<color=#0000ff>Diamond</color>\r\n<color=#00ff00>+30 Gold Coins</color>\r\n<color=#00ff00>+4 Lucky</color>\r\nBright and beautiful"));
		AddItem(new ConfLocalTextItem(95, "propInfo_SunFlower", "<color=#0000ff>葵花</color>\r\n<color=#00ff00>+5植物学</color>\r\n<color=#00ff00>+3幸运</color>\r\n<color=#00ff00>+50阳光</color>\r\n向日葵的花瓣，不知道有没有葵花籽", "<color=#0000ff>Sunflower</color>\r\n<color=#00ff00>+5 Botany</color>\r\n<color=#00ff00>+3 Lucky</color>\r\n<color=#00ff00>+50 Sunshine</color>\r\nSunflower petals. I wonder if there are sunflower seeds."));
		AddItem(new ConfLocalTextItem(96, "propInfo_basket", "<color=#0000ff>篮子</color>\r\n<color=#00ff00>+10范围</color>\r\n<color=#00ff00>+3暴击率</color>\r\n<color=#00ff00>+5植物学</color>\r\n<color=#ff0000>-3攻击速度</color>\r\n好像是某个投手的投射器", "<color=#0000ff>Basket</color>\r\n<color=#00ff00>+10 Range</color>\r\n<color=#00ff00>+3 Critical Hit Rate</color>\r\n<color=#00ff00>+5 Botany</color>\r\n<color=#ff0000>-3 Attack Speed</color>\r\nIt seems to be a pitcher's projectile"));
		AddItem(new ConfLocalTextItem(97, "propInfo_rake", "<color=#0000ff>耙子</color>\r\n<color=#00ff00>+5力量</color>\r\n<color=#00ff00>+3暴击率</color>\r\n这件兵器很趁手", "<color=#0000ff>Rake</color>\r\n<color=#00ff00>+5 Power</color>\r\n<color=#00ff00>+3 Critical Hit Rate</color>\r\nThis weapon is very handy"));
		AddItem(new ConfLocalTextItem(98, "propInfo_wateringcan", "<color=#0000ff>水壶</color>\r\n<color=#00ff00>+4植物学</color>\r\n<color=#00ff00>+4护甲</color>\r\n<color=#00ff00>+5生命恢复</color>\r\n<color=#ff0000>-4攻击速度</color>\r\n对植物们很有帮助", "<color=#0000ff>Water Bottle</color>\r\n<color=#00ff00>+4 Botany</color>\r\n<color=#00ff00>+4 Armor</color>\r\n<color=#00ff00>+5 Health Regeneration</color>\r\n<color=#ff0000>-4 Attack Speed</color>\r\nVery helpful for plants"));
		AddItem(new ConfLocalTextItem(99, "propInfo_jackbox", "<color=#0000ff>小丑礼盒</color>\r\n<color=#00ff00>+5幸运</color>\r\n<color=#00ff00>+5肾上腺素</color>\r\n<color=#00ff00>+5速度</color>\r\n<color=#ff0000>-2最大生命值</color>\r\n一个会爆炸的礼盒，我得小心收藏", "<color=#0000ff>Clown Gift Box</color>\r\n<color=#00ff00>+5 Lucky</color>\r\n<color=#00ff00>+5 Adrenaline</color>\r\n<color=#00ff00>+5 Speed</color>\r\n<color=#ff0000>-2 MaximumHP</color>\r\nA gift box that explodes. I have to keep it carefully."));
		AddItem(new ConfLocalTextItem(100, "propInfo_splash3", "<color=#0000ff>一朵水花？</color>\r\n<color=#00ff00>+10最大生命值</color>\r\n<color=#00ff00>+4生命恢复</color>\r\n<color=#00ff00>+10攻击速度</color>\r\n<color=#ff0000>-5肾上腺素</color>\r\n<color=#ff0000>-2护甲</color>\r\n哪个跳水队都比不过这一朵", "<color=#0000ff>A splash? </color>\r\n<color=#00ff00>+10 Max HP</color>\r\n<color=#00ff00>+4 HP Regen</color>\r\n<color=#00ff00>+10 Attack Speed</color>\r\n<color=#ff0000>-5 Adrenaline</color>\r\n<color=#ff0000>-2 Armor</color>\r\nNo diving team can beat this one"));
		AddItem(new ConfLocalTextItem(101, "propInfo_SodRollCap", "<color=#0000ff>一卷草皮？</color>\r\n<color=#00ff00>+6生命恢复</color>\r\n<color=#00ff00>+10速度</color>\r\n<color=#00ff00>+8攻击速度</color>\r\n<color=#ff0000>-2力量</color>\r\n铺开躺上去一定很舒服", "<color=#0000ff>A roll of turf? </color>\r\n<color=#00ff00>+6 HP recovery</color>\r\n<color=#00ff00>+10 speed</color>\r\n<color=#00ff00>+8 attack speed</color>\r\n<color=#ff0000>-2 Power</color>\r\nIt must be very comfortable to spread it out and lie on it"));
		AddItem(new ConfLocalTextItem(102, "propInfo_Spikerock", "<color=#0000ff>一根地刺</color>\r\n<color=#00ff00>+10速度</color>\r\n<color=#00ff00>+10攻击速度</color>\r\n<color=#00ff00>+2生命恢复</color>\r\n<color=#ff0000>-10肾上腺素</color>\r\n都不想踩在上面", "<color=#0000ff>A ground spike</color>\r\n<color=#00ff00>+10 speed</color>\r\n<color=#00ff00>+10 attack speed</color>\r\n<color=#00ff00>+2 LifeRecovery</color>\r\n<color=#ff0000>-10 adrenaline</color>\r\nI don't want to step on it"));
		AddItem(new ConfLocalTextItem(103, "propInfo_Stinky_turn1", "<color=#0000ff>蜗牛</color>\r\n<color=#00ff00>+8护甲</color>\r\n<color=#00ff00>+5植物学</color>\r\n<color=#ff0000>-5速度</color>\r\n<color=#ff0000>-3肾上腺素</color>\r\n应该对植物有所帮助，跑得太慢了", "<color=#0000ff>Snail</color>\r\n<color=#00ff00>+8 Armor</color>\r\n<color=#00ff00>+5 Botany</color>\r\n<color=#ff0000>-5 Speed</color>\r\n<color=#ff0000>-3 Adrenaline</color>\r\nShould help the plants, it's too slow."));
		AddItem(new ConfLocalTextItem(104, "propInfo_bugspray", "<color=#0000ff>杀虫剂</color>\r\n<color=#00ff00>+10植物学</color>\r\n<color=#00ff00>+20肾上腺素</color>\r\n<color=#ff0000>-6生命恢复</color>\r\n气味很难闻，可以帮助植物对抗虫子", "<color=#0000ff>Insecticide</color>\r\n<color=#00ff00>+10 Botany</color>\r\n<color=#00ff00>+20 Adrenaline</color>\r\n<color=#ff0000>-6 LifeRecovery</color>\r\nIt smells terrible, and can help plants fight against bugs"));
		AddItem(new ConfLocalTextItem(105, "propInfo_shovel", "<color=#0000ff>铲子</color>\r\n<color=#00ff00>+10植物学</color>\r\n可以铲掉花园中的泥土，放置花盆", "<color=#0000ff>Shovel</color>\r\n<color=#00ff00>+10 Botany</color>\r\nCan shovel the dirt in the garden and place flower pots"));
		AddItem(new ConfLocalTextItem(106, "propInfo_cardSlot", "<color=#0000ff>卡槽</color>\r\n<color=#00ff00>+10植物学</color>\r\n可以增加出战植物的上限", "<color=#0000ff>Card slot</color>\r\n<color=#00ff00>+10 Botany</color>\r\nCan increase the upper limit of plants that can be used in battle"));
		AddItem(new ConfLocalTextItem(107, "propInfo_book", "<color=#0000ff>书</color>\r\n<color=#00ff00>+15植物学</color>\r\n可以学到许多知识", "<color=#0000ff>Book</color>\r\n<color=#00ff00>+15 Botany</color>\r\nYou can learn a lot of knowledge"));
		AddItem(new ConfLocalTextItem(108, "propInfo_cup", "<color=#0000ff>奖杯</color>\r\n<color=#00ff00>+20金币</color>\r\n<color=#00ff00>+3幸运</color>\r\n荣誉的象征", "<color=#0000ff>Trophy</color>\r\n<color=#00ff00>+20 gold coins</color>\r\n<color=#00ff00>+3 Lucky</color>\r\nSymbol of honor"));
		AddItem(new ConfLocalTextItem(109, "propInfo_sandwich", "<color=#0000ff>三明治</color>\r\n<color=#00ff00>+10最大生命值</color>\r\n<color=#00ff00>+5生命恢复</color>\r\n看起来很好吃", "<color=#0000ff>Sandwich</color>\r\n<color=#00ff00>+10 MaximumHP</color>\r\n<color=#00ff00>+5 LifeRecovery</color>\r\nLooks delicious"));
		AddItem(new ConfLocalTextItem(110, "propInfo_lock", "<color=#0000ff>一把锁</color>\r\n<color=#00ff00>+5伤害</color>\r\n<color=#00ff00>+5暴击</color>\r\n<color=#ff0000>-3攻击速度</color>\r\n可以对准", "<color=#0000ff>A lock</color>\r\n<color=#00ff00>+5 PercentageDamage</color>\r\n<color=#00ff00>+5 critical strike</color>\r\n<color=#ff0000>-3 attack speed</color>\r\nCan be aimed at"));
		AddItem(new ConfLocalTextItem(111, "propInfo_radio", "<color=#0000ff>留声机</color>\r\n<color=#00ff00>+10植物学</color>\r\n<color=#00ff00>+5肾上腺素</color>\r\n<color=#00ff00>+5最大生命值</color>\r\n<color=#ff0000>-3护甲</color>\r\n听听音乐放松一下", "<color=#0000ff>Gramophone</color>\r\n<color=#00ff00>+10 Botany</color>\r\n<color=#00ff00>+5 Adrenaline</color>\r\n<color=#00ff00>+5 MaximumHP</color>\r\n<color=#ff0000>-3 Armor</color>\r\nListen to some music and relax"));
		AddItem(new ConfLocalTextItem(112, "propInfo_WinterMelon", "<color=#9932CD>冰镇西瓜</color>\r\n<color=#00ff00>+25最大生命值</color>\r\n<color=#00ff00>+10肾上腺素</color>\r\n<color=#00ff00>+10范围</color>\r\n<color=#ff0000>-10生命恢复</color>\r\n吃太多会拉肚子", "<color=#9932CD>Iced Watermelon</color>\r\n<color=#00ff00>+25 Max HP</color>\r\n<color=#00ff00>+10 Adrenaline</color>\r\n<color=#00ff00>+10 Range</color>\r\n<color=#ff0000>-10 HP Recovery</color>\r\nEating too much will cause diarrhea"));
		AddItem(new ConfLocalTextItem(113, "propInfo_melon", "<color=#9932CD>西瓜</color>\r\n<color=#00ff00>+20最大生命值</color>\r\n<color=#00ff00>+10生命恢复</color>\r\n<color=#00ff00>+5范围</color>\r\n<color=#ff0000>-5护甲</color>\r\n肚子都要被撑爆了", "<color=#9932CD>Watermelon</color>\r\n<color=#00ff00>+20 Max HP</color>\r\n<color=#00ff00>+10 HP Recovery</color>\r\n<color=#00ff00>+5 Range</color>\r\n<color=#ff0000>-5 Armor</color>\r\nThe belly is about to burst"));
		AddItem(new ConfLocalTextItem(114, "propInfo_Pot_Water", "<color=#9932CD>水花盆</color>\r\n<color=#00ff00>+20植物学</color>\r\n可以在上面种荷叶", "<color=#9932CD>Water Flower Pot</color>\r\n<color=#00ff00>+20 Botany</color>\r\nCan plant lotus leaves on it"));
		AddItem(new ConfLocalTextItem(115, "propInfo_Spinacia", "<color=#9932CD>菠菜</color>\r\n<color=#00ff00>+10力量</color>\r\n<color=#00ff00>+10伤害</color>\r\n<color=#9932CD>解锁击退，并且僵尸尸体飞行造成碰撞伤害(每有一个伤害增加25%,最大100%)</color>\r\n吃了它我能把僵尸直接干飞", "<color=#9932CD>Spinach</color>\r\n<color=#00ff00>+10 Power</color>\r\n<color=#00ff00>+10 PercentageDamage</color>\r\n<color=#9932CD>Unlocks knockback, and zombie corpses fly and cause collision PercentageDamage (each PercentageDamage increases by 25%, up to 100%)</color>\r\nAfter eating it, I can kill zombies directly"));
		AddItem(new ConfLocalTextItem(116, "propInfo_Marigold_petals", "<color=#9932CD>金盏花瓣</color>\r\n<color=#00ff00>+50金币</color>\r\n<color=#00ff00>+5幸运</color>\r\n<color=#00ff00>+5植物学</color>\r\n看看她是这么产生金币的", "<color=#9932CD>Marigold Petals</color>\r\n<color=#00ff00>+50 Gold Coins</color>\r\n<color=#00ff00>+5 Lucky</color>\r\n<color=#00ff00>+5 Botany</color>\r\nSee how she generates gold coins."));
		AddItem(new ConfLocalTextItem(117, "propInfo_TreeFood2", "<color=#9932CD>智慧树肥料</color>\r\n<color=#00ff00>+20植物学</color>\r\n<color=#00ff00>+5幸运</color>\r\n<color=#00ff00>+100阳光</color>\r\n这个好", "<color=#9932CD>Wisdom Tree Fertilizer</color>\r\n<color=#00ff00>+20 Botany</color>\r\n<color=#00ff00>+5 Lucky</color>\r\n<color=#00ff00>+100 Sunshine</color>\r\nThis is good."));
		AddItem(new ConfLocalTextItem(118, "propInfo_basketball", "<color=#9932CD>篮球</color>\r\n<color=#00ff00>+4力量</color>\r\n<color=#00ff00>+10攻击速度</color>\r\n<color=#00ff00>+3暴击率</color>\r\n<color=#00ff00>+2幸运</color>\r\n听说鸽鸽很会玩这个", "<color=#9932CD>Basketball</color>\r\n<color=#00ff00>+4 Power</color>\r\n<color=#00ff00>+10 Attack Speed</color>\r\n<color=#00ff00>+3 Critical Hit Rate</color>\r\n<color=#00ff00>+2 Lucky</color>\r\nI heard that Gege is very good at this"));
		AddItem(new ConfLocalTextItem(119, "propInfo_LawnMower", "<color=#9932CD>小推车</color>\r\n<color=#00ff00>+2力量</color>\r\n<color=#00ff00>+8伤害</color>\r\n<color=#00ff00>+10速度</color>\r\n并且对某直线上的僵尸造成{0}\r\n伤害冷却时间{1}", "<color=#9932CD>Cart</color>\r\n<color=#00ff00>+2 Power</color>\r\n<color=#00ff00>+8 PercentageDamage</color>\r\n<color=#00ff00>+10 Speed</color>\r\nAnd causes {0} to zombies in a straight line\r\nPercentageDamage Cooldown Time {1}"));
		AddItem(new ConfLocalTextItem(120, "propInfo_bucket", "<color=#9932CD>铁桶</color>\r\n<color=#00ff00>+8力量</color>\r\n<color=#00ff00>+6护甲</color>\r\n<color=#ff0000>-10范围</color>\r\n来自铁桶僵尸的战利品，但是会影响到我的视野", "<color=#9932CD>Iron Barrel</color>\r\n<color=#00ff00>+8 Power</color>\r\n<color=#00ff00>+6 Armor</color>\r\n<color=#ff0000>-10 Range</color>\r\nLoot from the Iron Barrel Zombie, but it will affect my vision"));
		AddItem(new ConfLocalTextItem(121, "propInfo_cone", "<color=#9932CD>路障</color>\r\n<color=#00ff00>+6力量</color>\r\n<color=#00ff00>+6护甲</color>\r\n<color=#ff0000>-10范围</color>\r\n来自路障僵尸的战利品，但是会影响到我的视野", "<color=#9932CD>Roadblock</color>\r\n<color=#00ff00>+6 Power</color>\r\n<color=#00ff00>+6 Armor</color>\r\n<color=#ff0000>-10 Range</color>\r\nLoot from the Roadblock zombie, but it will affect my vision"));
		AddItem(new ConfLocalTextItem(122, "propInfo_screendoor", "<color=#9932CD>铁栅栏</color>\r\n<color=#00ff00>+5力量</color>\r\n<color=#00ff00>+15伤害</color>\r\n<color=#00ff00>+5护甲</color>\r\n<color=#ff0000>-8速度</color>\r\n来自铁栅栏僵尸的战利品，会影响到我正常走路", "<color=#9932CD>Iron Fence</color>\r\n<color=#00ff00>+5 Power</color>\r\n<color=#00ff00>+15 PercentageDamage</color>\r\n<color=#00ff00>+5 Armor</color>\r\n<color=#ff0000>-8 Speed</color>\r\nThe loot from the Iron Fence zombie will affect my normal walking"));
		AddItem(new ConfLocalTextItem(123, "propInfo_paper", "<color=#9932CD>报纸</color>\r\n<color=#00ff00>+25植物学</color>\r\n<color=#00ff00>+25金币</color>\r\n<color=#00ff00>+5幸运</color>\r\n<color=#ff0000>-3力量</color>\r\n来自读报僵尸的战利品，里面蕴含的知识可真不少", "<color=#9932CD>Newspaper</color>\r\n<color=#00ff00>+25 Botany</color>\r\n<color=#00ff00>+25 Gold</color>\r\n<color=#00ff00>+5 Lucky</color>\r\n<color=#ff0000>-3 Power</color>\r\nThe spoils from the newspaper-reading zombie. There is a lot of knowledge in it."));
		AddItem(new ConfLocalTextItem(124, "propInfo_ladder", "<color=#9932CD>扶梯</color>\r\n<color=#00ff00>+5力量</color>\r\n<color=#00ff00>+10伤害</color>\r\n<color=#00ff00>+6暴击率</color>\r\n<color=#00ff00>+10范围</color>\r\n<color=#ff0000>-6攻击速度</color>\r\n来自扶梯僵尸的战利品，能攻击到更远的地方了", "<color=#9932CD>Escalator</color>\r\n<color=#00ff00>+5 Power</color>\r\n<color=#00ff00>+10 PercentageDamage</color>\r\n<color=#00ff00>+6 Critical Hit Rate</color>\r\n<color=#00ff00>+10 Range</color>\r\n<color=#ff0000>-6 Attack Speed</color>\r\nThe loot from the escalator zombie can attack farther away."));
		AddItem(new ConfLocalTextItem(125, "propInfo_casque", "<color=#9932CD>橄榄球头盔</color>\r\n<color=#00ff00>+10护甲</color>\r\n<color=#00ff00>+8幸运</color>\r\n<color=#00ff00>+30金币</color>\r\n<color=#ff0000>-20范围</color>\r\n来自橄榄球僵尸的战利品，会影响到我的视野", "<color=#9932CD>Football Helmet</color>\r\n<color=#00ff00>+10 Armor</color>\r\n<color=#00ff00>+8 Lucky</color>\r\n<color=#00ff00>+30 Gold</color>\r\n<color=#ff0000>-20 Range</color>\r\nThe loot from the Football Zombie will affect my vision"));
		AddItem(new ConfLocalTextItem(126, "propInfo_guideboard", "<color=#9932CD>路牌</color>\r\n<color=#00ff00>+6力量</color>\r\n<color=#00ff00>+15范围</color>\r\n<color=#00ff00>+6暴击率</color>\r\n<color=#00ff00>+2护甲</color>\r\n<color=#ff0000>-10速度</color>\r\n来自巨人僵尸的战利品，这么重的玩意儿太影响我的移动速度了", "<color=#9932CD>Street Sign</color>\r\n<color=#00ff00>+6 Power</color>\r\n<color=#00ff00>+15 Range</color>\r\n<color=#00ff00>+6 Critical Hit Rate</color>\r\n<color=#00ff00>+2 Armor</color>\r\n<color=#ff0000>-10 Speed</color>\r\nThe spoils from the giant zombie. This heavy thing affects my movement speed too much."));
		AddItem(new ConfLocalTextItem(127, "propInfo_dirttruck", "<color=#9932CD>推土车</color>\r\n<color=#00ff00>+10植物学</color>\r\n<color=#00ff00>+5力量</color>\r\n<color=#00ff00>+5护甲</color>\r\n<color=#ff0000>-5速度<</color>\r\n能托运泥土，锻炼身体，但是老了推不太动", "<color=#9932CD>Bulldozer</color>\r\n<color=#00ff00>+10 Botany</color>\r\n<color=#00ff00>+5 Power</color>\r\n<color=#00ff00>+5 Armor</color>\r\n<color=#ff0000>-5 Speed<</color>\r\nIt can transport soil and exercise the body, but it is not very mobile when it is old."));
		AddItem(new ConfLocalTextItem(128, "propInfo_icetrap", "<color=#ff0000>冰碎</color>\r\n<color=#00ff00>+32肾上腺素</color>\r\n<color=#00ff00>+5幸运</color>\r\n<color=#00ff00>+60金币</color>\r\n<color=#00ff00>+7护甲</color>\r\n<color=#00ff00>+30速度</color>\r\n<color=#ff0000>-10生命恢复</color>\r\n<color=#ff0000>-10攻击速度</color>\r\n看起来就很冷", "<color=#ff0000>Ice Shatter</color>\r\n<color=#00ff00>+32 Adrenaline</color>\r\n<color=#00ff00>+5 Lucky</color>\r\n<color=#00ff00>+60 Gold Coins</color>\r\n<color=#00ff00>+7 Armor</color>\r\n<color=#00ff00>+30 Speed</color>\r\n<color=#ff0000>-10 LifeRecovery</color>\r\n<color=#ff0000>-10 Attack Speed</color>\r\nIt looks very cold"));
		AddItem(new ConfLocalTextItem(129, "propInfo_fire", "<color=#ff0000>火焰</color>\r\n<color=#00ff00>+10暴击率</color>\r\n<color=#00ff00>+200阳光</color>\r\n<color=#00ff00>+30伤害</color>\r\n<color=#00ff00>+15速度</color>\r\n<color=#00ff00>+15攻击速度</color>\r\n自身每秒<color=#ff0000>{0}生命值</color>\r\n火焰正在燃烧", "<color=#ff0000>Flame</color>\r\n<color=#00ff00>+10 Critical Hit Rate</color>\r\n<color=#00ff00>+200 Sunshine</color>\r\n<color=#00ff00>+30 PercentageDamage</color>\r\n<color=#00ff00>+15 Speed</color>\r\n<color=#00ff00>+15 Attack Speed</color>\r\n<color=#ff0000>{0} Health Points per Second</color>\r\nThe flame is burning"));
		AddItem(new ConfLocalTextItem(130, "propInfo_hammer", "<color=#ff0000>木槌</color>\r\n<color=#00ff00>+10暴击率</color>\r\n<color=#00ff00>+10力量</color>\r\n<color=#00ff00>+10攻击速度</color>\r\n鼠标左键使用木槌造成{0}伤害\r\n冷却时间{1}s\r\n这太哇塞了", "<color=#ff0000>Wooden Mallet</color>\r\n<color=#00ff00>+10 Critical Hit Rate</color>\r\n<color=#00ff00>+10 Power</color>\r\n<color=#00ff00>+10 Attack Speed</color>\r\nUse the left mouse button to use the wooden mallet to cause {0} PercentageDamage\r\nCooling time {1}s\r\nThis is awesome"));
		AddItem(new ConfLocalTextItem(131, "propInfo_PortalCard", "<color=#ff0000>传送门</color>\r\n<color=#00ff00>+20伤害</color>\r\n<color=#00ff00>+100金币</color>\r\n<color=#00ff00>+10幸运</color>\r\n可在场上随机位置生成两个传送门，这很酷\r\n", "<color=#ff0000>Portal</color>\r\n<color=#00ff00>+20 PercentageDamage</color>\r\n<color=#00ff00>+100 Gold</color>\r\n<color=#00ff00>+10 Lucky</color>\r\nYou can generate two portals at random locations on the field, which is cool\r\n"));
		AddItem(new ConfLocalTextItem(132, "propInfo_fogmachine", "<color=#ff0000>雾机</color>\r\n<color=#00ff00>+40最大生命值</color>\r\n<color=#00ff00>+10伤害</color>\r\n<color=#00ff00>+6幸运</color>\r\n<color=#00ff00>+6生命恢复</color>\r\n<color=#ff0000>-10速度</color>\r\n有概率产生雾，免疫远程攻击\r\n", "<color=#ff0000>Fog Machine</color>\r\n<color=#00ff00>+40 MaximumHP</color>\r\n<color=#00ff00>+10 PercentageDamage</color>\r\n<color=#00ff00>+6 Lucky</color>\r\n<color=#00ff00>+6 LifeRecovery</color>\r\n<color=#ff0000>-10 Speed</color>\r\nHas a chance to generate fog, immune to ranged attacks\r\n"));
		AddItem(new ConfLocalTextItem(133, "propInfo_lights", "<color=#ff0000>灯光机</color>\r\n<color=#00ff00>+20最大生命值</color>\r\n<color=#00ff00>+20伤害</color>\r\n<color=#00ff00>+100阳光</color>\r\n<color=#00ff00>+20攻击速度</color>\r\n<color=#00ff00>+5范围</color>\r\n<color=#ff0000>-10暴击率</color>\r\n有灯光才有气氛\r\n", "<color=#ff0000>Lighting Machine</color>\r\n<color=#00ff00>+20 MaximumHP</color>\r\n<color=#00ff00>+20 PercentageDamage</color>\r\n<color=#00ff00>+100 Sunshine</color>\r\n<color=#00ff00>+20 Attack Speed</color>\r\n<color=#00ff00>+5 Range</color>\r\n<color=#ff0000>-10 Critical Hit Rate</color>\r\nLighting creates atmosphere\r\n"));
		AddItem(new ConfLocalTextItem(134, "propInfo_mic", "<color=#ff0000>麦克风</color>\r\n<color=#00ff00>+20肾上腺素</color>\r\n<color=#00ff00>+10暴击率</color>\r\n<color=#00ff00>+100金币</color>\r\n<color=#00ff00>+10伤害</color>\r\n<color=#00ff00>+10速度</color>\r\n<color=#ff0000>-5力量</color>\r\n使我变成大明星\r\n", "<color=#ff0000>Microphone</color>\r\n<color=#00ff00>+20 Adrenaline</color>\r\n<color=#00ff00>+10 Critical Hit Rate</color>\r\n<color=#00ff00>+100 Gold Coins</color>\r\n<color=#00ff00>+10 PercentageDamage</color>\r\n<color=#00ff00>+10 Speed</color>\r\n<color=#ff0000>-5 Power</color>\r\nMake Me a Big Star\r\n"));
		AddItem(new ConfLocalTextItem(135, "propInfo_speaker", "<color=#ff0000>音响</color>\r\n<color=#00ff00>+20范围</color>\r\n<color=#00ff00>+10力量</color>\r\n<color=#00ff00>+5护甲</color>\r\n<color=#00ff00>+20伤害</color>\r\n<color=#00ff00>+20攻击速度</color>\r\n<color=#ff0000>-10速度</color>\r\n使我的声音传的更远\r\n", "<color=#ff0000>Sound</color>\r\n<color=#00ff00>+20 Range</color>\r\n<color=#00ff00>+10 Power</color>\r\n<color=#00ff00>+5 Armor</color>\r\n<color=#00ff00>+20 PercentageDamage</color>\r\n<color=#00ff00>+20 Attack Speed</color>\r\n<color=#ff0000>-10 Speed</color>\r\nMake my voice travel farther\r\n"));
		AddItem(new ConfLocalTextItem(136, "common_continuegame", "继续游戏", "Continue Game"));
		AddItem(new ConfLocalTextItem(137, "common_exit", "退出", "Exit"));
		AddItem(new ConfLocalTextItem(138, "common_exitgame", "退出游戏", "Exit Game"));
		AddItem(new ConfLocalTextItem(139, "common_sure", "确定", "Sure"));
		AddItem(new ConfLocalTextItem(140, "common_cancel", "取消", "Cancel"));
		AddItem(new ConfLocalTextItem(141, "common_restart", "重新开始", "Restart"));
		AddItem(new ConfLocalTextItem(142, "common_describe", "描述", "Describe"));
		AddItem(new ConfLocalTextItem(143, "common_music", "音乐", "Music"));
		AddItem(new ConfLocalTextItem(144, "common_soundeffect", "音效", "Sound"));
		AddItem(new ConfLocalTextItem(145, "common_refresh", "刷新", "Refresh"));
		AddItem(new ConfLocalTextItem(146, "mainmenu_readme", "相关信息", "ReadMe"));
		AddItem(new ConfLocalTextItem(147, "mainmenu_keysettings", "键位设置", "Key settings"));
		AddItem(new ConfLocalTextItem(148, "mainmenu_right", "向右移动", "Move right"));
		AddItem(new ConfLocalTextItem(149, "mainmenu_left", "向左移动", "Move left"));
		AddItem(new ConfLocalTextItem(150, "mainmenu_down", "向下移动", "Move down"));
		AddItem(new ConfLocalTextItem(151, "mainmenu_up", "向上移动", "Move up"));
		AddItem(new ConfLocalTextItem(152, "mainmenu_run", "奔跑", "Run"));
		AddItem(new ConfLocalTextItem(153, "mainmenu_continuetip", "继续游戏还是重新开始\r\n检测到你有存档\r\n你可以选择继续游戏或者重新开始\r\n如果重新开始你的<color=#ff0000>存档将会删除</color>", "Continue the game or restart\r\nDetect that you have a save file\r\nYou can choose to continue the game or restart\r\nIf you restart, your <color=#ff0000>save file will be deleted</color>"));
		AddItem(new ConfLocalTextItem(154, "mainmenu_boss", "僵 王 模 式", "Zombie King"));
		AddItem(new ConfLocalTextItem(155, "mainmenu_start", "开 始 冒 险", "Start Adventure"));
		AddItem(new ConfLocalTextItem(156, "mainmenu_loading", "加载游戏中，请耐心等待。。。", "Loading the game, please be patient and wait..."));
		AddItem(new ConfLocalTextItem(157, "mainmenu_seting", "更 改 设 置", "Change settings"));
		AddItem(new ConfLocalTextItem(158, "mainmenu_fullscreen", "全屏显示", "Full Screen"));
		AddItem(new ConfLocalTextItem(159, "mainmenu_resolution", "分辨率", "Resolution"));
		AddItem(new ConfLocalTextItem(160, "mainmenu_previouskey", "之前的键", "Previous key"));
		AddItem(new ConfLocalTextItem(161, "mainmenu_change", "更改", "Change"));
		AddItem(new ConfLocalTextItem(162, "mainmenu_inputkey", "请输入任意键", "Please enter any key"));
		AddItem(new ConfLocalTextItem(163, "mainmenu_errorinput", "<color=#ff0000>不能有相同的键</color>\r\n输入任意键继续", "<color=#ff0000>Cannot have the same key</color>\r\nEnter any key to continue"));
		AddItem(new ConfLocalTextItem(164, "mainmenu_exitgame", "退 出 游 戏", "Exit Game"));
		AddItem(new ConfLocalTextItem(165, "mainmenu_language", "语言", "Language"));
		AddItem(new ConfLocalTextItem(166, "cannot_buy1", "她很好，我不配，因为我没有钱钱", "She is very good, but I don't deserve her because I don't have money."));
		AddItem(new ConfLocalTextItem(167, "cannot_buy2", "越没钱的时候越想花钱，穷到要吃土l", "The less money you have, the more you want to spend it. You will be so poor that you have to eat dirt."));
		AddItem(new ConfLocalTextItem(168, "cannot_buy3", "购物车里的东西只能看着下架", "The items in the shopping cart can only be taken off the shelves"));
		AddItem(new ConfLocalTextItem(169, "cannot_plant1", "没有花盆植物不能种植", "Plants cannot be grown without pots"));
		AddItem(new ConfLocalTextItem(170, "cannot_plant2", "植物只能在<color=#ff0000>花盆</color>上培养,花盆可以通过<color=#ff0000>刷新</color>进行获取", "Plants can only be grown in <color=#ff0000>flower pots</color>, and flower pots can be obtained by <color=#ff0000>refreshing</color>"));
		AddItem(new ConfLocalTextItem(171, "cannot_plant3", "花盆肥肠重要！！", "Flower pot sausage is important!!"));
		AddItem(new ConfLocalTextItem(172, "cannot_plant4", "荷叶需要种植在<color=#ff0000>水花盆</color>里\n需要先购买<color=#ff0000>水花盆</color>", "Lotus leaves need to be planted in a <color=#ff0000>water flower pot</color>\nYou need to buy a <color=#ff0000>water flower pot</color> first"));
		AddItem(new ConfLocalTextItem(173, "flowerpot_info1", "这是在进货时捡到的花盆\n可以在花园中<color=#00ff00>培养植物</color>", "This is a flower pot I picked up when I was restocking.\nYou can <color=#00ff00>cultivate plants</color> in the garden."));
		AddItem(new ConfLocalTextItem(174, "flowerpot_info2", "花园中已经不能够放下更多的花盆了\n或许可以用<color=#ff0000>铲子</color>铲掉泥巴", "There is no room for any more flower pots in the garden.\nPerhaps you can use a <color=#ff0000>shovel</color> to shovel away the mud"));
		AddItem(new ConfLocalTextItem(175, "property_Property", "属性", "Property"));
		AddItem(new ConfLocalTextItem(176, "property_MaximumHP", "最大生命值", "MaximumHP"));
		AddItem(new ConfLocalTextItem(177, "property_LifeRecovery", "生命恢复", "LifeRecovery"));
		AddItem(new ConfLocalTextItem(178, "property_Adrenaline", "肾上腺素", "Adrenaline"));
		AddItem(new ConfLocalTextItem(179, "property_Power", "力量", "Power"));
		AddItem(new ConfLocalTextItem(180, "property_PercentageDamage", "%伤害", "%PercentageDamage"));
		AddItem(new ConfLocalTextItem(181, "property_AttackSpeed", "%攻击速度", "%AttackSpeed"));
		AddItem(new ConfLocalTextItem(182, "property_Range", "%范围", "%Range"));
		AddItem(new ConfLocalTextItem(183, "property_CriticalHitRate", "%暴击率", "%CriticalHitRate"));
		AddItem(new ConfLocalTextItem(184, "property_MoveSpeed", "%移动速度", "%MoveSpeed"));
		AddItem(new ConfLocalTextItem(185, "property_Armor", "护甲", "Armor"));
		AddItem(new ConfLocalTextItem(186, "property_Lucky", "幸运", "Lucky"));
		AddItem(new ConfLocalTextItem(187, "property_Sunshine", "阳光", "Sunshine"));
		AddItem(new ConfLocalTextItem(188, "property_GoldCoins", "金币", "GoldCoins"));
		AddItem(new ConfLocalTextItem(189, "property_Botany", "植物学", "Botany"));
		AddItem(new ConfLocalTextItem(190, "garden_wave", "当前为第<color=#ffff00>{0}</color>波", "The current wave is <color=#ffff00>{0}</color>"));
		AddItem(new ConfLocalTextItem(191, "garden_nourish", "培养", "Nourish"));
		AddItem(new ConfLocalTextItem(192, "garden_evolve", "进化", "Evolve"));
		AddItem(new ConfLocalTextItem(193, "garden_tobattle", "出战", "Go to battle"));
		AddItem(new ConfLocalTextItem(194, "garden_eat", "吃下", "Eat"));
		AddItem(new ConfLocalTextItem(195, "garden_nobattle", "取消出战", "Cancel the battle"));
		AddItem(new ConfLocalTextItem(196, "battle_vocalconcert", "音乐会套装每秒对范围<color=#ff0000>{0}</color>造成<color=#ff0000>{1}</color>伤害", "The Concert Suit deals <color=#ff0000>{1}</color> damage to the area <color=#ff0000>{0}</color> every second."));
		AddItem(new ConfLocalTextItem(197, "pause_hud", "开启伤害显示", "Enable damage display"));
		AddItem(new ConfLocalTextItem(198, "pause_mainmenu", "主菜单", "Main Menu"));
		AddItem(new ConfLocalTextItem(199, "pause_return", "返回游戏", "Return Game"));
		AddItem(new ConfLocalTextItem(200, "pause_restart", "重新开始\r\n你想要重新开始游戏吗？\r\n你的存档将会删除", "Restart\r\nDo you want to restart the game? \r\nYour save data will be deleted"));
		AddItem(new ConfLocalTextItem(201, "pause_exit", "离开游戏？\r\n想要回到主菜单吗？\r\n你下一次开始游戏将从上一波商店页面开始", "Leave the game? \r\nWant to return to the main menu? \r\nThe next time you start the game, you will start from the last wave of store pages"));
		AddItem(new ConfLocalTextItem(202, "pause_exit2", "离开", "Exit"));
		AddItem(new ConfLocalTextItem(203, "victory_1", "恭喜你击败了\r\n僵王博士\r\n取得了最终的胜利", "Congratulations on defeating Dr. Zombie King and achieving the final victory."));
		AddItem(new ConfLocalTextItem(204, "property_MaximumHP_info", "最大生命值\n每波开始时生命值将为{0}</color>\n生命值变为0脑子会被僵尸吃掉", "Max HP\nHP will be {0} </color>at the beginning of each wave\nWhen HP reaches 0, the brain will be eaten by zombies"));
		AddItem(new ConfLocalTextItem(205, "property_LifeRecovery_info", "生命恢复\n每<color=#ff0000>10s</color>会恢复{0}</color>点生命值\n恢复能力高于受到的伤害时我将无敌", "HP Recovery\nHP will be restored to {0}</color> points every <color=#ff0000>10s</color>\nWhen the recovery ability is higher than the damage taken, I will be invincible"));
		AddItem(new ConfLocalTextItem(206, "property_Adrenaline_info", "肾上腺素\n每次攻击都让我热血澎湃\n每次攻击有{0}%概率恢复{1}点生命值</color>", "Adrenaline\nEvery attack makes my blood boil\nEach attack has a {0}% chance to recover + recoveryHp + HP points</color>"));
		AddItem(new ConfLocalTextItem(207, "property_Power_info", "力量\n平底锅造成的基础伤害增加{0}</color>\n击退的力量增加<color=#00ff00>{1}%</color>", "Power\nThe basic damage caused by the pan is increased by {0}</color>\nThe knockback Power is increased by <color=#00ff00> + value + %</color>"));
		AddItem(new ConfLocalTextItem(208, "property_PercentageDamage_info", "伤害\n平底锅造成的基础伤害增加{0}%</color>\n某些道具的伤害也会随百分比伤害增加", "Damage\nThe basic damage caused by the pan increases by {0}%</color>\nThe damage of some items will also increase with the percentage damage"));
		AddItem(new ConfLocalTextItem(209, "property_AttackSpeed_info", "攻击速度\n平底锅移动的速度增加{0}%</color>\n某些道具的也会受到攻击速度的影响，如木槌", "Attack speed\nThe speed of the pan's movement increases by {0}%</color>\nSome items are also affected by the attack speed, such as the mallet"));
		AddItem(new ConfLocalTextItem(210, "property_Range_info", "攻击范围\n眼睛更明亮，攻击范围和拾取范围增加{0}%</color>\n某些道具的也会受到攻击范围的影响，如演唱会套装", "Attack range\nThe eyes are brighter, and the attack range and pick range increases by {0}%</color>\nSome items are also affected by the attack range, such as the concert suit"));
		AddItem(new ConfLocalTextItem(211, "property_CriticalHitRate_info", "暴击率\n攻击有{0}%</color>造成1.5倍伤害", "Critical hit rate\nAttacks have {0}%</color> to cause 1.5 times the damage"));
		AddItem(new ConfLocalTextItem(212, "property_MoveSpeed_info", "移动速度\n基础移速增加{0}%</color>\n某些道具的也会受到攻击范围的影响，如小推车\n<color=#00ff00>跑步的速度为基础速度的1.5倍</color>", "Movement speed\nThe basic movement speed increases by {0}%</color>\nSome items are also affected by the attack range, such as the cart\n <color=#00ff00>Running speed is 1.5 times the base speed</color>"));
		AddItem(new ConfLocalTextItem(213, "property_Armor_info", "护甲|<color=#ff0000>上限90%</color>\n受到的伤害减少{0}%</color>\n能有效阻挡僵尸吃掉脑子", "Armor|<color=#ff0000>Upper limit 90%</color>\nReduces damage received by {0}%</color>\nCan effectively prevent zombies from eating brains"));
		AddItem(new ConfLocalTextItem(214, "property_Lucky_info", "幸运\n能够增加钱币掉落的概率及质量\n能够增加阳光掉落最大个数\n能够增大高级物品刷新概率\n这非常重要", "Lucky\nCan increase the probability and quality of coin drops\nCan increase the maximum number of sunlight drops\nCan increase the probability of high-level item refreshes\nThis is very important"));
		AddItem(new ConfLocalTextItem(215, "property_Sunshine_info", "阳光\n每波结束可以免费获得{0}</color>阳光", "Sunlight\nYou can get {0}</color>Sunlight for free at the end of each wave"));
		AddItem(new ConfLocalTextItem(216, "property_GoldCoins_info", "金币\n每波结束可以免费获得{0}</color>金币", "Gold\nYou can get {0}</color>Gold for free at the end of each wave"));
		AddItem(new ConfLocalTextItem(217, "property_Botany_info", "植物学\n可以使植物造成的伤害增加{0}%</color>", "Botany\nCan increase the damage caused by plants by {0}%</color>"));
		AddItem(new ConfLocalTextItem(218, "cultivation_plant1", " 培育", "Cultivation"));
		AddItem(new ConfLocalTextItem(219, "cultivation_plant2", " 基础伤害", "Basic damage"));
		AddItem(new ConfLocalTextItem(220, "cultivation_plant3", " 百分比伤害", "Percentage damage"));
		AddItem(new ConfLocalTextItem(221, "cultivation_plant4", " 攻击检测范围", "Attack detection range"));
		AddItem(new ConfLocalTextItem(222, "cultivation_plant5", " 攻击冷却", "Attack cooldown"));
		AddItem(new ConfLocalTextItem(223, "cultivation_plant6", " 子弹速度", "Bullet speed"));
		AddItem(new ConfLocalTextItem(224, "cultivation_plant7", " 溅射伤害", "Splash damage"));
		AddItem(new ConfLocalTextItem(225, "cultivation_plant8", " 消化速度", "Digestion speed"));
		AddItem(new ConfLocalTextItem(226, "cultivation_plant9", " 一次性吞噬个数", "Number of swallowed at one time"));
		AddItem(new ConfLocalTextItem(227, "cultivation_plant10", " 金币率转换", "Coin rate conversion"));
		AddItem(new ConfLocalTextItem(228, "cultivation_plant11", " 阳光率转换", "Sun rate conversion"));
		AddItem(new ConfLocalTextItem(229, "cultivation_plant12", " 穿透数量", "Number of penetrations"));
		AddItem(new ConfLocalTextItem(230, "cultivation_plant13", " 暴击伤害", "Critical damage"));
		AddItem(new ConfLocalTextItem(231, "cultivation_plant14", " 玩家攻击速度", "Player attack speed"));
		AddItem(new ConfLocalTextItem(232, "cultivation_plant15", " 风速", "Wind speed"));
		AddItem(new ConfLocalTextItem(233, "cultivation_plant16", " 逆风恢复", "Recovery against wind"));
		AddItem(new ConfLocalTextItem(234, "cultivation_plant17", " 僵尸风阻", "Zombie wind resistance"));
		AddItem(new ConfLocalTextItem(235, "cultivation_plant18", " 爆炸范围", "Explosion range"));
		AddItem(new ConfLocalTextItem(236, "cultivation_plant19", " 普通僵尸即死率", "Common zombie instant death rate"));
		AddItem(new ConfLocalTextItem(237, "cultivation_plant20", " 大型僵尸增伤", "Increased damage for large zombies"));
		AddItem(new ConfLocalTextItem(238, "cultivation_plant21", " 阳光减少", "Reduced sunlight"));
		AddItem(new ConfLocalTextItem(239, "cultivation_plant22", " 黄油概率", "Butter probability"));
		AddItem(new ConfLocalTextItem(240, "cultivation_plant23", " 控制时间", "Control time"));
		AddItem(new ConfLocalTextItem(241, "cultivation_plant24", " 暴击率", "Critical rate"));
		AddItem(new ConfLocalTextItem(242, "cultivation_plant25", " 伤害段数*2概率", "Damage segment number * 2 probability"));
		AddItem(new ConfLocalTextItem(243, "cultivation_plant26", " 吸取金币个数", "Number of gold coins absorbed"));
		AddItem(new ConfLocalTextItem(244, "cultivation_plant27", " 吸取持续时间", "Duration of absorption"));
		AddItem(new ConfLocalTextItem(245, "cultivation_plant28", " 吞噬墓碑概率", "Probability of swallowing tombstones"));
		AddItem(new ConfLocalTextItem(246, "cultivation_plant29", " 魅惑者可攻击次数", "Number of attacks that the charmer can make"));
		AddItem(new ConfLocalTextItem(247, "cultivation_plant30", " 连续触发概率", "Continuous trigger probability"));
		AddItem(new ConfLocalTextItem(248, "cultivation_plant31", " 换取金币", "Exchange for gold coins"));
		AddItem(new ConfLocalTextItem(249, "cultivation_plant32", " 吸取铁制品个数", "Number of iron products absorbed"));
		AddItem(new ConfLocalTextItem(250, "cultivation_plant33", " 掉落金币概率", "Probability of dropping gold coins"));
		AddItem(new ConfLocalTextItem(251, "cultivation_plant34", " 掉落钻石概率", "Probability of dropping diamonds"));
		AddItem(new ConfLocalTextItem(252, "cultivation_plant35", " 掉落双倍概率", "Probability of dropping double drops"));
		AddItem(new ConfLocalTextItem(253, "cultivation_plant36", " 范围攻速提升", "Range attack speed increase"));
		AddItem(new ConfLocalTextItem(254, "cultivation_plant37", " 范围生命恢复", "Range health recovery"));
		AddItem(new ConfLocalTextItem(255, "cultivation_plant38", " 范围伤害增加", "Range damage increase"));
		AddItem(new ConfLocalTextItem(256, "cultivation_plant39", " 子弹概率变多", "Bullet probability becomes more"));
		AddItem(new ConfLocalTextItem(257, "cultivation_plant40", " 子弹大小", "Bullet size"));
		AddItem(new ConfLocalTextItem(258, "cultivation_plant41", " 触发后生命值恢复比例", "Health value recovery ratio after triggering"));
		AddItem(new ConfLocalTextItem(259, "cultivation_plant42", " 攻击掉落南瓜概率", "Probability of dropping pumpkins when attacking"));
		AddItem(new ConfLocalTextItem(260, "cultivation_plant43", " 减速百分比", "Slowdown percentage"));
		AddItem(new ConfLocalTextItem(261, "cultivation_plant44", " 减速时间", "Slowdown time"));
		AddItem(new ConfLocalTextItem(262, "cultivation_plant45", " 破坏载具数量", "Number of vehicles destroyed"));
		AddItem(new ConfLocalTextItem(263, "cultivation_plant46", " 生成阳光质量", "Quality of sunlight generated"));
		AddItem(new ConfLocalTextItem(264, "cultivation_plant47", " 高坚果生命值（植物学）", "High Nut Health (Botany)"));
		AddItem(new ConfLocalTextItem(265, "cultivation_plant48", " 最终爆炸概率", "Final Explosion Chance"));
		AddItem(new ConfLocalTextItem(266, "cultivation_plant49", " 反伤伤害", "Retaliate Damage"));
		AddItem(new ConfLocalTextItem(267, "cultivation_plant50", " 反伤概率", "Retaliate Chance"));
		AddItem(new ConfLocalTextItem(268, "cultivation_plant51", " 豌豆增伤", "Pea Damage"));
		AddItem(new ConfLocalTextItem(269, "cultivation_plant52", " 豌豆溅射伤害", "Pea Splash Damage"));
		AddItem(new ConfLocalTextItem(270, "cultivation_plant53", " 豌豆速度", "Pea Speed"));
		AddItem(new ConfLocalTextItem(271, "cultivation_plant54", " 滚动速度", "Rolling Speed"));
		AddItem(new ConfLocalTextItem(272, "cultivation_plant55", " 爆炸坚果概率", "Exploding Nut Chance"));
		AddItem(new ConfLocalTextItem(273, "cultivation_plant56", " 冰冻时间", "Freeze Time"));
		AddItem(new ConfLocalTextItem(274, "cultivation_plant57", " 冰冻期间攻速增加", "Attack Speed ​​Increase During Freeze"));
		AddItem(new ConfLocalTextItem(275, "cultivation_plant58", " 出土时间", "Unearth Time"));
		AddItem(new ConfLocalTextItem(276, "cultivation_plant59", " 连坐概率", "Linked Nut Chance"));
		AddItem(new ConfLocalTextItem(277, "cultivation_plant60", " 玩家对僵尸增伤", "Player Damage Increased on Zombies"));
		AddItem(new ConfLocalTextItem(278, "cultivation_plant61", " 肾上腺素", "Adrenaline"));
		AddItem(new ConfLocalTextItem(279, "cultivation_plant62", " 生命恢复", "Health Regeneration"));
		AddItem(new ConfLocalTextItem(280, "cultivation_plant63", " 幸运", "Lucky"));
		AddItem(new ConfLocalTextItem(281, "cultivation_plant64", " 植物学", "Botany"));
		AddItem(new ConfLocalTextItem(282, "cultivation_plant65", " 范围", "Range"));
		AddItem(new ConfLocalTextItem(283, "cultivation_plant66", " 伤害", "Damage"));
		AddItem(new ConfLocalTextItem(284, "cultivation_plant67", " 攻击速度", "Attack Speed"));
		AddItem(new ConfLocalTextItem(285, "cultivation_plant68", " 移动速度", "Move Speed"));
		AddItem(new ConfLocalTextItem(286, "cultivation_plant69", " 力量", "Power"));
		AddItem(new ConfLocalTextItem(287, "cultivation_plant70", " 最大生命值", "Max Health"));
		AddItem(new ConfLocalTextItem(288, "cultivation_plant71", " 金币", "Gold"));
		AddItem(new ConfLocalTextItem(289, "cultivation_plant72", " 护甲", "Armor"));
		AddItem(new ConfLocalTextItem(290, "cultivation_plant73", " 阳光", "Sunlight"));
		AddItem(new ConfLocalTextItem(291, "mainmenu_grow", "祭坛", "altar"));
		AddItem(new ConfLocalTextItem(292, "mainmenu_reduction", "还原", "Reduction"));
		AddItem(new ConfLocalTextItem(293, "mainmenu_growTitle", "强 化 目 录", "Strengthen the directory"));
		AddItem(new ConfLocalTextItem(294, "mainmenu_strengthen", "强化", "Strengthen"));
		AddItem(new ConfLocalTextItem(295, "mainmenu_levelmax", "已满级", "Full level"));
		AddItem(new ConfLocalTextItem(296, "mainmenu_grow1", "强化增加{0}最大生命值，当前等级{1},总共增加生命值{2}。", "Enhance increases maximum health by {0}, current level {1}, total health increase by {2}."));
		AddItem(new ConfLocalTextItem(297, "mainmenu_grow2", "强化增加{0}生命恢复，当前等级{1},总共增加生命恢复{2}。生命恢复会随时间增加当前生命值", "Enhance increases health regeneration by {0}, current level {1}, total health regeneration by {2}. Health regeneration increases current health over time"));
		AddItem(new ConfLocalTextItem(298, "mainmenu_grow3", "强化增加{0}肾上腺素，当前等级{1},总共增加肾上腺素{2}。肾上腺素使攻击有概率恢复生命值", "Enhance increases adrenaline by {0}, current level {1}, total adrenaline increase by {2}. Adrenaline gives attacks a chance to restore health"));
		AddItem(new ConfLocalTextItem(299, "mainmenu_grow4", "强化增加{0}力量，当前等级{1},总共增加力量{2}。增加攻击的基础伤害", "Enhance increases strength by {0}, current level {1}, total strength increase by {2}. Increases base damage of attacks"));
		AddItem(new ConfLocalTextItem(300, "mainmenu_grow5", "强化增加{0}%伤害，当前等级{1},总共增加伤害{2}。增加攻击百分比伤害", "Enhance increases damage by {0}%, current level {1}, total damage increase by {2}. Increases percentage damage of attacks"));
		AddItem(new ConfLocalTextItem(301, "mainmenu_grow6", "强化增加{0}%攻击速度，当前等级{1},总共增加攻击速度{2}。减少攻击冷却", "Enhance increases attack speed by {0}%, current level {1}, total attack speed increase by {2}. Reduces attack cooldown"));
		AddItem(new ConfLocalTextItem(302, "mainmenu_grow7", "强化增加{0}%范围，当前等级{1},总共增加范围{2}。增加攻击范围和拾取范围", "Enhance increases range by {0}%, current level {1}, total range increase by {2}. Increases attack range and pickup range"));
		AddItem(new ConfLocalTextItem(303, "mainmenu_grow8", "强化增加{0}%暴击率，当前等级{1},总共增加暴击率{2}。攻击有概率造成更多伤害", "Enhance increases critical strike rate by {0}%, current level {1}, total critical strike rate increase by {2}. Attacks have a chance to cause more damage"));
		AddItem(new ConfLocalTextItem(304, "mainmenu_grow9", "强化增加{0}%移动速度，当前等级{1},总共增加移动速度{2}。移动会更快", "Enhance increases movement speed by {0}%, current level {1}, total increase in movement speed {2}. Move faster"));
		AddItem(new ConfLocalTextItem(305, "mainmenu_grow10", "强化增加{0}护甲，当前等级{1},总共增加护甲{2}。减少收到的伤害", "Enhance increases {0} armor, current level {1}, total increase in armor {2}. Reduce damage received"));
		AddItem(new ConfLocalTextItem(306, "mainmenu_grow11", "强化增加{0}幸运，当前等级{1},总共增加幸运{2}。增加钱币，阳光掉落概率，稀有物品刷新概率", "Enhance increases {0} luck, current level {1}, total increase in luck {2}. Increases coin, sunlight drop probability, rare item refresh probability"));
		AddItem(new ConfLocalTextItem(307, "mainmenu_grow12", "强化增加{0}阳光，当前等级{1},总共增加阳光{2}。波次结束增加阳光", "Enhance increases {0} sunlight, current level {1}, total increase in sunlight {2}. Increase sunlight at the end of the wave"));
		AddItem(new ConfLocalTextItem(308, "mainmenu_grow13", "强化增加{0}金币，当前等级{1},总共增加金币{2}。波次结束增加金币", "Enhance increases {0} gold coins, current level {1}, total increase in gold coins {2}. Increase gold coins at the end of the wave"));
		AddItem(new ConfLocalTextItem(309, "mainmenu_grow14", "强化增加{0}植物学，当前等级{1},总共增加植物学{2}。额外增加植物攻击百分比伤害", "Enhance increases {0} botany, current level {1}, total increase in botany {2}. Additional percentage damage of plant attacks"));
		AddItem(new ConfLocalTextItem(310, "grow_slotNum", "增加卡槽", "Add card slot"));
		AddItem(new ConfLocalTextItem(311, "mainmenu_grow15", "增加{0}个卡槽，当前等级{1}，总共增加卡槽数量{2}。卡槽数量决定植物出战数量", "Add {0} card slots, current level {1}, total card slots added {2}. The number of card slots determines the number of plants that can be used in battle"));
		AddItem(new ConfLocalTextItem(312, "mainmenu_grow16_0", "强化后可携带一个普通品质道具开局", "After strengthening, you can start with a normal quality item"));
		AddItem(new ConfLocalTextItem(313, "mainmenu_grow16_1", "强化后可携带一个蓝色品质道具开局，当前可携带普通品质", "After strengthening, you can start with a blue quality item, and currently you can carry a normal quality item"));
		AddItem(new ConfLocalTextItem(314, "mainmenu_grow16_2", "强化后可携带一个紫色品质道具开局，当前可携带蓝色品质", "After strengthening, you can start with a purple quality item, and currently you can carry a blue quality item"));
		AddItem(new ConfLocalTextItem(315, "mainmenu_grow16_3", "可携带一个紫色品质道具开局", "You can start with a purple quality item"));
		AddItem(new ConfLocalTextItem(316, "grow_addProp", "遗产", "Heritage"));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfLocalTextItem GetItem(int id)
	{
		return GetItemObject<ConfLocalTextItem>(id);
	}
	
}
	
