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

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        this.transform.SetSiblingIndex(this.transform.parent.childCount - 1);  // 设置最后一个渲染
        Time.timeScale = 0;
        AudioManager.Instance.PlayEffectSoundByName("pause");
        AudioManager.Instance.PlayMenuMusic(0.2f);
        musicSlider.value = AudioManager.Instance.BackmusicPlayer.volume;
        effectSoundSlider.value = AudioManager.Instance.EffectPlayer.volume;
        UIManager.Instance.PushPanel(UIPanelType.AttributePanel);
        BagPanel bagpanel = UIManager.Instance.PushPanel(UIPanelType.BagPanel) as BagPanel;
        bagpanel.AutoClose = false;
        hudToggle.isOn = SaveManager.Instance.SystemData.IsHUD;
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
        foreach (var item in AudioManager.Instance.AudioLists)
        {
            item.volume = value;
        }
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

    public void SetHUD(bool value)
    {
        SaveManager.Instance.SystemData.IsHUD = value;
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
            UIManager.Instance.PopPanel();
            UIManager.Instance.PopPanel();
        }
    }
}
