using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCardPage : MonoBehaviour
{
    public Card PlantCardItem;
    public Transform content;

    public List<Card> Cards;

    public Animator animator;
    private bool isExpand;

    [Tooltip("卡片是否在花园中")]
    public bool IsGarden;

    private void Start()
    {
        CreateCard();
    }

    private void OnMouseEnter()
    {
        if (!isExpand && Time.timeScale != 0)
        {
            animator.SetTrigger("Enter");
        }
    }

    private void OnMouseExit()
    {
        if (isExpand)
        {
            animator.SetTrigger("Exit");
        }
    }

    private void SetExpandTrue()
    {
        this.isExpand = true;
    }

    private void SetExpandFalse()
    {
        this.isExpand = false;
    }

    public void CreateCard()
    {
        if (Cards.Count < GardenManager.Instance.MaxSlot)
        {
            int len = GardenManager.Instance.MaxSlot - Cards.Count;
            for (int i = 0; i < len; i++)
            {
                var card = GameObject.Instantiate(PlantCardItem, content);
                card.UnSetPlant();
                Cards.Add(card);
            }
            SetCard();
        }
    }

    public void SetCard()
    {
        CreateCard();
        int index = 0;
        foreach (var item in GardenManager.Instance.CardslotPlant)
        {
            Cards[index].SetPlant(item, IsGarden);
            index++;
        }
        for (int i = index; i < GardenManager.Instance.MaxSlot; i++)
        {
            Cards[i].UnSetPlant();
        }
    }
}
