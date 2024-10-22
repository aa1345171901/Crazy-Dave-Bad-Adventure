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
	/// 道具类型
	/// </summary>
	public int propType;

	/// <summary>
	/// 默认参数1
	/// </summary>
	public int value1;

	/// <summary>
	/// 冷却时间
	/// </summary>
	public int coolingTime;

	/// <summary>
	/// 最大购买个数，0无限制
	/// </summary>
	public int maxNum;

	/// <summary>
	/// 前置购买道具
	/// </summary>
	public string frontProp;

	/// <summary>
	/// 获得道具对属性的增改，之前设计json更改，修改起来比较麻烦就不改了
	/// </summary>
	public string attributes;


	public ConfPropCardsItem()
	{
	}

	public ConfPropCardsItem(int id, string propName, string propImagePath, int defaultPrice, string info, int quality, int propType, int value1, int coolingTime, int maxNum, string frontProp, string attributes)
	{
		this.id = id;
		this.propName = propName;
		this.propImagePath = propImagePath;
		this.defaultPrice = defaultPrice;
		this.info = info;
		this.quality = quality;
		this.propType = propType;
		this.value1 = value1;
		this.coolingTime = coolingTime;
		this.maxNum = maxNum;
		this.frontProp = frontProp;
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
		AddItem(new ConfPropCardsItem(1, "jaw", "Shop/Props/jaw", 29, "propInfo_jaw", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"2\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"2\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"5\"},{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(2, "sadheart", "Shop/Props/sadheart", 21, "propInfo_sadheart", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"5\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(3, "magnetic", "Shop/Props/magnetic", 35, "propInfo_magnetic", 1, 0, 0, 0, 1, "", "[{\"attributeTypeString\":\"Speed\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(4, "blackhole", "Shop/Props/blackhole", 35, "propInfo_blackhole", 1, 0, 0, 0, 1, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"-4\"}]"));
		AddItem(new ConfPropCardsItem(5, "plate", "Shop/Props/plate", 27, "propInfo_plate", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"2\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"10\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"1\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(6, "throat", "Shop/Props/throat", 20, "propInfo_throat", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"5\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"5\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(7, "SeaShroom_head", "Shop/Props/SeaShroom_head", 18, "propInfo_SeaShroom_head", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Lucky\",\"increment\":\"2\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"3\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"5\"},{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(8, "SunShroom_head", "Shop/Props/SunShroom_head", 25, "propInfo_SunShroom_head", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Sunshine\",\"increment\":\"50\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"3\"},{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"4\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(9, "beard", "Shop/Props/beard", 18, "propInfo_beard", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Armor\",\"increment\":\"2\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"1\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(10, "glasses", "Shop/Props/glasses", 25, "propInfo_glasses", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Range\",\"increment\":\"5\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"1\"}]"));
		AddItem(new ConfPropCardsItem(11, "leaf", "Shop/Props/leaf", 20, "propInfo_leaf", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"3\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"1\"}]"));
		AddItem(new ConfPropCardsItem(12, "smallflower", "Shop/Props/smallflower", 24, "propInfo_smallflower", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"4\"}]"));
		AddItem(new ConfPropCardsItem(13, "fertilizer", "Shop/Props/fertilizer", 16, "propInfo_fertilizer", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"4\"}]"));
		AddItem(new ConfPropCardsItem(14, "key", "Shop/Props/key", 25, "propInfo_key", 1, 0, 0, 0, 0, "", ""));
		AddItem(new ConfPropCardsItem(15, "wallet", "Shop/Props/wallet", 20, "propInfo_wallet", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(16, "Diamond", "Shop/Props/Diamond", 66, "propInfo_Diamond", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"30\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"4\"}]"));
		AddItem(new ConfPropCardsItem(17, "SunFlower", "Shop/Props/SunFlower", 54, "propInfo_SunFlower", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"5\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"3\"},{\"attributeTypeString\":\"Sunshine\",\"increment\":\"50\"}]"));
		AddItem(new ConfPropCardsItem(18, "basket", "Shop/Props/basket", 55, "propInfo_basket", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Range\",\"increment\":\"10\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"3\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"5\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(19, "rake", "Shop/Props/rake", 80, "propInfo_rake", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"5\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"3\"}]"));
		AddItem(new ConfPropCardsItem(20, "wateringcan", "Shop/Props/wateringcan", 51, "propInfo_wateringcan", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"4\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"4\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"5\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"-4\"}]"));
		AddItem(new ConfPropCardsItem(21, "jackbox", "Shop/Props/jackbox", 58, "propInfo_jackbox", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Lucky\",\"increment\":\"5\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"5\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"5\"},{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(22, "splash3", "Shop/Props/splash3", 85, "propInfo_splash3", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"10\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"4\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"10\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-5\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(23, "SodRollCap", "Shop/Props/SodRollCap", 80, "propInfo_SodRollCap", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"6\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"8\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(24, "Spikerock", "Shop/Props/Spikerock", 70, "propInfo_Spikerock", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Speed\",\"increment\":\"10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"10\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"2\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(25, "Stinky_turn1", "Shop/Props/Stinky_turn1", 66, "propInfo_Stinky_turn1", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Armor\",\"increment\":\"8\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"5\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-5\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(26, "bugspray", "Shop/Props/bugspray", 70, "propInfo_bugspray", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"10\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"20\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"-6\"}]"));
		AddItem(new ConfPropCardsItem(27, "shovel", "Shop/Props/shovel", 52, "propInfo_shovel", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(28, "cardSlot", "Shop/Props/cardSlot", 55, "propInfo_cardSlot", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(29, "book", "Shop/Props/book", 50, "propInfo_book", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"15\"}]"));
		AddItem(new ConfPropCardsItem(30, "cup", "Shop/Props/cup", 54, "propInfo_cup", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"20\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"3\"}]"));
		AddItem(new ConfPropCardsItem(31, "sandwich", "Shop/Props/sandwich", 65, "propInfo_sandwich", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"10\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"5\"}]"));
		AddItem(new ConfPropCardsItem(32, "lock", "Shop/Props/lock", 45, "propInfo_lock", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"5\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"5\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(33, "radio", "Shop/Props/radio", 50, "propInfo_radio", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"10\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"5\"},{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"5\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(34, "WinterMelon", "Shop/Props/WinterMelon", 130, "propInfo_WinterMelon", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"25\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"10\"},{\"attributeTypeString\":\"Range\",\"increment\":\"10\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(35, "melon", "Shop/Props/melon", 105, "propInfo_melon", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"20\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"10\"},{\"attributeTypeString\":\"Range\",\"increment\":\"5\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(36, "Pot_Water", "Shop/Props/Pot_Water", 80, "propInfo_Pot_Water", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"20\"}]"));
		AddItem(new ConfPropCardsItem(37, "Spinacia", "Shop/Props/Spinacia", 145, "propInfo_Spinacia", 3, 0, 0, 0, 4, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"10\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(38, "Marigold_petals", "Shop/Props/Marigold_petals", 128, "propInfo_Marigold_petals", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"50\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"5\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"5\"}]"));
		AddItem(new ConfPropCardsItem(39, "TreeFood2", "Shop/Props/TreeFood2", 126, "propInfo_TreeFood2", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"20\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"5\"},{\"attributeTypeString\":\"Sunshine\",\"increment\":\"100\"}]"));
		AddItem(new ConfPropCardsItem(40, "basketball", "Shop/Props/basketball", 118, "propInfo_basketball", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"4\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"10\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"3\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"2\"}]"));
		AddItem(new ConfPropCardsItem(41, "LawnMower", "Shop/Props/LawnMower", 144, "propInfo_LawnMower", 3, 1, 10, 8, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"2\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"8\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(42, "bucket", "Shop/Props/bucket", 125, "propInfo_bucket", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"8\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"6\"},{\"attributeTypeString\":\"Range\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(43, "cone", "Shop/Props/cone", 100, "propInfo_cone", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"6\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"6\"},{\"attributeTypeString\":\"Range\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(44, "screendoor", "Shop/Props/screendoor", 110, "propInfo_screendoor", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"5\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"15\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"5\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-8\"}]"));
		AddItem(new ConfPropCardsItem(45, "paper", "Shop/Props/paper", 112, "propInfo_paper", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"25\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"25\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"5\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(46, "ladder", "Shop/Props/ladder", 136, "propInfo_ladder", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"5\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"10\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"6\"},{\"attributeTypeString\":\"Range\",\"increment\":\"10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"-6\"}]"));
		AddItem(new ConfPropCardsItem(47, "casque", "Shop/Props/casque", 134, "propInfo_casque", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Armor\",\"increment\":\"10\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"8\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"30\"},{\"attributeTypeString\":\"Range\",\"increment\":\"-20\"}]"));
		AddItem(new ConfPropCardsItem(48, "guideboard", "Shop/Props/guideboard", 124, "propInfo_guideboard", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"6\"},{\"attributeTypeString\":\"Range\",\"increment\":\"15\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"6\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"2\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(49, "dirttruck", "Shop/Props/dirttruck", 110, "propInfo_dirttruck", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Botany\",\"increment\":\"10\"},{\"attributeTypeString\":\"Power\",\"increment\":\"5\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"5\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(50, "icetrap", "Shop/Props/icetrap", 286, "propInfo_icetrap", 4, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"32\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"30\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"7\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"5\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"60\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"-10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(51, "fire", "Shop/Props/fire", 260, "propInfo_fire", 4, 2, -1, 0, 0, "", "[{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"30\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"10\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"15\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"15\"},{\"attributeTypeString\":\"Sunshine\",\"increment\":\"200\"}]"));
		AddItem(new ConfPropCardsItem(52, "hammer", "Shop/Props/hammer", 320, "propInfo_hammer", 4, 3, 3, 2, 1, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"10\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(53, "PortalCard", "Shop/Props/PortalCard", 344, "propInfo_PortalCard", 4, 0, 0, 0, 1, "", "[{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"20\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"10\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"100\"}]"));
		AddItem(new ConfPropCardsItem(54, "fogmachine", "Shop/Props/fogmachine", 258, "propInfo_fogmachine", 4, 4, 0, 0, 1, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"40\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"10\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"6\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"6\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(55, "lights", "Shop/Props/lights", 255, "propInfo_lights", 4, 4, 0, 0, 1, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"20\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"20\"},{\"attributeTypeString\":\"Sunshine\",\"increment\":\"100\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"20\"},{\"attributeTypeString\":\"Range\",\"increment\":\"5\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(56, "mic", "Shop/Props/mic", 325, "propInfo_mic", 4, 4, 0, 0, 1, "", "[{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"20\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"10\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"100\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"10\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"10\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(57, "speaker", "Shop/Props/speaker", 340, "propInfo_speaker", 4, 4, 0, 0, 1, "", "[{\"attributeTypeString\":\"Range\",\"increment\":\"20\"},{\"attributeTypeString\":\"Power\",\"increment\":\"10\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"5\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"20\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"20\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(58, "brokenEggshell", "Shop/Props/brokenEggshell", 25, "propInfo_brokenEggshell", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"2\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"2\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(59, "dangerReminder", "Shop/Props/dangerReminder", 22, "propInfo_dangerReminder", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"5\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"5\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(60, "goggles", "Shop/Props/goggles", 20, "propInfo_goggles", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Range\",\"increment\":\"5\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"2\"},{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"4\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-1\"}]"));
		AddItem(new ConfPropCardsItem(61, "staff", "Shop/Props/staff", 28, "propInfo_staff", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"15\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(62, "shell", "Shop/Props/shell", 22, "propInfo_shell", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Armor\",\"increment\":\"4\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(63, "gong", "Shop/Props/gong", 24, "propInfo_gong", 1, 5, 5, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"4\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"5\"},{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"4\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(64, "gongStick", "Shop/Props/gongStick", 24, "propInfo_gongStick", 1, 5, 5, 0, 0, "", "[{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"3\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"6\"},{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"4\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"-3\"}]"));
		AddItem(new ConfPropCardsItem(65, "ignitedBomb", "Shop/Props/ignitedBomb", 25, "propInfo_ignitedBomb", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"4\"},{\"attributeTypeString\":\"Power\",\"increment\":\"2\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"2\"},{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"3\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(66, "pumpkin", "Shop/Props/pumpkin", 30, "propInfo_pumpkin", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"4\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"2\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"1\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"-1\"}]"));
		AddItem(new ConfPropCardsItem(67, "wing", "Shop/Props/wing", 24, "propInfo_wing", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Speed\",\"increment\":\"5\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"2\"},{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"4\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(68, "pennant", "Shop/Props/pennant", 55, "propInfo_pennant", 1, 5, 10, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"5\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"4\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"4\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"2\"}]"));
		AddItem(new ConfPropCardsItem(69, "damagedSkateboard", "Shop/Props/damagedSkateboard", 22, "propInfo_damagedSkateboard", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Speed\",\"increment\":\"5\"},{\"attributeTypeString\":\"Power\",\"increment\":\"1\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(70, "butter", "Shop/Props/butter", 20, "propInfo_butter", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"4\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"2\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"2\"},{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(71, "actionBars", "Shop/Props/actionBars", 22, "propInfo_actionBars", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Speed\",\"increment\":\"4\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"4\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(72, "recorder", "Shop/Props/recorder", 50, "propInfo_recorder", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Lucky\",\"increment\":\"5\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"2\"},{\"attributeTypeString\":\"GoldCoins\",\"increment\":\"5\"},{\"attributeTypeString\":\"Botany\",\"increment\":\"5\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(73, "mask", "Shop/Props/mask", 70, "propInfo_mask", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"10\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"10\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"5\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"5\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"5\"},{\"attributeTypeString\":\"Range\",\"increment\":\"-10\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(74, "drumsticks", "Shop/Props/drumsticks", 66, "propInfo_drumsticks", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"8\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"8\"}]"));
		AddItem(new ConfPropCardsItem(75, "cloud", "Shop/Props/cloud", 45, "propInfo_cloud", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Speed\",\"increment\":\"6\"},{\"attributeTypeString\":\"Range\",\"increment\":\"6\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"5\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"-1\"}]"));
		AddItem(new ConfPropCardsItem(76, "iceCake", "Shop/Props/iceCake", 50, "propInfo_iceCake", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"5\"},{\"attributeTypeString\":\"Power\",\"increment\":\"2\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"5\"}]"));
		AddItem(new ConfPropCardsItem(77, "batWings", "Shop/Props/batWings", 45, "propInfo_batWings", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Speed\",\"increment\":\"10\"},{\"attributeTypeString\":\"Range\",\"increment\":\"5\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"5\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(78, "hemline", "Shop/Props/hemline", 47, "propInfo_hemline", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Armor\",\"increment\":\"5\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"4\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(79, "umbrella", "Shop/Props/umbrella", 52, "propInfo_umbrella", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"5\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"5\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"4\"},{\"attributeTypeString\":\"Range\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(80, "brokenHat", "Shop/Props/brokenHat", 26, "propInfo_brokenHat", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"5\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"1\"},{\"attributeTypeString\":\"Power\",\"increment\":\"1\"},{\"attributeTypeString\":\"Range\",\"increment\":\"-10\"}]"));
		AddItem(new ConfPropCardsItem(81, "pinecone", "Shop/Props/pinecone", 150, "propInfo_pinecone", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"20\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"10\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"10\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"-10\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(82, "zombieRightHand", "Shop/Props/zombieRightHand", 120, "propInfo_zombieRightHand", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"10\"},{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"20\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-10\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(83, "zombieLeftHand", "Shop/Props/zombieLeftHand", 120, "propInfo_zombieLeftHand", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"8\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"20\"},{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"-10\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(84, "imitator", "Shop/Props/imitator", 300, "propInfo_imitator", 4, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"50\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(85, "base", "Shop/Props/base", 16, "propInfo_base", 1, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"1\"}]"));
		AddItem(new ConfPropCardsItem(86, "zombieBanOrder", "Shop/Props/zombieBanOrder", 15, "propInfo_zombieBanOrder", 1, 5, -10, 0, 5, "", "[{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"5\"},{\"attributeTypeString\":\"Power\",\"increment\":\"-1\"}]"));
		AddItem(new ConfPropCardsItem(87, "PoleVaulting", "Shop/Props/PoleVaulting", 130, "propInfo_PoleVaulting", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"5\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"5\"},{\"attributeTypeString\":\"Range\",\"increment\":\"5\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"5\"}]"));
		AddItem(new ConfPropCardsItem(88, "cart", "Shop/Props/cart", 45, "propInfo_cart", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Power\",\"increment\":\"2\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"5\"}]"));
		AddItem(new ConfPropCardsItem(89, "unknownButton", "Shop/Props/unknownButton", 56, "propInfo_unknownButton", 2, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"10\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"5\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"2\"}]"));
		AddItem(new ConfPropCardsItem(90, "zamboni", "Shop/Props/zamboni", 100, "propInfo_zamboni", 3, 0, 0, 0, 0, "", "[{\"attributeTypeString\":\"Speed\",\"increment\":\"10\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"10\"},{\"attributeTypeString\":\"AttackSpeed\",\"increment\":\"5\"}]"));
		AddItem(new ConfPropCardsItem(91, "purpleGarlic", "Shop/Props/purpleGarlic", 25, "propInfo_purpleGarlic", 1, 6, 1, 1, 4, "", "[{\"attributeTypeString\":\"Speed\",\"increment\":\"5\"},{\"attributeTypeString\":\"PercentageDamage\",\"increment\":\"4\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"-2\"}]"));
		AddItem(new ConfPropCardsItem(92, "mint", "Shop/Props/mint", 64, "propInfo_mint", 2, 0, 8, 0, 6, "purpleGarlic", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"5\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"4\"}]"));
		AddItem(new ConfPropCardsItem(93, "fireStar", "Shop/Props/fireStar", 24, "propInfo_fireStar", 1, 7, 1, 5, 1, "", "[{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"2\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"1\"}]"));
		AddItem(new ConfPropCardsItem(94, "fireSeed", "Shop/Props/fireSeed", 66, "propInfo_fireSeed", 2, 7, 3, 4, 1, "fireStar", "[{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"4\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"2\"}]"));
		AddItem(new ConfPropCardsItem(95, "fireBall", "Shop/Props/fireBall", 144, "propInfo_fireBall", 3, 7, 5, 3, 1, "fireSeed", "[{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"8\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"4\"}]"));
		AddItem(new ConfPropCardsItem(96, "fireElf", "Shop/Props/fireElf", 299, "propInfo_fireElf", 4, 7, 10, 2, 1, "fireBall", "[{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"20\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(97, "waterDroplet", "Shop/Props/waterDroplet", 22, "propInfo_waterDroplet", 1, 8, 1, 8, 1, "", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"2\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"2\"}]"));
		AddItem(new ConfPropCardsItem(98, "waterBead", "Shop/Props/waterBead", 64, "propInfo_waterBead", 2, 8, 3, 6, 1, "waterDroplet", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"4\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"4\"}]"));
		AddItem(new ConfPropCardsItem(99, "waterSplash", "Shop/Props/waterSplash", 124, "propInfo_waterSplash", 3, 8, 5, 4, 1, "waterBead", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"10\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"10\"}]"));
		AddItem(new ConfPropCardsItem(100, "waterElf", "Shop/Props/waterElf", 284, "propInfo_waterElf", 4, 8, 10, 2, 1, "waterSplash", "[{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"20\"},{\"attributeTypeString\":\"LifeRecovery\",\"increment\":\"20\"}]"));
		AddItem(new ConfPropCardsItem(101, "darkCloud", "Shop/Props/darkCloud", 144, "propInfo_darkCloud", 3, 9, 5, 6, 4, "", "[{\"attributeTypeString\":\"Adrenaline\",\"increment\":\"25\"},{\"attributeTypeString\":\"CriticalDamage\",\"increment\":\"10\"},{\"attributeTypeString\":\"Sunshine\",\"increment\":\"100\"},{\"attributeTypeString\":\"Lucky\",\"increment\":\"-2\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"-1\"}]"));
		AddItem(new ConfPropCardsItem(102, "vampireScepter", "Shop/Props/vampireScepter", 34, "propInfo_vampireScepter", 1, 0, 1, 0, 10, "", ""));
		AddItem(new ConfPropCardsItem(103, "alloyHelmet", "Shop/Props/alloyHelmet", 99, "propInfo_alloyHelmet", 2, 0, 0, 0, 1, "", "[{\"attributeTypeString\":\"Armor\",\"increment\":\"4\"},{\"attributeTypeString\":\"MaximumHP\",\"increment\":\"10\"},{\"attributeTypeString\":\"Range\",\"increment\":\"-5\"}]"));
		AddItem(new ConfPropCardsItem(104, "alloyEye", "Shop/Props/alloyEye", 88, "propInfo_alloyEye", 2, 0, 0, 0, 1, "", "[{\"attributeTypeString\":\"Range\",\"increment\":\"10\"},{\"attributeTypeString\":\"CriticalHitRate\",\"increment\":\"2\"},{\"attributeTypeString\":\"Armor\",\"increment\":\"2\"},{\"attributeTypeString\":\"Speed\",\"increment\":\"-5\"}]"));
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
	
