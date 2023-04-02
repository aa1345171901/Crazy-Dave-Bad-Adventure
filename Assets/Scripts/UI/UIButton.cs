using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [Tooltip("����ʱʹ�õĲ���")]
    public Material glowMaterial;

    [Tooltip("���°�ťʱƫ��")]
    public Vector3 offset;

    [Header("Event")]
    [Space(10)]
    public UnityEvent OnClick;

    private Image image;
    private Vector3 pressedPos;
    private Vector3 defaultPos;

    private Camera UICamera;
    private RectTransform rectTransform;

    private void Start()
    {
        image = this.GetComponent<Image>();
        pressedPos = this.transform.position + offset;
        defaultPos = this.transform.position;
        UICamera = UIManager.Instance.UICamera;
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnMouseDown()
    {
        this.transform.position = pressedPos;
        AudioManager.Instance.PlayEffectSoundByName("btnPressed");
    }

    private void OnMouseUp()
    {
        image.material = null;
        // �ж�����Ƿ��ڰ�ť��Χ��
        if (BoundsUtils.GetSceneRect(UICamera, rectTransform).Contains(Input.mousePosition))
        {
            OnClick?.Invoke();
        }
        this.transform.position = defaultPos;
    }

    private void OnMouseEnter()
    {
        image.material = glowMaterial;
        AudioManager.Instance.PlayEffectSoundByName("btnHighlight");
    }

    private void OnMouseExit()
    {
        image.material = null;
        this.transform.position = defaultPos;
    }
}
