using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [Tooltip("高亮时使用的材质")]
    public Material glowMaterial;
    [Tooltip("常态时使用的材质")]
    public Material normalMaterial;
    [Tooltip("改变材质的目标UI")]
    public MaskableGraphic graphic;

    [Tooltip("按下按钮时偏移")]
    public Vector3 offset;

    [Tooltip("按下按钮时播放的音效名")]
    public string effectSoundsPressed = "btnPressed";
    [Tooltip("高亮时播放的音效名")]
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
        // 判断鼠标是否在按钮范围内
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
