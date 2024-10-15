using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfAchievementItem : ConfBaseItem
{
	/// <summary>
	/// 成就id
	/// </summary>
	public string achievementId;

	/// <summary>
	/// 成就名称
	/// </summary>
	public string title;

	/// <summary>
	/// 图片路径
	/// </summary>
	public string imgPath;

	/// <summary>
	/// 成就描述
	/// </summary>
	public string desc;

	/// <summary>
	/// 解锁的目标描述
	/// </summary>
	public string lockText;

	/// <summary>
	/// 进度
	/// </summary>
	public int process;

	/// <summary>
	/// 成就类型
	/// </summary>
	public int type;

	/// <summary>
	/// 解锁成就参数
	/// </summary>
	public string param;

	/// <summary>
	/// 解锁物品参数  type_类型   1植物，2道具
	/// </summary>
	public string unlockParam;


	public ConfAchievementItem()
	{
	}

	public ConfAchievementItem(int id, string achievementId, string title, string imgPath, string desc, string lockText, int process, int type, string param, string unlockParam)
	{
		this.id = id;
		this.achievementId = achievementId;
		this.title = title;
		this.imgPath = imgPath;
		this.desc = desc;
		this.lockText = lockText;
		this.process = process;
		this.type = type;
		this.param = param;
		this.unlockParam = unlockParam;
	}	

	public ConfAchievementItem Clone()
	{
		ConfAchievementItem item = (ConfAchievementItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfAchievementBase : ConfBase<ConfAchievementItem>
{
    public override void Init()
    {
		confName = "Achievement";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfAchievementItem(1, "10001", "achievement_title1", "UI/Achievement/10001", "achievement_info1", "", 1, 1, "", ""));
		AddItem(new ConfAchievementItem(2, "20001", "achievement_title2", "UI/Achievement/20001", "achievement_info2", "", 5000, 2, "", ""));
		AddItem(new ConfAchievementItem(3, "20002", "achievement_title3", "UI/Achievement/20002", "achievement_info3", "", 10000, 2, "", ""));
		AddItem(new ConfAchievementItem(4, "20003", "achievement_title4", "UI/Achievement/20003", "achievement_info4", "", 40000, 2, "", ""));
		AddItem(new ConfAchievementItem(5, "30001", "achievement_title5", "UI/Achievement/30001", "achievement_info5", "", 200, 3, "", ""));
		AddItem(new ConfAchievementItem(6, "30002", "achievement_title6", "UI/Achievement/30002", "achievement_info6", "", 1000, 3, "", ""));
		AddItem(new ConfAchievementItem(7, "30003", "achievement_title7", "UI/Achievement/30003", "achievement_info7", "", 2000, 3, "", ""));
		AddItem(new ConfAchievementItem(8, "40001", "achievement_title8", "UI/Achievement/40001", "achievement_info8", "", 1, 4, "", ""));
		AddItem(new ConfAchievementItem(9, "50001", "achievement_title9", "UI/Achievement/50001", "achievement_info9", "achievement_lock9", 4, 5, "1", "1_2"));
		AddItem(new ConfAchievementItem(10, "50002", "achievement_title10", "UI/Achievement/50002", "achievement_info10", "achievement_lock10", 2, 5, "2", "1_11"));
		AddItem(new ConfAchievementItem(11, "50003", "achievement_title11", "UI/Achievement/50003", "achievement_info11", "achievement_lock11", 4, 5, "9", "1_40"));
		AddItem(new ConfAchievementItem(12, "50004", "achievement_title13", "UI/Achievement/50004", "achievement_info13", "achievement_lock13", 4, 5, "10", "1_12"));
		AddItem(new ConfAchievementItem(13, "50005", "achievement_title14", "UI/Achievement/50005", "achievement_info14", "achievement_lock14", 4, 5, "29", "1_33"));
		AddItem(new ConfAchievementItem(14, "50006", "achievement_title15", "UI/Achievement/50006", "achievement_info15", "achievement_lock15", 4, 5, "26", "1_25"));
		AddItem(new ConfAchievementItem(15, "60001", "achievement_title27", "UI/Achievement/60001", "achievement_info27", "achievement_lock27", 1, 6, "", "2_key"));
		AddItem(new ConfAchievementItem(16, "60002", "achievement_title12", "UI/Achievement/60002", "achievement_info12", "achievement_lock12", 1, 6, "Pot_Water", "1_5"));
		AddItem(new ConfAchievementItem(17, "70001", "achievement_title16", "UI/Achievement/70001", "achievement_info16", "achievement_lock16", 10, 7, "11", "1_37"));
		AddItem(new ConfAchievementItem(18, "70002", "achievement_title28", "UI/Achievement/70002", "achievement_info28", "", 10, 7, "6", ""));
		AddItem(new ConfAchievementItem(19, "80001", "achievement_title17", "UI/Achievement/80001", "achievement_info17", "achievement_lock17", 1, 8, "10", "2_basketball"));
		AddItem(new ConfAchievementItem(20, "80002", "achievement_title18", "UI/Achievement/80002", "achievement_info18", "achievement_lock18", 10, 8, "2", "2_bucket"));
		AddItem(new ConfAchievementItem(21, "80003", "achievement_title19", "UI/Achievement/80003", "achievement_info19", "achievement_lock19", 10, 8, "5", "2_casque"));
		AddItem(new ConfAchievementItem(22, "80004", "achievement_title20", "UI/Achievement/80004", "achievement_info20", "achievement_lock20", 1, 8, "11", "2_guideboard"));
		AddItem(new ConfAchievementItem(23, "80005", "achievement_title21", "UI/Achievement/80005", "achievement_info21", "achievement_lock21", 10, 8, "6", "2_paper"));
		AddItem(new ConfAchievementItem(24, "80006", "achievement_title22", "UI/Achievement/80006", "achievement_info22", "achievement_lock22", 20, 8, "1", "2_cone"));
		AddItem(new ConfAchievementItem(25, "80007", "achievement_title23", "UI/Achievement/80007", "achievement_info23", "achievement_lock23", 10, 8, "3", "2_screendoor"));
		AddItem(new ConfAchievementItem(26, "80008", "achievement_title24", "UI/Achievement/80008", "achievement_info24", "achievement_lock24", 1, 8, "9", "2_zamboni"));
		AddItem(new ConfAchievementItem(27, "80009", "achievement_title80009", "UI/Achievement/80009", "achievement_info80009", "achievement_lock80009", 20, 8, "7", "2_PoleVaulting"));
		AddItem(new ConfAchievementItem(28, "90001", "achievement_title25", "UI/Achievement/90001", "achievement_info25", "achievement_lock25", 20, 9, "35", "2_icetrap"));
		AddItem(new ConfAchievementItem(29, "90002", "achievement_title26", "UI/Achievement/90002", "achievement_info26", "achievement_lock26", 20, 9, "36", "2_fire"));
		AddItem(new ConfAchievementItem(30, "90003", "achievement_title29", "UI/Achievement/90003", "achievement_info29", "", 1, 9, "1001", ""));
		AddItem(new ConfAchievementItem(31, "100001", "achievement_title30", "UI/Achievement/100001", "achievement_info30", "", 20, 10, "Power", ""));
		AddItem(new ConfAchievementItem(32, "100002", "achievement_title31", "UI/Achievement/100002", "achievement_info31", "", 100, 10, "MaximumHP", ""));
		AddItem(new ConfAchievementItem(33, "100003", "achievement_title32", "UI/Achievement/100003", "achievement_info32", "", 50, 10, "AttackSpeed", ""));
		AddItem(new ConfAchievementItem(34, "100004", "achievement_title33", "UI/Achievement/100004", "achievement_info33", "", 50, 10, "Armor", ""));
		AddItem(new ConfAchievementItem(35, "100005", "achievement_title34", "UI/Achievement/100005", "achievement_info34", "", 100, 10, "Botany", ""));
		AddItem(new ConfAchievementItem(36, "100006", "achievement_title35", "UI/Achievement/100006", "achievement_info35", "", 100, 10, "GoldCoins", ""));
		AddItem(new ConfAchievementItem(37, "100007", "achievement_title36", "UI/Achievement/100007", "achievement_info36", "", 500, 10, "Sunshine", ""));
		AddItem(new ConfAchievementItem(38, "100008", "achievement_title37", "UI/Achievement/100008", "achievement_info37", "", 25, 10, "Lucky", ""));
		AddItem(new ConfAchievementItem(39, "100009", "achievement_title100009", "UI/Achievement/100009", "achievement_info100009", "", 100, 10, "CriticalHitRate", ""));
		AddItem(new ConfAchievementItem(40, "1", "achievement_title38", "UI/Achievement/1", "achievement_info38", "", 1, 0, "", ""));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfAchievementItem GetItem(int id)
	{
		return GetItemObject<ConfAchievementItem>(id);
	}
	
}
	
