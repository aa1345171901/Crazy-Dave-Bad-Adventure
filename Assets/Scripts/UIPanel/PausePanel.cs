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
        UIManager.Instance.PopPanel();
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
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
