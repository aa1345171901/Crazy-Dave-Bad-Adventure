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
