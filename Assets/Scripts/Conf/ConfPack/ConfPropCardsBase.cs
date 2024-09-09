using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfPropCardsItem : ConfBaseItem
{
	/// <summary>
	/// 唯一标识
	/// </summary>
	public string propName;

	/// <summary>
	/// 商店道具图标
	/// </summary>
	public string propImagePath;

	/// <summary>
	/// 默认价格
	/// </summary>
	public int defaultPrice;

	/// <summary>
	/// 描述
	/// </summary>
	public string info;

	/// <summary>
	/// 品质
	/// </summary>
	public int quality;

	/// <summary>
	/// 伤害类型
	/// </summary>
	public int propDamageType;

	/// <summary>
	/// 默认伤害
	/// </summary>
	public int defalutDamage;

	/// <summary>
	/// 冷却时间
	/// </summary>
	public int coolingTime;

	/// <summary>
	/// 获得道具对属性的增改，之前设计json更改，修改起来比较麻烦就不改了
	/// </summary>
	public string attributes;


	public ConfPropCardsItem()
	{
	}

	public ConfPropCardsItem(int id, string propName, string propImagePath, int defaultPrice, string info, int quality, int propDamageType, int defalutDamage, int coolingTime, string attributes)
	{
		this.id = id;
		this.propName = propName;
		this.propImagePath = propImagePath;
		this.defaultPrice = defaultPrice;
		this.info = info;
		this.quality = quality;
		this.propDamageType = propDamageType;
		this.defalutDamage = defalutDamage;
		this.coolingTime = coolingTime;
		this.attributes = attributes;
	}	

	public ConfPropCardsItem Clone()
	{
		ConfPropCardsItem item = (ConfPropCardsItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfPropCardsBase : ConfBase<ConfPropCardsItem>
{
    public override void Init()
    {
		confName = "PropCards";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfPropCardsItem(1, "jaw", "Shop/Props/jaw", 29, "一个下巴？\r\n<color=#00ff00>+2肾上腺素</color>\r\n<color=#00ff00>+2生命恢复</color>\r\n<color=#00ff00>+5攻击速度</color>\r\n<color=#ff0000>-2最大生命值</color>\r\n看起来令人毛骨悚然", 1, 0, 0, 0, "[{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"2\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"2\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"5\"},{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(2, "sadheart", "Shop/Props/sadheart", 21, "正在哭泣的红心\r\n<color=#00ff00>+5最大生命值</color>\r\n<color=#ff0000>-3肾上腺素</color>\r\n她看起来好可怜", 1, 0, 0, 0, "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"5\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(3, "magnetic", "Shop/Props/magnetic", 35, "牵引金币的磁铁\r\n<color=#ff0000>-3速度</color>\r\n有了它我就不需要一个一个点击收集<color=#ff00ff>金币</color>了\r\n但是相互作用力之下，我的速度也收到了影响", 1, 0, 0, 0, "[{\"attributeTypeString\":\"Speed\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(4, "blackhole", "Shop/Props/blackhole", 35, "吸收阳光的黑洞\r\n<color=#ff0000>-4植物学</color>\r\n有了它我就不需要一个一个点击收集<color=#ff00ff>阳光</color>了\r\n植物好像非常害怕它", 1, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"-4\"}]"));
		AddItem(new ConfPropCardsItem(5, "plate", "Shop/Props/plate", 27, "金属盘子\r\n<color=#00ff00>+2最大生命值</color>\r\n<color=#00ff00>+10金币</color>\r\n<color=#00ff00>+1护甲</color>\r\n<color=#ff0000>-5肾上腺素</color>\r\n可以用来干饭", 1, 0, 0, 0, "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"2\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"10\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"1\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(6, "throat", "Shop/Props/throat", 20, "长牙齿的咽喉？\r\n<color=#00ff00>+5最大生命值</color>\r\n<color=#00ff00>+5肾上腺素</color>\r\n<color=#ff0000>-5生命恢复</color>\r\n看起来更像一条蛇", 1, 0, 0, 0, "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"5\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"5\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(7, "SeaShroom_head", "Shop/Props/SeaShroom_head", 18, "小海菇的帽子\r\n<color=#00ff00>+2幸运</color>\r\n<color=#00ff00>+3植物学</color>\r\n<color=#00ff00>+5肾上腺素</color>\r\n<color=#ff0000>-3最大生命值</color>\r\n可以好好研究一下", 1, 0, 0, 0, "[{\"attributeTypeString\":\"Lucky\",\"increment\":\"2\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"3\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"5\"},{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(8, "SunShroom_head", "Shop/Props/SunShroom_head", 25, "阳光菇帽子\r\n<color=#00ff00>+50阳光</color>\r\n<color=#00ff00>+3植物学</color>\r\n<color=#00ff00>+4最大生命值</color>\r\n<color=#ff0000>-2力量</color>\r\n可以好好研究一下", 1, 0, 0, 0, "[{\"attributeTypeString\":\"Sunshine\",\"increment\":\"50\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"3\"},{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"4\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(9, "beard", "Shop/Props/beard", 18, "戴夫的胡须\r\n<color=#00ff00>+2护甲</color>\r\n<color=#00ff00>+1幸运</color>\r\n<color=#ff0000>-3肾上腺素</color>\r\n这是我的胡子", 1, 0, 0, 0, "[{\"attributeTypeString\":\"Armor\",\"increment\":\"2\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"1\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(10, "glasses", "Shop/Props/glasses", 25, "眼镜\r\n<color=#00ff00>+5范围</color>\r\n<color=#00ff00>+1暴击率</color>\r\n戴上后视野会变得清晰", 1, 0, 0, 0, "[{\"attributeTypeString\":\"Range\",\"increment\":\"5\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"1\"}]"));
		AddItem(new ConfPropCardsItem(11, "leaf", "Shop/Props/leaf", 20, "叶子\r\n<color=#00ff00>+3植物学</color>\r\n<color=#00ff00>+1幸运</color>\r\n不知道属于哪个植物", 1, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"3\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"1\"}]"));
		AddItem(new ConfPropCardsItem(12, "smallflower", "Shop/Props/smallflower", 24, "小花\r\n<color=#00ff00>+4最大生命值</color>\r\n看起来很眼熟", 1, 0, 0, 0, "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"4\"}]"));
		AddItem(new ConfPropCardsItem(13, "fertilizer", "Shop/Props/fertilizer", 16, "肥料\r\n<color=#00ff00>+4植物学</color>\r\n应该能帮到植物们", 1, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"4\"}]"));
		AddItem(new ConfPropCardsItem(14, "key", "Shop/Props/key", 25, "车钥匙\r\n<color=#9932CD>可以免费刷新一次商品</color>", 1, 0, 0, 0, ""));
		AddItem(new ConfPropCardsItem(15, "wallet", "Shop/Props/wallet", 20, "钱袋\r\n<color=#00ff00>+10金币</color>\r\n里面能装不少钱", 1, 0, 0, 0, "[{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(16, "Diamond", "Shop/Props/Diamond", 66, "<color=#0000ff>钻石</color>\r\n<color=#00ff00>+30金币</color>\r\n<color=#00ff00>+4幸运</color>\r\n亮闪亮闪的很漂亮", 2, 0, 0, 0, "[{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"30\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"4\"}]"));
		AddItem(new ConfPropCardsItem(17, "SunFlower", "Shop/Props/SunFlower", 54, "<color=#0000ff>葵花</color>\r\n<color=#00ff00>+5植物学</color>\r\n<color=#00ff00>+3幸运</color>\r\n<color=#00ff00>+50阳光</color>\r\n向日葵的花瓣，不知道有没有葵花籽", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"5\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"3\"},{\"attributeTypeString\":\"Sunshine\",\"increment\":\"50\"}]"));
		AddItem(new ConfPropCardsItem(18, "basket", "Shop/Props/basket", 55, "<color=#0000ff>篮子</color>\r\n<color=#00ff00>+10范围</color>\r\n<color=#00ff00>+3暴击率</color>\r\n<color=#00ff00>+5植物学</color>\r\n<color=#ff0000>-3攻击速度</color>\r\n好像是某个投手的投射器", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Range\",\"increment\":\"10\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"3\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"5\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(19, "rake", "Shop/Props/rake", 80, "<color=#0000ff>耙子</color>\r\n<color=#00ff00>+5力量</color>\r\n<color=#00ff00>+3暴击率</color>\r\n这件兵器很趁手", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Power\",\"increment\":\"5\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"3\"}]"));
		AddItem(new ConfPropCardsItem(20, "wateringcan", "Shop/Props/wateringcan", 51, "<color=#0000ff>水壶</color>\r\n<color=#00ff00>+4植物学</color>\r\n<color=#00ff00>+4护甲</color>\r\n<color=#00ff00>+5生命恢复</color>\r\n<color=#ff0000>-4攻击速度</color>\r\n对植物们很有帮助", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"4\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"4\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"5\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"-4\"}]"));
		AddItem(new ConfPropCardsItem(21, "jackbox", "Shop/Props/jackbox", 58, "<color=#0000ff>小丑礼盒</color>\r\n<color=#00ff00>+5幸运</color>\r\n<color=#00ff00>+5肾上腺素</color>\r\n<color=#00ff00>+5速度</color>\r\n<color=#ff0000>-2最大生命值</color>\r\n一个会爆炸的礼盒，我得小心收藏", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Lucky\",\"increment\":\"5\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"5\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"5\"},{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(22, "splash3", "Shop/Props/splash3", 85, "<color=#0000ff>一朵水花？</color>\r\n<color=#00ff00>+10最大生命值</color>\r\n<color=#00ff00>+4生命恢复</color>\r\n<color=#00ff00>+10攻击速度</color>\r\n<color=#ff0000>-5肾上腺素</color>\r\n<color=#ff0000>-2护甲</color>\r\n哪个跳水队都比不过这一朵", 2, 0, 0, 0, "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"10\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"4\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"10\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-5\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(23, "SodRollCap", "Shop/Props/SodRollCap", 80, "<color=#0000ff>一卷草皮？</color>\r\n<color=#00ff00>+6生命恢复</color>\r\n<color=#00ff00>+10速度</color>\r\n<color=#00ff00>+8攻击速度</color>\r\n<color=#ff0000>-2力量</color>\r\n铺开躺上去一定很舒服", 2, 0, 0, 0, "[{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"6\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"8\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(24, "Spikerock", "Shop/Props/Spikerock", 70, "<color=#0000ff>一根地刺</color>\r\n<color=#00ff00>+10速度</color>\r\n<color=#00ff00>+10攻击速度</color>\r\n<color=#00ff00>+2生命恢复</color>\r\n<color=#ff0000>-10肾上腺素</color>\r\n都不想踩在上面", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Speed\",\"increment\":\"10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"10\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"2\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(25, "Stinky_turn1", "Shop/Props/Stinky_turn1", 66, "<color=#0000ff>蜗牛</color>\r\n<color=#00ff00>+8护甲</color>\r\n<color=#00ff00>+5植物学</color>\r\n<color=#ff0000>-5速度</color>\r\n<color=#ff0000>-3肾上腺素</color>\r\n应该对植物有所帮助，跑得太慢了", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Armor\",\"increment\":\"8\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"5\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-5\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(26, "bugspray", "Shop/Props/bugspray", 70, "<color=#0000ff>杀虫剂</color>\r\n<color=#00ff00>+10植物学</color>\r\n<color=#00ff00>+20肾上腺素</color>\r\n<color=#ff0000>-6生命恢复</color>\r\n气味很难闻，可以帮助植物对抗虫子", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"10\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"20\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"-6\"}]"));
		AddItem(new ConfPropCardsItem(27, "shovel", "Shop/Props/shovel", 52, "<color=#0000ff>铲子</color>\r\n<color=#00ff00>+10植物学</color>\r\n可以铲掉花园中的泥土，放置花盆", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(28, "cardSlot", "Shop/Props/cardSlot", 55, "<color=#0000ff>卡槽</color>\r\n<color=#00ff00>+10植物学</color>\r\n可以增加出战植物的上限", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(29, "book", "Shop/Props/book", 50, "<color=#0000ff>书</color>\r\n<color=#00ff00>+15植物学</color>\r\n可以学到许多知识", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"15\"}]"));
		AddItem(new ConfPropCardsItem(30, "cup", "Shop/Props/cup", 54, "<color=#0000ff>奖杯</color>\r\n<color=#00ff00>+20金币</color>\r\n<color=#00ff00>+3幸运</color>\r\n荣誉的象征", 2, 0, 0, 0, "[{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"20\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"3\"}]"));
		AddItem(new ConfPropCardsItem(31, "sandwich", "Shop/Props/sandwich", 65, "<color=#0000ff>三明治</color>\r\n<color=#00ff00>+10最大生命值</color>\r\n<color=#00ff00>+5生命恢复</color>\r\n看起来很好吃", 2, 0, 0, 0, "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"10\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"5\"}]"));
		AddItem(new ConfPropCardsItem(32, "lock", "Shop/Props/lock", 45, "<color=#0000ff>一把锁</color>\r\n<color=#00ff00>+5伤害</color>\r\n<color=#00ff00>+5暴击</color>\r\n<color=#ff0000>-3攻击速度</color>\r\n可以对准", 2, 0, 0, 0, "[{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"5\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"5\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(33, "radio", "Shop/Props/radio", 50, "<color=#0000ff>留声机</color>\r\n<color=#00ff00>+10植物学</color>\r\n<color=#00ff00>+5肾上腺素</color>\r\n<color=#00ff00>+5最大生命值</color>\r\n<color=#ff0000>-3护甲</color>\r\n听听音乐放松一下", 2, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"10\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"5\"},{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"5\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(34, "WinterMelon", "Shop/Props/WinterMelon", 130, "<color=#9932CD>冰镇西瓜</color>\r\n<color=#00ff00>+25最大生命值</color>\r\n<color=#00ff00>+10肾上腺素</color>\r\n<color=#00ff00>+10范围</color>\r\n<color=#ff0000>-10生命恢复</color>\r\n吃太多会拉肚子", 3, 0, 0, 0, "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"25\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"10\"},{\"attributeTypeString\":\"Range\",\"increment\":\"10\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(35, "melon", "Shop/Props/melon", 105, "<color=#9932CD>西瓜</color>\r\n<color=#00ff00>+20最大生命值</color>\r\n<color=#00ff00>+10生命恢复</color>\r\n<color=#00ff00>+5范围</color>\r\n<color=#ff0000>-5护甲</color>\r\n肚子都要被撑爆了", 3, 0, 0, 0, "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"20\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"10\"},{\"attributeTypeString\":\"Range\",\"increment\":\"5\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(36, "Pot_Water", "Shop/Props/Pot_Water", 80, "<color=#9932CD>水花盆</color>\r\n<color=#00ff00>+20植物学</color>\r\n可以在上面种荷叶", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"20\"}]"));
		AddItem(new ConfPropCardsItem(37, "Spinacia", "Shop/Props/Spinacia", 145, "<color=#9932CD>菠菜</color>\r\n<color=#00ff00>+10力量</color>\r\n<color=#00ff00>+10伤害</color>\r\n<color=#9932CD>解锁击退，并且僵尸尸体飞行造成碰撞伤害(每有一个伤害增加25%,最大100%)</color>\r\n吃了它我能把僵尸直接干飞", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Power\",\"increment\":\"10\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(38, "Marigold_petals", "Shop/Props/Marigold_petals", 128, "<color=#9932CD>金盏花瓣</color>\r\n<color=#00ff00>+50金币</color>\r\n<color=#00ff00>+5幸运</color>\r\n<color=#00ff00>+5植物学</color>\r\n看看她是这么产生金币的", 3, 0, 0, 0, "[{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"50\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"5\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"5\"}]"));
		AddItem(new ConfPropCardsItem(39, "TreeFood2", "Shop/Props/TreeFood2", 126, "<color=#9932CD>智慧树肥料</color>\r\n<color=#00ff00>+20植物学</color>\r\n<color=#00ff00>+5幸运</color>\r\n<color=#00ff00>+100阳光</color>\r\n这个好", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"20\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"5\"},{\"attributeTypeString\":\"Sunshine\",\"increment\":\"100\"}]"));
		AddItem(new ConfPropCardsItem(40, "basketball", "Shop/Props/basketball", 118, "<color=#9932CD>篮球</color>\r\n<color=#00ff00>+4力量</color>\r\n<color=#00ff00>+10攻击速度</color>\r\n<color=#00ff00>+3暴击率</color>\r\n<color=#00ff00>+2幸运</color>\r\n听说鸽鸽很会玩这个", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Power\",\"increment\":\"4\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"10\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"3\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"2\"}]"));
		AddItem(new ConfPropCardsItem(41, "LawnMower", "Shop/Props/LawnMower", 144, "<color=#9932CD>小推车</color>\r\n<color=#00ff00>+2力量</color>\r\n<color=#00ff00>+8伤害</color>\r\n<color=#00ff00>+10速度</color>\r\n并且对某直线上的僵尸造成{0}\r\n伤害冷却时间{1}", 3, 1, 10, 8, "[{\"attributeTypeString\":\"Power\",\"increment\":\"2\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"8\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(42, "bucket", "Shop/Props/bucket", 125, "<color=#9932CD>铁桶</color>\r\n<color=#00ff00>+8力量</color>\r\n<color=#00ff00>+6护甲</color>\r\n<color=#ff0000>-10范围</color>\r\n来自铁桶僵尸的战利品，但是会影响到我的视野", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Power\",\"increment\":\"8\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"6\"},{\"attributeTypeString\":\"Range\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(43, "cone", "Shop/Props/cone", 100, "<color=#9932CD>路障</color>\r\n<color=#00ff00>+6力量</color>\r\n<color=#00ff00>+6护甲</color>\r\n<color=#ff0000>-10范围</color>\r\n来自路障僵尸的战利品，但是会影响到我的视野", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Power\",\"increment\":\"6\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"6\"},{\"attributeTypeString\":\"Range\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(44, "screendoor", "Shop/Props/screendoor", 110, "<color=#9932CD>铁栅栏</color>\r\n<color=#00ff00>+5力量</color>\r\n<color=#00ff00>+15伤害</color>\r\n<color=#00ff00>+5护甲</color>\r\n<color=#ff0000>-8速度</color>\r\n来自铁栅栏僵尸的战利品，会影响到我正常走路", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Power\",\"increment\":\"5\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"15\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"5\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-8\"}]"));
		AddItem(new ConfPropCardsItem(45, "paper", "Shop/Props/paper", 112, "<color=#9932CD>报纸</color>\r\n<color=#00ff00>+25植物学</color>\r\n<color=#00ff00>+25金币</color>\r\n<color=#00ff00>+5幸运</color>\r\n<color=#ff0000>-3力量</color>\r\n来自读报僵尸的战利品，里面蕴含的知识可真不少", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"25\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"25\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"5\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(46, "ladder", "Shop/Props/ladder", 136, "<color=#9932CD>扶梯</color>\r\n<color=#00ff00>+5力量</color>\r\n<color=#00ff00>+10伤害</color>\r\n<color=#00ff00>+6暴击率</color>\r\n<color=#00ff00>+10范围</color>\r\n<color=#ff0000>-6攻击速度</color>\r\n来自扶梯僵尸的战利品，能攻击到更远的地方了", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Power\",\"increment\":\"5\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"10\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"6\"},{\"attributeTypeString\":\"Range\",\"increment\":\"10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"-6\"}]"));
		AddItem(new ConfPropCardsItem(47, "casque", "Shop/Props/casque", 134, "<color=#9932CD>橄榄球头盔</color>\r\n<color=#00ff00>+10护甲</color>\r\n<color=#00ff00>+8幸运</color>\r\n<color=#00ff00>+30金币</color>\r\n<color=#ff0000>-20范围</color>\r\n来自橄榄球僵尸的战利品，会影响到我的视野", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Armor\",\"increment\":\"10\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"8\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"30\"},{\"attributeTypeString\":\"Range\",\"increment\":\"-20\"}]"));
		AddItem(new ConfPropCardsItem(48, "guideboard", "Shop/Props/guideboard", 124, "<color=#9932CD>路牌</color>\r\n<color=#00ff00>+6力量</color>\r\n<color=#00ff00>+15范围</color>\r\n<color=#00ff00>+6暴击率</color>\r\n<color=#00ff00>+2护甲</color>\r\n<color=#ff0000>-10速度</color>\r\n来自巨人僵尸的战利品，这么重的玩意儿太影响我的移动速度了", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Power\",\"increment\":\"6\"},{\"attributeTypeString\":\"Range\",\"increment\":\"15\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"6\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"2\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(49, "dirttruck", "Shop/Props/dirttruck", 110, "<color=#9932CD>推土车</color>\r\n<color=#00ff00>+10植物学</color>\r\n<color=#00ff00>+5力量</color>\r\n<color=#00ff00>+5护甲</color>\r\n<color=#ff0000>-5速度<</color>\r\n能托运泥土，锻炼身体，但是老了推不太动", 3, 0, 0, 0, "[{\"attributeTypeString\":\"Botany\",\"increment\":\"10\"},{\"attributeTypeString\":\"Power\",\"increment\":\"5\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"5\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(50, "icetrap", "Shop/Props/icetrap", 286, "<color=#ff0000>冰碎</color>\r\n<color=#00ff00>+32肾上腺素</color>\r\n<color=#00ff00>+5幸运</color>\r\n<color=#00ff00>+60金币</color>\r\n<color=#00ff00>+7护甲</color>\r\n<color=#00ff00>+30速度</color>\r\n<color=#ff0000>-10生命恢复</color>\r\n<color=#ff0000>-10攻击速度</color>\r\n看起来就很冷", 4, 0, 0, 0, "[{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"32\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"30\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"7\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"5\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"60\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"-10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(51, "fire", "Shop/Props/fire", 260, "<color=#ff0000>火焰</color>\r\n<color=#00ff00>+10暴击率</color>\r\n<color=#00ff00>+200阳光</color>\r\n<color=#00ff00>+30伤害</color>\r\n<color=#00ff00>+15速度</color>\r\n<color=#00ff00>+15攻击速度</color>\r\n自身每秒<color=#ff0000>{0}生命值</color>\r\n火焰正在燃烧", 4, 2, -1, 0, "[{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"30\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"10\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"15\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"15\"},{\"attributeTypeString\":\"Sunshine\",\"increment\":\"200\"}]"));
		AddItem(new ConfPropCardsItem(52, "hammer", "Shop/Props/hammer", 320, "<color=#ff0000>木槌</color>\r\n<color=#00ff00>+10暴击率</color>\r\n<color=#00ff00>+10力量</color>\r\n<color=#00ff00>+10攻击速度</color>\r\n鼠标左键使用木槌造成{0}伤害\r\n冷却时间{1}s\r\n这太哇塞了", 4, 3, 3, 2, "[{\"attributeTypeString\":\"Power\",\"increment\":\"10\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(53, "PortalCard", "Shop/Props/PortalCard", 344, "<color=#ff0000>传送门</color>\r\n<color=#00ff00>+20伤害</color>\r\n<color=#00ff00>+100金币</color>\r\n<color=#00ff00>+10幸运</color>\r\n可在场上随机位置生成两个传送门，这很酷\r\n", 4, 0, 0, 0, "[{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"20\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"10\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"100\"}]"));
		AddItem(new ConfPropCardsItem(54, "fogmachine", "Shop/Props/fogmachine", 258, "<color=#ff0000>雾机</color>\r\n<color=#00ff00>+40最大生命值</color>\r\n<color=#00ff00>+10伤害</color>\r\n<color=#00ff00>+6幸运</color>\r\n<color=#00ff00>+6生命恢复</color>\r\n<color=#ff0000>-10速度</color>\r\n有概率产生雾，免疫远程攻击\r\n", 4, 4, 0, 0, "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"40\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"10\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"6\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"6\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(55, "lights", "Shop/Props/lights", 255, "<color=#ff0000>灯光机</color>\r\n<color=#00ff00>+20最大生命值</color>\r\n<color=#00ff00>+20伤害</color>\r\n<color=#00ff00>+100阳光</color>\r\n<color=#00ff00>+20攻击速度</color>\r\n<color=#00ff00>+5范围</color>\r\n<color=#ff0000>-10暴击率</color>\r\n有灯光才有气氛\r\n", 4, 4, 0, 0, "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"20\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"20\"},{\"attributeTypeString\":\"Sunshine\",\"increment\":\"100\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"20\"},{\"attributeTypeString\":\"Range\",\"increment\":\"5\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(56, "mic", "Shop/Props/mic", 325, "<color=#ff0000>麦克风</color>\r\n<color=#00ff00>+20肾上腺素</color>\r\n<color=#00ff00>+10暴击率</color>\r\n<color=#00ff00>+100金币</color>\r\n<color=#00ff00>+10伤害</color>\r\n<color=#00ff00>+10速度</color>\r\n<color=#ff0000>-5力量</color>\r\n使我变成大明星\r\n", 4, 4, 0, 0, "[{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"20\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"10\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"100\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"10\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"10\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(57, "speaker", "Shop/Props/speaker", 340, "<color=#ff0000>音响</color>\r\n<color=#00ff00>+20范围</color>\r\n<color=#00ff00>+10力量</color>\r\n<color=#00ff00>+5护甲</color>\r\n<color=#00ff00>+20伤害</color>\r\n<color=#00ff00>+20攻击速度</color>\r\n<color=#ff0000>-10速度</color>\r\n使我的声音传的更远\r\n", 4, 4, 0, 0, "[{\"attributeTypeString\":\"Range\",\"increment\":\"20\"},{\"attributeTypeString\":\"Power\",\"increment\":\"10\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"5\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"20\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"20\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-10\"}]"));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfPropCardsItem GetItem(int id)
	{
		return GetItemObject<ConfPropCardsItem>(id);
	}
	
}
	
