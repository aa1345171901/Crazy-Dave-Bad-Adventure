using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : BasePanel
{
    public Slider musicSlider;
    public Slider effectSoundSlider;
    public Toggle hudToggle;

    public GameObject mainMenuPage;
    public GameObject restartPage;

    DamageStatisticsPanel damageStatisticsPanel;

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        this.transform.SetSiblingIndex(this.transform.parent.childCount - 1);  // 设置最后一个渲染
        Time.timeScale = 0;
        AudioManager.Instance.PlayEffectSoundByName("pause");
        AudioManager.Instance.PlayMenuMusic(0.2f);
        musicSlider.value = SaveManager.Instance.systemData.MusicVolume;
        effectSoundSlider.value = SaveManager.Instance.systemData.SoundEffectVolume;
        UIManager.Instance.PushPanel(UIPanelType.AttributePanel);
        BagPanel bagpanel = UIManager.Instance.PushPanel(UIPanelType.BagPanel) as BagPanel;
        bagpanel.AutoClose = false;
        hudToggle.isOn = SaveManager.Instance.systemData.IsHUD;
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
        AudioManager.Instance.SaveVolumeData();  // 只在退出界面时保存音量数据
        Time.timeScale = 1;
        AudioManager.Instance.ResumeMusic();
    }

    public void ReturnGame()
    {
        Close();
    }

    public void MusicVolumeChanged(float value)
    {
        AudioManager.Instance.ChangeMusicVolume(value);
    }

    public void EffectVolumeChanged(float value)
    {
        AudioManager.Instance.ChangeEffectVolume(value);
    }

    public void ReStartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        SaveManager.Instance.DeleteUserData();
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1;
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

    public void SetHUD(bool value)
    {
        SaveManager.Instance.systemData.IsHUD = value;
    }

    public void Close()
    {
        if (mainMenuPage.activeSelf || restartPage.activeSelf)
        {
            mainMenuPage.SetActive(false);
            restartPage.SetActive(false);
        }
        else
        {
            if (damageStatisticsPanel != null && damageStatisticsPanel.gameObject.activeSelf)
                UIManager.Instance.PopPanel();
            UIManager.Instance.PopPanel();
            UIManager.Instance.PopPanel();
        }
    }
}
