using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfMgr
{
    public class Data
    {
		public ConfPlantCards plantCards = new ConfPlantCards();
		public ConfPropCards propCards = new ConfPropCards();

    }

    public Data data = new Data();

    public System.Action onInitCall;
	
	public ConfPlantCards plantCards { get { return data.plantCards; } }		//商店配置表.xlsx
	public ConfPropCards propCards { get { return data.propCards; } }		//商店配置表.xlsx
	
	public void Init()
    {
		plantCards.Init();
		propCards.Init();

		
        onInitCall?.Invoke();
	
		plantCards.OnInit();
		propCards.OnInit();

    }
}
