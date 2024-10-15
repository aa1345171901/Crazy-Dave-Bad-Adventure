using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConfWaveItem : ConfBaseItem
{
	/// <summary>
	/// 波次
	/// </summary>
	public int waveIndex;

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


	public ConfWaveItem()
	{
	}

	public ConfWaveItem(int id, int waveIndex, int zombieType, int[] generateCount, float[] intervalTime, int firstGenerateTime)
	{
		this.id = id;
		this.waveIndex = waveIndex;
		this.zombieType = zombieType;
		this.generateCount = generateCount;
		this.intervalTime = intervalTime;
		this.firstGenerateTime = firstGenerateTime;
	}	

	public ConfWaveItem Clone()
	{
		ConfWaveItem item = (ConfWaveItem)this.MemberwiseClone();
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
public class ConfWaveBase : ConfBase<ConfWaveItem>
{
    public override void Init()
    {
		confName = "Wave";
		Init1();

	}

	private void Init1()
	{
		AddItem(new ConfWaveItem(101, 1, 0, new int[]{ 2, 3, 4 }, new float[]{ 4f, 3f, 2f }, 0));
		AddItem(new ConfWaveItem(201, 2, 0, new int[]{ 2, 3, 4 }, new float[]{ 4f, 2.8f, 1.8f }, 0));
		AddItem(new ConfWaveItem(301, 3, 0, new int[]{ 2, 4, 5 }, new float[]{ 4f, 2.8f, 1.8f }, 0));
		AddItem(new ConfWaveItem(401, 4, 0, new int[]{ 3, 4, 5 }, new float[]{ 4f, 2.8f, 1.8f }, 0));
		AddItem(new ConfWaveItem(501, 5, 0, new int[]{ 2, 3, 4 }, new float[]{ 4f, 3f, 2f }, 0));
		AddItem(new ConfWaveItem(502, 5, 7, new int[]{ 1, 1, 2 }, new float[]{ 6f, 5f, 4f }, 2));
		AddItem(new ConfWaveItem(601, 6, 0, new int[]{ 2, 3, 3 }, new float[]{ 4f, 3f, 2f }, 0));
		AddItem(new ConfWaveItem(602, 6, 1, new int[]{ 1, 1, 2 }, new float[]{ 8f, 8f, 8f }, 1));
		AddItem(new ConfWaveItem(603, 6, 7, new int[]{ 1, 1, 2 }, new float[]{ 6f, 5f, 4f }, 2));
		AddItem(new ConfWaveItem(701, 7, 0, new int[]{ 2, 3, 4 }, new float[]{ 4f, 3f, 2f }, 0));
		AddItem(new ConfWaveItem(702, 7, 1, new int[]{ 1, 1, 2 }, new float[]{ 8f, 8f, 10f }, 1));
		AddItem(new ConfWaveItem(703, 7, 7, new int[]{ 1, 1, 2 }, new float[]{ 6f, 5f, 4f }, 2));
		AddItem(new ConfWaveItem(704, 7, 4, new int[]{ 1, 1, 1 }, new float[]{ 8f, 8f, 6f }, 3));
		AddItem(new ConfWaveItem(801, 8, 0, new int[]{ 2, 3, 4 }, new float[]{ 4f, 3f, 2f }, 0));
		AddItem(new ConfWaveItem(802, 8, 1, new int[]{ 1, 1, 2 }, new float[]{ 6f, 6f, 8f }, 1));
		AddItem(new ConfWaveItem(803, 8, 7, new int[]{ 1, 1, 2 }, new float[]{ 6f, 5f, 4f }, 2));
		AddItem(new ConfWaveItem(804, 8, 4, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 5f }, 2));
		AddItem(new ConfWaveItem(901, 9, 0, new int[]{ 2, 3, 4 }, new float[]{ 4f, 3f, 2f }, 0));
		AddItem(new ConfWaveItem(902, 9, 1, new int[]{ 1, 1, 2 }, new float[]{ 8f, 8f, 7f }, 1));
		AddItem(new ConfWaveItem(903, 9, 7, new int[]{ 1, 1, 2 }, new float[]{ 8f, 8f, 7f }, 2));
		AddItem(new ConfWaveItem(904, 9, 4, new int[]{ 1, 1, 1 }, new float[]{ 8f, 8f, 7f }, 2));
		AddItem(new ConfWaveItem(905, 9, 2, new int[]{ 1, 1, 1 }, new float[]{ 15f, 15f, 8f }, 8));
		AddItem(new ConfWaveItem(1001, 10, 0, new int[]{ 3, 3, 3 }, new float[]{ 4f, 4f, 3.5f }, 0));
		AddItem(new ConfWaveItem(1002, 10, 1, new int[]{ 1, 1, 2 }, new float[]{ 8f, 5f, 6f }, 1));
		AddItem(new ConfWaveItem(1003, 10, 4, new int[]{ 1, 1, 2 }, new float[]{ 8f, 5f, 6f }, 2));
		AddItem(new ConfWaveItem(1004, 10, 11, new int[]{ 1, 1, 1 }, new float[]{ 2000f, 2000f, 2000f }, 0));
		AddItem(new ConfWaveItem(1101, 11, 0, new int[]{ 2, 2, 2 }, new float[]{ 4f, 3f, 2f }, 0));
		AddItem(new ConfWaveItem(1102, 11, 1, new int[]{ 1, 1, 1 }, new float[]{ 6f, 5f, 4f }, 1));
		AddItem(new ConfWaveItem(1103, 11, 4, new int[]{ 1, 1, 1 }, new float[]{ 6f, 5f, 4f }, 1));
		AddItem(new ConfWaveItem(1104, 11, 7, new int[]{ 1, 1, 1 }, new float[]{ 6f, 5f, 4f }, 2));
		AddItem(new ConfWaveItem(1105, 11, 8, new int[]{ 1, 1, 1 }, new float[]{ 12f, 10f, 8f }, 3));
		AddItem(new ConfWaveItem(1106, 11, 9, new int[]{ 1, 1, 1 }, new float[]{ 8f, 7f, 7f }, 8));
		AddItem(new ConfWaveItem(1201, 12, 0, new int[]{ 2, 3, 4 }, new float[]{ 3.5f, 3f, 2.5f }, 0));
		AddItem(new ConfWaveItem(1202, 12, 1, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 8f }, 1));
		AddItem(new ConfWaveItem(1203, 12, 2, new int[]{ 1, 1, 1 }, new float[]{ 10f, 10f, 8f }, 8));
		AddItem(new ConfWaveItem(1204, 12, 3, new int[]{ 1, 1, 1 }, new float[]{ 10f, 10f, 8f }, 6));
		AddItem(new ConfWaveItem(1205, 12, 5, new int[]{ 1, 1, 1 }, new float[]{ 10f, 10f, 8f }, 4));
		AddItem(new ConfWaveItem(1206, 12, 6, new int[]{ 1, 1, 1 }, new float[]{ 8f, 8f, 6f }, 8));
		AddItem(new ConfWaveItem(1207, 12, 10, new int[]{ 1, 1, 1 }, new float[]{ 8f, 8f, 6f }, 8));
		AddItem(new ConfWaveItem(1301, 13, 0, new int[]{ 2, 3, 4 }, new float[]{ 3.5f, 3f, 2.5f }, 0));
		AddItem(new ConfWaveItem(1302, 13, 1, new int[]{ 1, 1, 1 }, new float[]{ 4f, 4f, 3f }, 1));
		AddItem(new ConfWaveItem(1303, 13, 2, new int[]{ 1, 1, 1 }, new float[]{ 8f, 8f, 6f }, 8));
		AddItem(new ConfWaveItem(1304, 13, 4, new int[]{ 1, 1, 1 }, new float[]{ 4f, 4f, 3f }, 6));
		AddItem(new ConfWaveItem(1305, 13, 6, new int[]{ 1, 1, 1 }, new float[]{ 5f, 5f, 4f }, 4));
		AddItem(new ConfWaveItem(1306, 13, 7, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 4f }, 6));
		AddItem(new ConfWaveItem(1307, 13, 8, new int[]{ 1, 1, 1 }, new float[]{ 10f, 8f, 6f }, 2));
		AddItem(new ConfWaveItem(1308, 13, 9, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 4f }, 0));
		AddItem(new ConfWaveItem(1309, 13, 10, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 6f }, 8));
		AddItem(new ConfWaveItem(1401, 14, 0, new int[]{ 2, 3, 4 }, new float[]{ 3.5f, 3f, 2.5f }, 0));
		AddItem(new ConfWaveItem(1402, 14, 1, new int[]{ 1, 1, 1 }, new float[]{ 4f, 4f, 3f }, 1));
		AddItem(new ConfWaveItem(1403, 14, 3, new int[]{ 1, 1, 1 }, new float[]{ 8f, 8f, 6f }, 8));
		AddItem(new ConfWaveItem(1404, 14, 4, new int[]{ 1, 1, 1 }, new float[]{ 4f, 4f, 3f }, 6));
		AddItem(new ConfWaveItem(1405, 14, 6, new int[]{ 1, 1, 1 }, new float[]{ 5f, 5f, 4f }, 4));
		AddItem(new ConfWaveItem(1406, 14, 7, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 4f }, 6));
		AddItem(new ConfWaveItem(1407, 14, 8, new int[]{ 1, 1, 1 }, new float[]{ 10f, 8f, 6f }, 2));
		AddItem(new ConfWaveItem(1408, 14, 5, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 4f }, 0));
		AddItem(new ConfWaveItem(1409, 14, 10, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 6f }, 8));
		AddItem(new ConfWaveItem(1501, 15, 0, new int[]{ 2, 2, 2 }, new float[]{ 4f, 4f, 3f }, 0));
		AddItem(new ConfWaveItem(1502, 15, 3, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 6f }, 8));
		AddItem(new ConfWaveItem(1503, 15, 6, new int[]{ 1, 1, 1 }, new float[]{ 8f, 8f, 8f }, 4));
		AddItem(new ConfWaveItem(1504, 15, 7, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 6f }, 6));
		AddItem(new ConfWaveItem(1505, 15, 9, new int[]{ 1, 1, 1 }, new float[]{ 10f, 10f, 10f }, 2));
		AddItem(new ConfWaveItem(1506, 15, 11, new int[]{ 1, 1, 1 }, new float[]{ 2000f, 2000f, 2000f }, 0));
		AddItem(new ConfWaveItem(1601, 16, 1, new int[]{ 2, 3, 4 }, new float[]{ 5f, 4.5f, 4f }, 0));
		AddItem(new ConfWaveItem(1602, 16, 2, new int[]{ 1, 1, 1 }, new float[]{ 8f, 8f, 6f }, 8));
		AddItem(new ConfWaveItem(1603, 16, 3, new int[]{ 1, 1, 1 }, new float[]{ 8f, 8f, 6f }, 8));
		AddItem(new ConfWaveItem(1604, 16, 4, new int[]{ 1, 1, 1 }, new float[]{ 4f, 4f, 3f }, 6));
		AddItem(new ConfWaveItem(1605, 16, 5, new int[]{ 1, 1, 1 }, new float[]{ 5f, 5f, 4f }, 4));
		AddItem(new ConfWaveItem(1606, 16, 6, new int[]{ 1, 1, 1 }, new float[]{ 5f, 5f, 4f }, 4));
		AddItem(new ConfWaveItem(1607, 16, 7, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 4f }, 6));
		AddItem(new ConfWaveItem(1608, 16, 8, new int[]{ 1, 1, 1 }, new float[]{ 10f, 8f, 6f }, 2));
		AddItem(new ConfWaveItem(1609, 16, 9, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 4f }, 0));
		AddItem(new ConfWaveItem(1610, 16, 10, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 6f }, 8));
		AddItem(new ConfWaveItem(1701, 17, 1, new int[]{ 3, 3, 4 }, new float[]{ 4f, 4f, 4f }, 0));
		AddItem(new ConfWaveItem(1702, 17, 2, new int[]{ 1, 1, 1 }, new float[]{ 8f, 8f, 6f }, 8));
		AddItem(new ConfWaveItem(1703, 17, 3, new int[]{ 1, 1, 1 }, new float[]{ 8f, 8f, 6f }, 8));
		AddItem(new ConfWaveItem(1704, 17, 4, new int[]{ 1, 1, 1 }, new float[]{ 4f, 4f, 3f }, 6));
		AddItem(new ConfWaveItem(1705, 17, 5, new int[]{ 1, 1, 1 }, new float[]{ 5f, 5f, 4f }, 4));
		AddItem(new ConfWaveItem(1706, 17, 6, new int[]{ 1, 1, 1 }, new float[]{ 5f, 5f, 4f }, 4));
		AddItem(new ConfWaveItem(1707, 17, 7, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 4f }, 6));
		AddItem(new ConfWaveItem(1708, 17, 8, new int[]{ 1, 1, 1 }, new float[]{ 10f, 8f, 6f }, 2));
		AddItem(new ConfWaveItem(1709, 17, 9, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 4f }, 0));
		AddItem(new ConfWaveItem(1710, 17, 10, new int[]{ 1, 1, 1 }, new float[]{ 6f, 6f, 6f }, 8));
		AddItem(new ConfWaveItem(1801, 18, 1, new int[]{ 4, 4, 4 }, new float[]{ 3f, 3f, 2f }, 0));
		AddItem(new ConfWaveItem(1802, 18, 7, new int[]{ 3, 3, 4 }, new float[]{ 3f, 3f, 2f }, 4));
		AddItem(new ConfWaveItem(1901, 19, 1, new int[]{ 4, 4, 4 }, new float[]{ 3f, 3f, 2f }, 0));
		AddItem(new ConfWaveItem(1902, 19, 7, new int[]{ 2, 2, 2 }, new float[]{ 3f, 3f, 2f }, 8));
		AddItem(new ConfWaveItem(1903, 19, 6, new int[]{ 3, 3, 3 }, new float[]{ 3f, 3f, 3f }, 2));
		AddItem(new ConfWaveItem(2001, 20, 0, new int[]{ 1, 1, 1 }, new float[]{ 3f, 3f, 3f }, 0));
		AddItem(new ConfWaveItem(2002, 20, 1, new int[]{ 1, 1, 1 }, new float[]{ 6f, 8f, 6f }, 0));
		AddItem(new ConfWaveItem(2003, 20, 2, new int[]{ 1, 1, 1 }, new float[]{ 10f, 10f, 10f }, 8));
		AddItem(new ConfWaveItem(2004, 20, 3, new int[]{ 1, 1, 1 }, new float[]{ 10f, 10f, 10f }, 16));
		AddItem(new ConfWaveItem(2005, 20, 4, new int[]{ 1, 1, 1 }, new float[]{ 10f, 10f, 10f }, 20));
		AddItem(new ConfWaveItem(2006, 20, 5, new int[]{ 1, 1, 1 }, new float[]{ 15f, 15f, 15f }, 24));
		AddItem(new ConfWaveItem(2007, 20, 6, new int[]{ 1, 1, 1 }, new float[]{ 10f, 10f, 10f }, 8));
		AddItem(new ConfWaveItem(2008, 20, 7, new int[]{ 1, 1, 1 }, new float[]{ 10f, 10f, 10f }, 16));
		AddItem(new ConfWaveItem(2009, 20, 8, new int[]{ 1, 1, 1 }, new float[]{ 10f, 10f, 10f }, 12));
		AddItem(new ConfWaveItem(2010, 20, 9, new int[]{ 1, 1, 1 }, new float[]{ 15f, 15f, 15f }, 16));
		AddItem(new ConfWaveItem(2011, 20, 10, new int[]{ 1, 1, 1 }, new float[]{ 20f, 20f, 20f }, 20));
		AddItem(new ConfWaveItem(2022, 20, 11, new int[]{ 1, 1, 1 }, new float[]{ 60f, 60f, 60f }, 45));
		AddItem(new ConfWaveItem(2033, 20, 12, new int[]{ 1, 1, 1 }, new float[]{ 99999999f, 99999999f, 99999999f }, -99999999));
	}

	public override void AddItem(ConfBaseItem item)
	{
		base.AddItem(item);
	}

	public ConfWaveItem GetItem(int id)
	{
		return GetItemObject<ConfWaveItem>(id);
	}
	
}
	
