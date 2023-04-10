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
        for (int i = 0; i < GardenManager.Instance.MaxSlot; i++)
        {
            var card = GameObject.Instantiate(PlantCardItem, content);
            card.Bg.enabled = false;
            card.Plant.enabled = false;
            card.Sun.enabled = false;
            Cards.Add(card);
        }
    }

    public void SetCard()
    {
        int index = 0;
        foreach (var item in GardenManager.Instance.CardslotPlant)
        {
            Cards[index].SetPlant(item);
            index++;
        }
        for (int i = index; i < GardenManager.Instance.MaxSlot; i++)
        {
            Cards[i].Bg.enabled = false;
            Cards[i].Plant.enabled = false;
            Cards[i].Sun.enabled = false;
        }
    }
}
