using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowItem : MonoBehaviour
{
    public Text growName;
    public Image img;
    public Transform levelContent;
    public GameObject levelItem;

    public void InitData(ConfExternlGrowItem confItem)
    {
        string name = GameTool.LocalText(confItem.name);
        growName.text = name;
        Sprite sprite = Resources.Load<Sprite>(confItem.imgPath);
        img.sprite = sprite;
        for (int i = 0; i < confItem.cost.Length; i++)
        {
            var newLevelItem = GameObject.Instantiate(levelItem, levelContent);
            newLevelItem.SetActive(true);
        }
    }
}
