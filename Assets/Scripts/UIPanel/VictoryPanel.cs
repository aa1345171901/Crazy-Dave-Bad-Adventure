using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryPanel : BasePanel
{
    DamageStatisticsPanel damageStatisticsPanel;

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        this.transform.SetSiblingIndex(this.transform.parent.childCount - 1);  // 设置最后一个渲染
        AudioManager.Instance.PlayMenuMusic(0.2f);
        UIManager.Instance.PushPanel(UIPanelType.AttributePanel);
        BagPanel bagpanel = UIManager.Instance.PushPanel(UIPanelType.BagPanel) as BagPanel;
        bagpanel.AutoClose = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
        AudioManager.Instance.SaveVolumeData();  // 只在退出界面时保存音量数据
        AudioManager.Instance.ResumeMusic();
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(1);
        SaveManager.Instance.DeleteUserData();
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenDamageStatistics()
    {
        if (damageStatisticsPanel == null || !damageStatisticsPanel.gameObject.activeSelf)
        {
            damageStatisticsPanel = UIManager.Instance.PushPanel(UIPanelType.DamageStatisticsPanel) as DamageStatisticsPanel;
        }
        else
        {
            UIManager.Instance.PopPanel();
        }
    }
}
