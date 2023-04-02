using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ʵ�������ͣ����
/// </summary>
public class ShopItem : MonoBehaviour
{
    [Header("��Ʒ������Ϣ")]
    public string Info;
    public int Price;
    public Text PriceText;

    private Button button;
    public bool isDown { get; protected set; }  // �ж�����Ƿ��ڰ�ť��
    private Camera UICamera;
    private RectTransform rectTransform;

    public Action CannotAfford;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        UICamera = UIManager.Instance.UICamera;
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // �ж�����Ƿ��ڰ�ť��Χ��
        if (BoundsUtils.GetSceneRect(UICamera, rectTransform).Contains(Input.mousePosition))
        {
            if (!isDown)
            {
                isDown = true;
                AudioManager.Instance.PlayEffectSoundByName("shopItem");
            }
        }
        else
        {
            isDown = false;
        }
    }

    protected virtual void OnClick()
    {
        if (ShopManager.Instance.Money >= Price)
        {
            this.isDown = false;
            this.gameObject.SetActive(false);
        }
        else
        {
            CannotAfford?.Invoke();
        }
    }

    public virtual void SetInfo()
    {

    }

    public virtual void UpdateMoney()
    {
        if (ShopManager.Instance.Money < Price)
            this.PriceText.color = Color.red;
        else
            this.PriceText.color = new Color(0.2f, 0.2f, 0.2f);
    }
}
