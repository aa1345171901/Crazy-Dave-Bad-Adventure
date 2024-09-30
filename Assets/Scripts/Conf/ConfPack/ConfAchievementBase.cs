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
	/// 参数
	/// </summary>
	public string param;


	public ConfAchievementItem()
	{
	}

	public ConfAchievementItem(int id, string achievementId, string title, string imgPath, string desc, string lockText, int process, int type, string param)
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
		AddItem(new ConfAchievementItem(1, "10001", "achievement_title1", "UI/Achievement/10001", "achievement_info1", "", 1, 1, ""));
		AddItem(new ConfAchievementItem(2, "10002", "achievement_title2", "UI/Achievement/10001", "achievement_info2", "", 300, 2, ""));
		AddItem(new ConfAchievementItem(3, "10003", "achievement_title3", "UI/Achievement/10001", "achievement_info3", "", 2000, 2, ""));
		AddItem(new ConfAchievementItem(4, "10004", "achievement_title4", "UI/Achievement/10001", "achievement_info4", "", 10000, 2, ""));
		AddItem(new ConfAchievementItem(5, "10005", "achievement_title5", "UI/Achievement/10001", "achievement_info5", "", 50, 3, ""));
		AddItem(new ConfAchievementItem(6, "10006", "achievement_title6", "UI/Achievement/10001", "achievement_info6", "", 200, 3, ""));
		AddItem(new ConfAchievementItem(7, "10007", "achievement_title7", "UI/Achievement/10001", "achievement_info7", "", 500, 3, ""));
		AddItem(new ConfAchievementItem(8, "10008", "achievement_title8", "UI/Achievement/10001", "achievement_info8", "", 1, 4, ""));
		AddItem(new ConfAchievementItem(9, "10009", "achievement_title9", "UI/Achievement/10001", "achievement_info9", "achievement_lock9", 4, 5, "1"));
		AddItem(new ConfAchievementItem(10, "10010", "achievement_title10", "UI/Achievement/10001", "achievement_info10", "achievement_lock10", 2, 5, "2"));
		AddItem(new ConfAchievementItem(11, "10011", "achievement_title11", "UI/Achievement/10001", "achievement_info11", "achievement_lock11", 2, 5, "9"));
		AddItem(new ConfAchievementItem(12, "10012", "achievement_title12", "UI/Achievement/10001", "achievement_info12", "achievement_lock12", 1, 6, "Pot_Water"));
		AddItem(new ConfAchievementItem(13, "10013", "achievement_title13", "UI/Achievement/10001", "achievement_info13", "achievement_lock13", 2, 5, "10"));
		AddItem(new ConfAchievementItem(14, "10014", "achievement_title14", "UI/Achievement/10001", "achievement_info14", "achievement_lock14", 2, 5, "29"));
		AddItem(new ConfAchievementItem(15, "10015", "achievement_title15", "UI/Achievement/10001", "achievement_info15", "achievement_lock15", 2, 5, "26"));
		AddItem(new ConfAchievementItem(16, "10016", "achievement_title16", "UI/Achievement/10001", "achievement_info16", "achievement_lock16", 10, 7, "11"));
		AddItem(new ConfAchievementItem(17, "10017", "achievement_title17", "UI/Achievement/10001", "achievement_info17", "achievement_lock17", 1, 8, "10"));
		AddItem(new ConfAchievementItem(18, "10018", "achievement_title18", "UI/Achievement/10001", "achievement_info18", "achievement_lock18", 1, 8, "2"));
		AddItem(new ConfAchievementItem(19, "10019", "achievement_title19", "UI/Achievement/10001", "achievement_info19", "achievement_lock19", 1, 8, "5"));
		AddItem(new ConfAchievementItem(20, "10020", "achievement_title20", "UI/Achievement/10001", "achievement_info20", "achievement_lock20", 1, 8, "11"));
		AddItem(new ConfAchievementItem(21, "10021", "achievement_title21", "UI/Achievement/10001", "achievement_info21", "achievement_lock21", 1, 8, "6"));
		AddItem(new ConfAchievementItem(22, "10022", "achievement_title22", "UI/Achievement/10001", "achievement_info22", "achievement_lock22", 1, 8, "1"));
		AddItem(new ConfAchievementItem(23, "10023", "achievement_title23", "UI/Achievement/10001", "achievement_info23", "achievement_lock23", 1, 8, "3"));
		AddItem(new ConfAchievementItem(24, "10024", "achievement_title24", "UI/Achievement/10001", "achievement_info24", "achievement_lock24", 1, 8, "9"));
		AddItem(new ConfAchievementItem(25, "10025", "achievement_title25", "UI/Achievement/10001", "achievement_info25", "achievement_lock25", 1, 9, "35"));
		AddItem(new ConfAchievementItem(26, "10026", "achievement_title26", "UI/Achievement/10001", "achievement_info26", "achievement_lock26", 1, 9, "36"));
		AddItem(new ConfAchievementItem(27, "10027", "achievement_title27", "UI/Achievement/10001", "achievement_info27", "achievement_lock27", 1, 6, ""));
		AddItem(new ConfAchievementItem(28, "10028", "achievement_title28", "UI/Achievement/10001", "achievement_info28", "", 10, 7, "6"));
		AddItem(new ConfAchievementItem(29, "10029", "achievement_title29", "UI/Achievement/10001", "achievement_info29", "", 1, 9, "1001"));
		AddItem(new ConfAchievementItem(30, "10030", "achievement_title30", "UI/Achievement/10001", "achievement_info30", "", 20, 10, "Power"));
		AddItem(new ConfAchievementItem(31, "10031", "achievement_title31", "UI/Achievement/10001", "achievement_info31", "", 100, 10, "MaximumHP"));
		AddItem(new ConfAchievementItem(32, "10032", "achievement_title32", "UI/Achievement/10001", "achievement_info32", "", 50, 10, "AttackSpeed"));
		AddItem(new ConfAchievementItem(33, "10033", "achievement_title33", "UI/Achievement/10001", "achievement_info33", "", 50, 10, "Armor"));
		AddItem(new ConfAchievementItem(34, "10034", "achievement_title34", "UI/Achievement/10001", "achievement_info34", "", 100, 10, "Botany"));
		AddItem(new ConfAchievementItem(35, "10035", "achievement_title35", "UI/Achievement/10001", "achievement_info35", "", 100, 10, "GoldCoins"));
		AddItem(new ConfAchievementItem(36, "10036", "achievement_title36", "UI/Achievement/10001", "achievement_info36", "", 500, 10, "Sunshine"));
		AddItem(new ConfAchievementItem(37, "10037", "achievement_title37", "UI/Achievement/10001", "achievement_info37", "", 25, 10, "Lucky"));
		AddItem(new ConfAchievementItem(38, "10038", "achievement_title38", "UI/Achievement/10001", "achievement_info38", "", 1, 0, ""));
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
	
