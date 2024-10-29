using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfMgr
{
    public class Data
    {
		public ConfOtherGameModes otherGameModes = new ConfOtherGameModes();
		public ConfPlantCards plantCards = new ConfPlantCards();
		public ConfPropCards propCards = new ConfPropCards();
		public ConfPlantIllustrations plantIllustrations = new ConfPlantIllustrations();
		public ConfZombieIllustrations zombieIllustrations = new ConfZombieIllustrations();
		public ConfBasicAttribute basicAttribute = new ConfBasicAttribute();
		public ConfExternlGrow externlGrow = new ConfExternlGrow();
		public ConfAchievement achievement = new ConfAchievement();
		public ConfMoneyParam moneyParam = new ConfMoneyParam();
		public ConfGrowParam growParam = new ConfGrowParam();
		public ConfSeedWeight seedWeight = new ConfSeedWeight();
		public ConfLocalText localText = new ConfLocalText();
		public ConfGameIntParam gameIntParam = new ConfGameIntParam();
		public ConfWave wave = new ConfWave();
		public ConfWaveTimer waveTimer = new ConfWaveTimer();

    }

    public Data data = new Data();

    public System.Action onInitCall;
	
	public ConfOtherGameModes otherGameModes { get { return data.otherGameModes; } }		//其他游戏模式配置表.xlsx
	public ConfPlantCards plantCards { get { return data.plantCards; } }		//商店配置表.xlsx
	public ConfPropCards propCards { get { return data.propCards; } }		//商店配置表.xlsx
	public ConfPlantIllustrations plantIllustrations { get { return data.plantIllustrations; } }		//图鉴配置表.xlsx
	public ConfZombieIllustrations zombieIllustrations { get { return data.zombieIllustrations; } }		//图鉴配置表.xlsx
	public ConfBasicAttribute basicAttribute { get { return data.basicAttribute; } }		//基础属性表.xlsx
	public ConfExternlGrow externlGrow { get { return data.externlGrow; } }		//局外养成配置表.xlsx
	public ConfAchievement achievement { get { return data.achievement; } }		//成就配置表.xlsx
	public ConfMoneyParam moneyParam { get { return data.moneyParam; } }		//掉落配置表.xlsx
	public ConfGrowParam growParam { get { return data.growParam; } }		//掉落配置表.xlsx
	public ConfSeedWeight seedWeight { get { return data.seedWeight; } }		//掉落配置表.xlsx
	public ConfLocalText localText { get { return data.localText; } }		//文本配置表.xlsx
	public ConfGameIntParam gameIntParam { get { return data.gameIntParam; } }		//杂项配置表.xlsx
	public ConfWave wave { get { return data.wave; } }		//波次配置表.xlsx
	public ConfWaveTimer waveTimer { get { return data.waveTimer; } }		//波次配置表.xlsx
	
	public void Init()
    {
		otherGameModes.Init();
		plantCards.Init();
		propCards.Init();
		plantIllustrations.Init();
		zombieIllustrations.Init();
		basicAttribute.Init();
		externlGrow.Init();
		achievement.Init();
		moneyParam.Init();
		growParam.Init();
		seedWeight.Init();
		localText.Init();
		gameIntParam.Init();
		wave.Init();
		waveTimer.Init();

		
        onInitCall?.Invoke();
	
		otherGameModes.OnInit();
		plantCards.OnInit();
		propCards.OnInit();
		plantIllustrations.OnInit();
		zombieIllustrations.OnInit();
		basicAttribute.OnInit();
		externlGrow.OnInit();
		achievement.OnInit();
		moneyParam.OnInit();
		growParam.OnInit();
		seedWeight.OnInit();
		localText.OnInit();
		gameIntParam.OnInit();
		wave.OnInit();
		waveTimer.OnInit();

    }
}
