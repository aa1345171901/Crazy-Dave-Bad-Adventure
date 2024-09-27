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


	public ConfAchievementItem()
	{
	}

	public ConfAchievementItem(int id, string achievementId, string title, string imgPath, string desc, string lockText, int process, int type)
	{
		this.id = id;
		this.achievementId = achievementId;
		this.title = title;
		this.imgPath = imgPath;
		this.desc = desc;
		this.lockText = lockText;
		this.process = process;
		this.type = type;
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
		AddItem(new ConfAchievementItem(1, "10001", "property_MaximumHP", "UI/Achievement/10001", "第一次开始冒险", "解锁橄榄球头盔", 0, 0));
		AddItem(new ConfAchievementItem(2, "10002", "property_LifeRecovery", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(3, "10003", "property_Adrenaline", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(4, "10004", "property_Power", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(5, "10005", "property_PercentageDamage", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(6, "10006", "property_AttackSpeed", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(7, "10007", "property_Range", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(8, "10008", "property_CriticalHitRate", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(9, "10009", "property_MoveSpeed", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(10, "10010", "property_Armor", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(11, "10011", "property_Lucky", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(12, "10012", "property_Sunshine", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(13, "10013", "property_GoldCoins", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(14, "10014", "property_Botany", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(15, "10015", "grow_slotNum", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(16, "10016", "grow_addProp", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(17, "10017", "grow_addPlant", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(18, "10018", "grow_addSun", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(19, "10019", "grow_addMoney", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(20, "10020", "grow_resurrection", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(21, "10021", "grow_curse", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(22, "10022", "grow_physicalRecovery", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(23, "10023", "grow_runTime", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(24, "10024", "grow_dashTime", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
		AddItem(new ConfAchievementItem(25, "10025", "grow_dashRecovery", "UI/Achievement/10001", "第一次开始冒险", "", 10, 0));
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
	
