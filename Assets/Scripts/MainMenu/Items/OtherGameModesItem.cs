using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OtherGameModesItem : MonoBehaviour
{
    public Image img;
    public GameObject have;
    public Text nameText;

    Button button;

    ConfOtherGameModesItem confItem;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnEnterGameModes);
    }

    public void InitData(ConfOtherGameModesItem confItem)
    {
        this.confItem = confItem;

        Sprite bg = Resources.Load<Sprite>(confItem.imgPath);
        img.sprite = bg;

        nameText.text = GameTool.LocalText(confItem.name);
    }

    void OnEnterGameModes()
    {
        SaveManager.Instance.SetSpecialMode((BattleMode)confItem.type);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
}
