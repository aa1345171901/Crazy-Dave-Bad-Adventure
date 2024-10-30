using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfOtherGameModeWavesParamItem : ConfBaseItem
{
	/// <summary>
	/// 索引id，对应OtherGameModes的id
	/// </summary>
	public int gameModeId;

	/// <summary>
	/// 过关时间，单位s,0为无限时间
	/// </summary>
	public int waveTime;

	/// <summary>
	/// 多少s增加僵尸,0不增强
	/// </summary>
	public float enhanceTime;

	/// <summary>
	/// 可掉落物品,空为不掉落，all为全部掉落
	/// </summary>
	public string[] dropProps;

	/// <summary>
	/// 可掉落植物卡片，空为不掉落，all为全部掉落
	/// </summary>
	public string[] dropPlants;


	public ConfOtherGameModeWavesParamItem()
	{
	}

	public ConfOtherGameModeWavesParamItem(int id, int gameModeId, int waveTime, float enhanceTime, string[] dropProps, string[] dropPlants)
	{
		this.id = id;
		this.gameModeId = gameModeId;
		this.waveTime = waveTime;
		this.enhanceTime = enhanceTime;
		this.dropProps = dropProps;
		this.dropPlants = dropPlants;
	}	

	public ConfOtherGameModeWavesParamItem Clone()
	{
		ConfOtherGameModeWavesParamItem item = (ConfOtherGameModeWavesParamItem)this.MemberwiseClone();
		item.dropProps = new string[this.dropProps.Length];
		for (int i = 0; i < this.dropProps.Length; i++)
        {
			item.dropProps[i] = this.dropProps[i];
		}
		item.dropPlants = new string[this.dropPlants.Length];
		for (int i = 0; i < this.dropPlants.Length; i++)
        {
			item.dropPlants[i] = this.dropPlants[i];
		}

		return item;
	}
}
public class ConfOtherGameModeWavesParamBase : ConfBase<ConfOtherGameModeWavesParamItem>
{
    public override void Init()
    {
		confName = "OtherGameModeWavesParam";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfOtherGameModeWavesParamItem(1, 1001, 300, 0f, new string[]{  }, new string[]{  }));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfOtherGameModeWavesParamItem GetItem(int id)
	{
		return GetItemObject<ConfOtherGameModeWavesParamItem>(id);
	}
	
}
	
