using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfMgr
{
    public class Data
    {
		public ConfPlantCards plantCards = new ConfPlantCards();
		public ConfPropCards propCards = new ConfPropCards();
		public ConfBasicAttribute basicAttribute = new ConfBasicAttribute();
		public ConfMoneyParam moneyParam = new ConfMoneyParam();
		public ConfGrowParam growParam = new ConfGrowParam();
		public ConfLocalText localText = new ConfLocalText();
		public ConfWave wave = new ConfWave();
		public ConfWaveTimer waveTimer = new ConfWaveTimer();

    }

    public Data data = new Data();

    public System.Action onInitCall;
	
	public ConfPlantCards plantCards { get { return data.plantCards; } }		//商店配置表.xlsx
	public ConfPropCards propCards { get { return data.propCards; } }		//商店配置表.xlsx
	public ConfBasicAttribute basicAttribute { get { return data.basicAttribute; } }		//基础属性表.xlsx
	public ConfMoneyParam moneyParam { get { return data.moneyParam; } }		//掉落配置表.xlsx
	public ConfGrowParam growParam { get { return data.growParam; } }		//掉落配置表.xlsx
	public ConfLocalText localText { get { return data.localText; } }		//文本配置表.xlsx
	public ConfWave wave { get { return data.wave; } }		//波次配置表.xlsx
	public ConfWaveTimer waveTimer { get { return data.waveTimer; } }		//波次配置表.xlsx
	
	public void Init()
    {
		plantCards.Init();
		propCards.Init();
		basicAttribute.Init();
		moneyParam.Init();
		growParam.Init();
		localText.Init();
		wave.Init();
		waveTimer.Init();

		
        onInitCall?.Invoke();
	
		plantCards.OnInit();
		propCards.OnInit();
		basicAttribute.OnInit();
		moneyParam.OnInit();
		growParam.OnInit();
		localText.OnInit();
		wave.OnInit();
		waveTimer.OnInit();

    }
}
