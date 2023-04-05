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
        // 判断鼠标是否在按钮范围内
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
