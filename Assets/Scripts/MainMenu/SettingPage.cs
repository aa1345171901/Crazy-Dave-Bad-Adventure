using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class SettingPage : MonoBehaviour
{
    public Slider musicSlider;
    public Slider effectSoundSlider;
    public Toggle fullScreen;

    public Dropdown dropdown;
    public Dropdown dropdownLanguage;
    public GameObject btnKeyChange;

    public GameObject ReadMe;

    public GameObject KeyChangePage;

    public KeyChangeItem nowKeyChangeItem;
    public GameObject anyKeyDownPage;
    public GameObject keyChangeErrorPage;

    public List<KeyChangeItem> keyChangeItems;

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

    private List<string> languages = new List<string>()
    {
        "简体中文",
        "English",
    };
    private List<string> languagePrex = new List<string>()
    {
        "cn",
        "en",
    };

    private void Start()
    {
#if UNITY_ANDROID
        dropdown.gameObject.SetActive(false);
        fullScreen.transform.parent.gameObject.SetActive(false);
        btnKeyChange.SetActive(false);
#endif
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

        foreach (var item in languages)
        {
            dropdownLanguage.options.Add(new Dropdown.OptionData(item));
        }
        dropdownLanguage.value = languagePrex.IndexOf(ConfManager.Instance.language);
        dropdownLanguage.onValueChanged.AddListener(ChangeLanguage);
    }

    void ChangeLanguage(int index)
    {
        ConfManager.Instance.language = languagePrex[index];
        ConfManager.Instance.languageChange?.Invoke();
        SaveManager.Instance.systemData.language = languagePrex[index];
        SaveManager.Instance.SaveSystemData();
    }

    private void OnGUI()
    {
        if (anyKeyDownPage.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                var keyCode = Event.current.keyCode;
                if (keyCode != KeyCode.None)
                {
                    nowKeyChangeItem.nowKey.text = keyCode.ToString();
                    anyKeyDownPage.SetActive(false);
                }
            }
        }
        if (keyChangeErrorPage.activeSelf)
        {
            if (Input.anyKeyDown)
                keyChangeErrorPage.SetActive(false);
        }
    }

    public void MusicVolumeChanged(float value)
    {
        AudioManager.Instance.ChangeMusicVolume(value);
    }

    public void EffectVolumeChanged(float value)
    {
        AudioManager.Instance.ChangeEffectVolume(value);
    }

    public void Return()
    {
        this.gameObject.SetActive(false);
        AudioManager.Instance.SaveVolumeData();
        Time.timeScale = 1;
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
        ReadMe.SetActive(true);
    }

    public void CloseRead()
    {
        ReadMe.SetActive(false);
    }

    public void OpenKeyPage()
    {
        KeyChangePage.SetActive(true);
        foreach (var item in keyChangeItems)
        {
            item.nowKey.text = "";
            item.SetKeyText();
        }
    }

    public void CloseKeyPage()
    {
        if (JudgeKey())
        {
            foreach (var item in keyChangeItems)
            {
                switch (item.keyEnum)
                {
                    case KeyEnum.Key:
                        if (!string.IsNullOrEmpty(item.nowKey.text))
                            InputManager.SetKey(item.KeyName, (KeyCode)Enum.Parse(typeof(KeyCode), item.nowKey.text));
                        break;
                    case KeyEnum.Axis:
                        if (!string.IsNullOrEmpty(item.nowKey.text))
                            InputManager.SetAxisKey(item.KeyName, (KeyCode)Enum.Parse(typeof(KeyCode), item.nowKey.text), item.isMin);
                        break;
                    case KeyEnum.KeyValue:
                        if (!string.IsNullOrEmpty(item.nowKey.text))
                            InputManager.SetValueKey(item.KeyName, (KeyCode)Enum.Parse(typeof(KeyCode), item.nowKey.text));
                        break;
                    default:
                        break;
                }
            }
            KeyChangePage.SetActive(false);
        }
        else
        {
            keyChangeErrorPage.SetActive(true);
        }
    }

    private bool JudgeKey()
    {
        bool result = true;
        HashSet<string> hashset = new HashSet<string>();
        foreach (var item in keyChangeItems)
        {
            if (!string.IsNullOrEmpty(item.nowKey.text))
            {
                if (!hashset.Contains(item.nowKey.text))
                {
                    hashset.Add(item.nowKey.text);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if (!hashset.Contains(item.lastKey.text))
                {
                    hashset.Add(item.lastKey.text);
                }
                else
                {
                    result = false;
                }
            }
        }
        return result;
    }

    public void ChangeKeyItemClick(KeyChangeItem keyChangeItem)
    {
        nowKeyChangeItem = keyChangeItem;
        anyKeyDownPage.SetActive(true);
    }
}
