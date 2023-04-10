using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image Bg;
    public Image Plant;
    public Text Sun;

    public PlantAttribute PlantAttribute;

    public void SetPlant(PlantAttribute plantAttribute)
    {
        this.PlantAttribute = plantAttribute;

        Sprite bg = Resources.Load<Sprite>(plantAttribute.plantCard.plantBgImagePath);
        this.Bg.sprite = bg;

        Sprite plantImage = Resources.Load<Sprite>(plantAttribute.plantCard.plantImagePath);
        this.Plant.sprite = plantImage;

        this.Sun.text = plantAttribute.plantCard.defaultSun.ToString();

        Bg.enabled = true;
        Plant.enabled = true;
        Sun.enabled = true;
    }
}
