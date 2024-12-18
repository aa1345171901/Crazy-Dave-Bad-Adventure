using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfWaveTimerItem : ConfBaseItem
{
	/// <summary>
	/// 波次
	/// </summary>
	public int waveIndex;

	/// <summary>
	/// 每波时间，单位s,0为无限时间
	/// </summary>
	public int waveTime;


	public ConfWaveTimerItem()
	{
	}

	public ConfWaveTimerItem(int id, int waveIndex, int waveTime)
	{
		this.id = id;
		this.waveIndex = waveIndex;
		this.waveTime = waveTime;
	}	

	public ConfWaveTimerItem Clone()
	{
		ConfWaveTimerItem item = (ConfWaveTimerItem)this.MemberwiseClone();

		return item;
	}
}
public class ConfWaveTimerBase : ConfBase<ConfWaveTimerItem>
{
    public override void Init()
    {
		confName = "WaveTimer";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfWaveTimerItem(1, 1, 54));
		AddItem(new ConfWaveTimerItem(2, 2, 54));
		AddItem(new ConfWaveTimerItem(3, 3, 54));
		AddItem(new ConfWaveTimerItem(4, 4, 54));
		AddItem(new ConfWaveTimerItem(5, 5, 54));
		AddItem(new ConfWaveTimerItem(6, 6, 54));
		AddItem(new ConfWaveTimerItem(7, 7, 54));
		AddItem(new ConfWaveTimerItem(8, 8, 54));
		AddItem(new ConfWaveTimerItem(9, 9, 54));
		AddItem(new ConfWaveTimerItem(10, 10, 120));
		AddItem(new ConfWaveTimerItem(11, 11, 54));
		AddItem(new ConfWaveTimerItem(12, 12, 54));
		AddItem(new ConfWaveTimerItem(13, 13, 54));
		AddItem(new ConfWaveTimerItem(14, 14, 54));
		AddItem(new ConfWaveTimerItem(15, 15, 120));
		AddItem(new ConfWaveTimerItem(16, 16, 54));
		AddItem(new ConfWaveTimerItem(17, 17, 54));
		AddItem(new ConfWaveTimerItem(18, 18, 54));
		AddItem(new ConfWaveTimerItem(19, 19, 54));
		AddItem(new ConfWaveTimerItem(20, 20, 0));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfWaveTimerItem GetItem(int id)
	{
		return GetItemObject<ConfWaveTimerItem>(id);
	}
	
}
	
