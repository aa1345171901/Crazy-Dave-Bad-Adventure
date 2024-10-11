using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieIllustrationsItem : MonoBehaviour
{
    public Image zombieImg;

    ConfZombieIllustrationsItem confItem;

    Button button;
    Action<ConfZombieIllustrationsItem> onClick;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => 
        { 
            onClick?.Invoke(confItem); 
        });
    }

    public void InitData(ConfZombieIllustrationsItem confItem, Action<ConfZombieIllustrationsItem> call)
    {
        this.confItem = confItem;
        this.onClick = call;

        var path = SaveManager.Instance.externalGrowthData.GetZombieCount(confItem.zombieType) > 0 ? confItem.zombieImagePath : confItem.zombieImagePath + "_1";
        Sprite plantImage = Resources.Load<Sprite>(path);
        this.zombieImg.sprite = plantImage;
    }
}
