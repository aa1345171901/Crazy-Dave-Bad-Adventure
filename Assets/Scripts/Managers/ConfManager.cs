using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ConfManager : BaseManager<ConfManager>
{
    public ConfMgr confMgr { get; private set; }

    public string language { get; set; }

    public Action languageChange { get; set; }

    /// <summary>
    /// 最大生命值
    /// </summary>
    public string MaximumHP = GameTool.LocalText("property_MaximumHP");

    /// <summary>
    /// 生命恢复
    /// </summary>
    public string LifeRecovery = GameTool.LocalText("property_LifeRecovery");

    /// <summary>
    /// 肾上腺素
    /// </summary>
    public string Adrenaline = GameTool.LocalText("property_Adrenaline");

    /// <summary>
    /// 力量
    /// </summary>
    public string Power = GameTool.LocalText("property_Power");

    /// <summary>
    /// 伤害
    /// </summary>
    public string PercentageDamage = GameTool.LocalText("property_PercentageDamage");

    /// <summary>
    /// 攻击速度
    /// </summary>
    public string AttackSpeed = GameTool.LocalText("property_AttackSpeed");

    /// <summary>
    /// 范围
    /// </summary>
    public string Range = GameTool.LocalText("property_Range");

    /// <summary>
    /// 暴击率
    /// </summary>
    public string CriticalHitRate = GameTool.LocalText("property_CriticalHitRate");

    /// <summary>
    /// 移动速度
    /// </summary>
    public string Speed = GameTool.LocalText("property_Speed");

    /// <summary>
    /// 护甲
    /// </summary>
    public string Armor = GameTool.LocalText("property_Armor");

    /// <summary>
    /// 幸运
    /// </summary>
    public string Lucky = GameTool.LocalText("property_Lucky");

    /// <summary>
    /// 阳光
    /// </summary>
    public string Sunshine = GameTool.LocalText("property_Sunshine");

    /// <summary>
    /// 金币
    /// </summary>
    public string GoldCoins = GameTool.LocalText("property_GoldCoins");

    /// <summary>
    /// 植物学
    /// </summary>
    public string Botany = GameTool.LocalText("property_Botany");

    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrEmpty(SaveManager.Instance.systemData.language))
            language = SaveManager.Instance.systemData.language;
        else
            language = (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified || Application.systemLanguage == SystemLanguage.ChineseTraditional) ? "cn" : "en";

        DontDestroyOnLoad(this);
        confMgr = new ConfMgr();
        confMgr.Init();
    }
}
