using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class PropBagItem : MonoBehaviour
{
    public Image image;
    public Text CountText;

    private int count;
    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            count = value;
            CountText.text = count.ToString();
        }
    }

    public BagPanel BagPanel { get; set; }
    public PropCard PropCard { get; set; }

    private void OnMouseEnter()
    {
        AudioManager.Instance.PlayEffectSoundByName("shopItem");
        BagPanel.NowShowPropCard = PropCard;
    }

    private void OnMouseUp()
    {
        if (BagPanel.AutoClose)
        {
            AudioManager.Instance.PlayEffectSoundByName("btnPressed");
            BagPanel.sellPage.SetActive(true);
            BagPanel.sellPage.transform.position = this.transform.position;
            BagPanel.sellAll.gameObject.SetActive(count > 1);
            int nowPrice = Mathf.RoundToInt(PropCard.GetNowPrice() * ConfManager.Instance.confMgr.gameIntParam.GetItemByKey("sellRate").value / 100f);
            BagPanel.sellOneText.text = nowPrice.ToString();
            BagPanel.sellAllText.text = (nowPrice * count).ToString();
            BagPanel.sellProp = PropCard;
        }
    }

    private void OnMouseExit()
    {
        BagPanel.NowShowPropCard = null;
    }

    public void SetPropCard(PropCard propCard)
    {
        this.PropCard = propCard;
        Sprite image = Resources.Load<Sprite>(propCard.propImagePath);
        this.image.sprite = image;
    }
}
