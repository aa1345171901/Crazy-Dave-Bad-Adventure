using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class SettingPage : MonoBehaviour
{
    public Slider musicSlider;
    public Slider effectSoundSlider;
    public List<Collider2D> Buttons;
    public Toggle fullScreen;

    public Dropdown dropdown;

    public GameObject ReadMe;

    private List<int[]> resolution = new List<int[]>()
    {
        new int[2]{1920, 1080 },
        new int[2]{1680, 1050 },
        new int[2]{1600, 1200 },
        new int[2]{1440, 900 },
        new int[2]{1366, 768 },
        new int[2]{1280, 1024 },
        new int[2]{1280, 960 },
        new int[2]{1024, 768 },
        new int[2]{800, 600 },
    };

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.BackmusicPlayer.volume;
        effectSoundSlider.value = AudioManager.Instance.EffectPlayer.volume;
        fullScreen.isOn = Screen.fullScreen;
        for (int i = 0; i < resolution.Count; i++)
        {
            if (Screen.width == resolution[i][0])
            {
                dropdown.value = i;
            }
        }
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

    public void Return()
    {
        this.gameObject.SetActive(false);
        AudioManager.Instance.SaveVolumeData();
        Time.timeScale = 1;
        foreach (var item in Buttons)
        {
            item.enabled = true;
        }
    }

    public void FullScreen(bool isValue)
    {
        if (isValue)
            Screen.fullScreen = true;
        else
            Screen.fullScreen = false;
    }

    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0:
                QualitySettings.SetQualityLevel(5, true);
                break;
            case 1:
            case 2:
                QualitySettings.SetQualityLevel(4, true);
                break;
            case 3:
            case 4:
                QualitySettings.SetQualityLevel(3, true);
                break;
            case 5:
            case 6:
                QualitySettings.SetQualityLevel(2, true);
                break;
            case 7:
                QualitySettings.SetQualityLevel(1, true);
                break;
            case 8:
                QualitySettings.SetQualityLevel(0, true);
                break;
            default:
                break;
        }
        Screen.SetResolution(resolution[index][0], resolution[index][1], fullScreen.isOn);
    }

    public void OpenRead()
    {
        ReadMe.gameObject.SetActive(true);
    }

    public void CloseRead()
    {
        ReadMe.gameObject.SetActive(false);
    }
}
