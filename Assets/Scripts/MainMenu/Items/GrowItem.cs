using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GrowItem : MonoBehaviour
{
    public Text growName;
    public Image img;
    public Transform levelContent;
    public GameObject levelItem;

    Action<string, GrowItem> onClick;

    ConfExternlGrowItem confItem;

    public void InitData(ConfExternlGrowItem confItem, Action<string, GrowItem> onClick)
    {
        this.confItem = confItem;
        this.onClick = onClick;
        string name = GameTool.LocalText(confItem.name);
        growName.text = name;
        Sprite sprite = Resources.Load<Sprite>(confItem.imgPath);
        img.sprite = sprite;
        for (int i = 0; i < confItem.cost.Length; i++)
        {
            var newLevelItem = GameObject.Instantiate(levelItem, levelContent);
            newLevelItem.gameObject.SetActive(true);
        }
        var level = SaveManager.Instance.externalGrowthData.GetLevelByKey(confItem.key);
        UpdateLevel(level);
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        onClick?.Invoke(confItem.key, this);
    }

    public void UpdateLevel(int level, bool isLevelUp = false)
    {
        for (int i = 1; i < levelContent.childCount; i++)
        {
            var check = level > i - 1;
            var checkGo = levelContent.GetChild(i).GetChild(0).gameObject;
            checkGo.SetActive(check);
            if (level == i)
            {
                StartCoroutine(CheckAnim(checkGo));
            }
        }
    }

    IEnumerator CheckAnim(GameObject check)
    {
        float scale = 2;
        while (scale > 1f)
        {
            scale -= 0.01f;
            check.transform.localScale = Vector3.one * scale;
            yield return new WaitForEndOfFrame();
        }
    }
}
