using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfPropCards : ConfPropCardsBase
{
    public List<PropCard> PropCards = new List<PropCard>();
    public Dictionary<ConfPropCardsItem, PropCard> maxNumLimit = new Dictionary<ConfPropCardsItem, PropCard>();
    public Dictionary<ConfPropCardsItem, PropCard> frontLimit = new Dictionary<ConfPropCardsItem, PropCard>();
    Dictionary<string, ConfPropCardsItem> dicts = new Dictionary<string, ConfPropCardsItem>();
    Dictionary<int, List<ConfPropCardsItem>> typeDicts = new Dictionary<int, List<ConfPropCardsItem>>();

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
            if (!string.IsNullOrEmpty(item.frontProp))
            {
                frontLimit[item] = propCard;
            }
            if (!typeDicts.ContainsKey(item.propType))
            {
                typeDicts[item.propType] = new List<ConfPropCardsItem>();
            }
            typeDicts[item.propType].Add(item);

            dicts[item.propName] = item;
        }
    }

    public ConfPropCardsItem GetItemByName(string name)
    {
        if (dicts.ContainsKey(name))
            return dicts[name];
        return null;
    }

    public ConfPropCardsItem GetItemByTypeLevel(int propType, int level)
    {
        if (typeDicts.ContainsKey(propType))
        {
            foreach (var item in typeDicts[propType])
            {
                if (item.quality == level)
                    return item;
            }
        }
        return null;
    }
}
