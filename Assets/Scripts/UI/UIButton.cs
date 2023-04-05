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
    [Tooltip("��̬ʱʹ�õĲ���")]
    public Material normalMaterial;
    [Tooltip("�ı���ʵ�Ŀ��UI")]
    public MaskableGraphic graphic;

    [Tooltip("���°�ťʱƫ��")]
    public Vector3 offset;

    [Tooltip("���°�ťʱ���ŵ���Ч��")]
    public string effectSoundsPressed = "btnPressed";
    [Tooltip("����ʱ���ŵ���Ч��")]
    public string effectSoundsHighlight = "btnHighlight";

    [Header("Event")]
    [Space(10)]
    public UnityEvent OnClick;
    private Vector3 pressedPos;
    private Vector3 defaultPos;

    private Camera UICamera;
    private RectTransform rectTransform;

    private void Start()
    {
        pressedPos = this.transform.position + offset;
        defaultPos = this.transform.position;
        UICamera = UIManager.Instance.UICamera;
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnMouseDown()
    {
        this.transform.position = pressedPos;
        AudioManager.Instance.PlayEffectSoundByName(effectSoundsPressed);
    }

    private void OnMouseUp()
    {
        graphic.material = normalMaterial;
        // �ж�����Ƿ��ڰ�ť��Χ��
        if (BoundsUtils.GetSceneRect(UICamera, rectTransform).Contains(Input.mousePosition))
        {
            OnClick?.Invoke();
        }
        this.transform.position = defaultPos;
    }

    private void OnMouseEnter()
    {
        graphic.material = glowMaterial;
        AudioManager.Instance.PlayEffectSoundByName(effectSoundsHighlight);
    }

    private void OnMouseExit()
    {
        graphic.material = normalMaterial;
        this.transform.position = defaultPos;
    }
}
