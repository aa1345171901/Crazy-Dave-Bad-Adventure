using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCardPage : MonoBehaviour
{
    public Card PlantCardItem;
    public Transform content;

    public List<Card> Cards;

    [Tooltip("卡片是否在花园中")]
    public bool IsGarden;

    public Animator animator;
    private bool isExpand;
    private Camera UICamera;
    private RectTransform rectTransform;
    private Rect bounds;
    private bool isShowing;

    private void Start()
    {
        CreateCard();
        UICamera = UIManager.Instance.UICamera;
        rectTransform = GetComponent<RectTransform>();
        bounds = BoundsUtils.GetSceneRect(UICamera, rectTransform);
        bounds.y -= bounds.height * 3 / 4;
    }

    private void Update()
    {
        if (!isShowing)
        {
            // 判断鼠标是否在按钮范围内
            if (bounds.Contains(Input.mousePosition) && !isExpand && Time.timeScale != 0)
            {
                isShowing = true;
                animator.SetTrigger("Enter");
            }
            if (isExpand && !bounds.Contains(Input.mousePosition))
            {
                isShowing = true;
                animator.SetTrigger("Exit");
            }
        }
    }

    private void SetExpandTrue()
    {
        this.isExpand = true;
        isShowing = false;
    }

    private void SetExpandFalse()
    {
        this.isExpand = false;
        isShowing = false;
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
            Invoke("DelaySetBounds", 0.1f);
        }
    }

    private void DelaySetBounds()
    {
        bounds = BoundsUtils.GetSceneRect(UICamera, rectTransform);
        bounds.y -= bounds.height * 3 / 4;
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
