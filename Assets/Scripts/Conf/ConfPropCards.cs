using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfPropCards : ConfPropCardsBase
{
    public List<PropCard> PropCards = new List<PropCard>();
    public Dictionary<ConfPropCardsItem, PropCard> maxNumLimit = new Dictionary<ConfPropCardsItem, PropCard>();

    public override void OnInit()
    {
        base.OnInit();
        foreach (var item in items)
        {
            var propCard = new PropCard();
            propCard.propName = item.propName;
            propCard.propImagePath = item.propImagePath;
            propCard.defaultPrice = item.defaultPrice;
            propCard.info = item.info;
            propCard.quality = item.quality;
            propCard.attributes = new List<AttributeIncrement>();
            if (!string.IsNullOrEmpty(item.attributes))
            {
                JArray json = JArray.Parse(item.attributes);
                foreach (var attr in json)
                {
                    var attributeIncrement = new AttributeIncrement();
                    var jo = JObject.Parse(attr.ToString());
                    AttributeType type = (AttributeType)System.Enum.Parse(typeof(AttributeType), jo["attributeTypeString"].ToString());
                    attributeIncrement.attributeType = type;
                    attributeIncrement.increment = int.Parse(jo["increment"].ToString());
                    propCard.attributes.Add(attributeIncrement);
                }
            }
            propCard.propType = (PropType)item.propType;
            propCard.value1 = item.value1;
            propCard.coolingTime = item.coolingTime;
            PropCards.Add(propCard);

            if (item.maxNum != 0)
            {
                maxNumLimit[item] = propCard;
            }
        }
    }
}
