using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopDownPlate;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DamageStatisticsPanel : BasePanel
{
    public Transform damageStatisticsRoot;
    public DamageShowItem damageStatisticsItem;
    public Transform takingDamageStatisticsRoot;
    public DamageShowItem takingDamageStatisticsItem;

    Dictionary<int, DamageShowItem> damageStatisticsDict = new Dictionary<int, DamageShowItem>();
    Dictionary<int, DamageShowItem> takingDamageStatisticsDict = new Dictionary<int, DamageShowItem>();

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        UpdateUI();
    }

    void UpdateUI()
    {
        UpdateItem(SaveManager.Instance.damageStatistics, damageStatisticsDict, false, damageStatisticsRoot, damageStatisticsItem);
        UpdateItem(SaveManager.Instance.takingDamageStatistics, takingDamageStatisticsDict, true, takingDamageStatisticsRoot, takingDamageStatisticsItem);
    }

    void UpdateItem(List<TypeIntData> damageList, Dictionary<int, DamageShowItem> damageDict, bool isTaking, Transform root, DamageShowItem damageShowItem)
    {
        if (damageList == null)
            return;
        int sum = damageList.Sum((e) => e.value);
        foreach (var item in damageList)
        {
            DamageShowItem newDamageItem;
            if (damageDict.ContainsKey(item.key))
            {
                newDamageItem = damageDict[item.key];
            }
            else
            {
                newDamageItem = GameObject.Instantiate(damageShowItem, root);
                newDamageItem.gameObject.SetActive(true);
                newDamageItem.InitData(isTaking, item.key, item.value, sum);
                damageDict[item.key] = newDamageItem;
            }
            newDamageItem.UpdateUI(item.value, sum);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }
}
