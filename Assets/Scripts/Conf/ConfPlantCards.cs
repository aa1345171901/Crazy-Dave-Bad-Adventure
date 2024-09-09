using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfPlantCards : ConfPlantCardsBase
{
    public List<PlantCard> PlantCards = new List<PlantCard>();

    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            var platCard = new PlantCard();
            platCard.plantName = item.plantName;
            platCard.plantBgImagePath = item.plantBgImagePath;
            platCard.plantImagePath = item.plantImagePath;
            platCard.defaultPrice = item.defaultPrice;
            platCard.defaultSun = item.defaultSun;
            platCard.info = item.info;
            platCard.plantType = (PlantType)item.plantType;
            PlantCards.Add(platCard);
        }
    }
}
