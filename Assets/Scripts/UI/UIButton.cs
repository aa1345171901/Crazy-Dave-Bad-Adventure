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

    public bool CanMove = true;

    [Header("Event")]
    [Space(10)]
    public UnityEvent OnClick;

    private Vector3 defaultPos;

    private Camera UICamera;
    private RectTransform rectTransform;

    private void Start()
    {
        UICamera = UIManager.Instance.UICamera;
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnMouseDown()
    {
        if (CanMove)
            this.transform.position = defaultPos + offset;
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
        if (CanMove)
            this.transform.position = defaultPos;
    }

    private void OnMouseEnter()
    {
        defaultPos = this.transform.position;
        graphic.material = glowMaterial;
        AudioManager.Instance.PlayEffectSoundByName(effectSoundsHighlight);
    }

    private void OnMouseExit()
    {
        graphic.material = normalMaterial;
        if (CanMove && defaultPos != Vector3.zero)
            this.transform.position = defaultPos;
    }
}
