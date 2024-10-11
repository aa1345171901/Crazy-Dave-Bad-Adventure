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

        Sprite plantImage = Resources.Load<Sprite>(confItem.zombieImagePath);
        this.zombieImg.sprite = plantImage;
    }
}
