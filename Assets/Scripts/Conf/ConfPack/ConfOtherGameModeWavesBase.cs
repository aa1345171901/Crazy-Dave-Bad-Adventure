using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfOtherGameModeWavesItem : ConfBaseItem
{
	/// <summary>
	/// 索引id，对应OtherGameModes的id
	/// </summary>
	public int gameModeId;

	/// <summary>
	/// 僵尸种类
	/// </summary>
	public int zombieType;

	/// <summary>
	/// 每次生成数量，三个阶段
	/// </summary>
	public int[] generateCount;

	/// <summary>
	/// 生成间隔时间，三个阶段
	/// </summary>
	public float[] intervalTime;

	/// <summary>
	/// 第一只生成时间(s)
	/// </summary>
	public int firstGenerateTime;


	public ConfOtherGameModeWavesItem()
	{
	}

	public ConfOtherGameModeWavesItem(int id, int gameModeId, int zombieType, int[] generateCount, float[] intervalTime, int firstGenerateTime)
	{
		this.id = id;
		this.gameModeId = gameModeId;
		this.zombieType = zombieType;
		this.generateCount = generateCount;
		this.intervalTime = intervalTime;
		this.firstGenerateTime = firstGenerateTime;
	}	

	public ConfOtherGameModeWavesItem Clone()
	{
		ConfOtherGameModeWavesItem item = (ConfOtherGameModeWavesItem)this.MemberwiseClone();
		item.generateCount = new int[this.generateCount.Length];
		for (int i = 0; i < this.generateCount.Length; i++)
        {
			item.generateCount[i] = this.generateCount[i];
		}
		item.intervalTime = new float[this.intervalTime.Length];
		for (int i = 0; i < this.intervalTime.Length; i++)
        {
			item.intervalTime[i] = this.intervalTime[i];
		}

		return item;
	}
}
public class ConfOtherGameModeWavesBase : ConfBase<ConfOtherGameModeWavesItem>
{
    public override void Init()
    {
		confName = "OtherGameModeWaves";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfOtherGameModeWavesItem(1, 1001, 0, new int[]{ 2, 3, 4 }, new float[]{ 4f, 3f, 2f }, 0));
		AddItem(new ConfOtherGameModeWavesItem(2, 1001, 0, new int[]{ 2, 3, 4 }, new float[]{ 4f, 2.8f, 1.8f }, 0));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfOtherGameModeWavesItem GetItem(int id)
	{
		return GetItemObject<ConfOtherGameModeWavesItem>(id);
	}
	
}
	
