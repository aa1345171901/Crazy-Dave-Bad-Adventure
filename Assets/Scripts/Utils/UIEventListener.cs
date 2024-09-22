using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// UI事件控制
/// </summary>
public class UIEventListener : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// 鼠标点击事件
    /// </summary>
    public UnityEvent onClick = new UnityEvent();

    /// <summary>
    /// 鼠标进入事件
    /// </summary>
    public UnityEvent onMouseEnter = new UnityEvent();

    /// <summary>
    /// 鼠标滑出事件
    /// </summary>
    public UnityEvent onMouseExit = new UnityEvent();

    /// <summary>
    /// 鼠标点击事件
    /// </summary>
    public UnityEvent onPointDown = new UnityEvent();

    /// <summary>
    /// 鼠标抬起事件
    /// </summary>
    public UnityEvent onPointUp = new UnityEvent();

    /// <summary>
    /// 鼠标双击事件
    /// </summary>
    public UnityEvent onPointDoubleClick = new UnityEvent();

    /// <summary>
    /// 鼠标点击事件
    /// </summary>
    public Action onClickCall;

    /// <summary>
    /// 鼠标进入事件
    /// </summary>
    public Action onMouseEnterCall;

    /// <summary>
    /// 鼠标滑出事件
    /// </summary>
    public Action onMouseExitCall;

    /// <summary>
    /// 鼠标按压事件
    /// </summary>
    public Action onPressCall;

    /// <summary>
    /// 鼠标按压结束事件
    /// </summary>
    public Action onPressEndCall;

    private bool isMouseEnter = false;
    private bool _isPress;
    bool isPointUp;
    public bool isPress { get { return _isPress; } }

    void OnDisable()
    {
        OnPointerExit(null);
    }

    void Update()
    {
        if (onPressEndCall == null && onPressCall == null)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && _isPress)
        {
            _isPress = false;
            if (onPressEndCall != null)
            {
                onPressEndCall();
            }
        }

        if (_isPress)
        {
            if (onPressCall != null)
            {
                onPressCall();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            return;
        }

        if (onClickCall != null)
        {
            onClickCall();
        }
        if (onClick != null)
        {
            onClick.Invoke();
        }
        if (eventData.clickCount == 2)
        {
            onPointDoubleClick.Invoke();
        }

        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isMouseEnter || isPointUp)
        {
            return;
        }
        isMouseEnter = true;

        if (onMouseEnterCall != null)
        {
            onMouseEnterCall();
        }
        if (onMouseEnter != null)
        {
            onMouseEnter.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isMouseEnter || isPointUp)
        {
            return;
        }
        isMouseEnter = false;

        if (onMouseExitCall != null)
        {
            onMouseExitCall();
        }
        if (onMouseExit != null)
        {
            onMouseExit.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onPointUp != null)
        {
            onPointUp.Invoke();
            isPointUp = true;
            isMouseEnter = false;
            IEnumerator Delay()
            {
                yield return new WaitForSeconds(0.5f);
                isPointUp = false;
            }
            StartCoroutine(Delay());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onPointDown != null)
        {
            onPointDown.Invoke();
        }
    }
}
