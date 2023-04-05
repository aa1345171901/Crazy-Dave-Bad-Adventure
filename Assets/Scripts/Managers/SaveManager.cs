using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SaveManager();
            return _instance;
        }
    }

    public SoundVolumeData SoundVolumeData { get; protected set; } = new SoundVolumeData();

    private SaveManager()
    {
        LoadData();
    }

    private void LoadData()
    {
        string soundsVolumeDataStr = PlayerPrefs.GetString("SoundVolumeData");
        if (!string.IsNullOrEmpty(soundsVolumeDataStr))
        {
            SoundVolumeData = JsonUtility.FromJson<SoundVolumeData>(soundsVolumeDataStr);
        }
    }

    public void SaveVolumeData()
    {
        PlayerPrefs.SetString("SoundVolumeData", JsonUtility.ToJson(SoundVolumeData));
    }
}
