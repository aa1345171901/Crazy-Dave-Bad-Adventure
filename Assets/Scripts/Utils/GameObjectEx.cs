using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// API扩展
/// </summary>
public static class GameObjectEx
{
    /// <summary>
    /// 获取组件，没有则添加
    /// </summary>
    public static T GetComponentOrAdd<T>(this GameObject g) where T : Component
    {
        T t = g.GetComponent<T>();
        if (t == null)
        {
            t = g.AddComponent<T>();
        }
        return t;
    }

    /// <summary>
    /// 刷新布局
    /// </summary>
    public static void UpdateLayout(GameObject go, bool isAnim = true)
    {
        if (go == null)
        {
            return;
        }

        if (isAnim)
        {
            go.SetActive(false);
            go.SetActive(true);
        }

        UIBehaviour[] layouts = go.GetComponentsInChildren<UIBehaviour>(true);

        if (go == null)
        {
            return;
        }
        for (int i = 0; i < layouts.Length; i++)
        {
            if (layouts[i] != null && layouts[i].GetComponent<ILayoutController>() != null)
            {
                RectTransform rect = layouts[i].GetComponent<RectTransform>();
                if (rect != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
                }
            }
        }
    }

    /// <summary>
    /// 开启协程执行滑动，拖拽在鼠标静止时不执行
    /// </summary>
    public static void AutoScroll(PointerEventData eventData, Transform root, RectTransform item, bool canToFirst)
    {
        var layout = root.GetComponent<VerticalLayoutGroup>();
        layout.GetComponent<ContentSizeFitter>().enabled = false;
        var scroll = layout.GetComponentInParent<ScrollRect>();
        layout.enabled = false;

        // 将拖拽坐标转换为UI坐标
        RectTransformUtility.ScreenPointToWorldPointInRectangle(item, eventData.position, eventData.pressEventCamera, out Vector3 worldPoint);
        item.position = new Vector3(item.position.x, worldPoint.y);

        int index = item.GetSiblingIndex();
        if (eventData.delta.y < 0 && index < root.childCount - 1)
        {
            var downItem = root.GetChild(index + 1);
            if (item.position.y < downItem.position.y)
            {
                item.SetSiblingIndex(index + 1);
                downItem.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, item.sizeDelta.y + layout.spacing);
            }
        }
        else
        {
            if ((canToFirst && index > 0) || index - 1 > 0)
            {
                var upItem = root.GetChild(index - 1);
                if (item.position.y > upItem.position.y)
                {
                    item.SetSiblingIndex(index - 1);
                    upItem.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, item.sizeDelta.y + layout.spacing);
                }
            }
        }

        // 获取目标对象相对于视口的位置 边缘滑动
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scroll.viewport, Input.mousePosition, eventData.pressEventCamera, out Vector2 localPoint);
        if ((index == 0 || index == item.transform.parent.childCount - 1) && scroll.viewport.rect.Contains(localPoint))
            return;
        if (localPoint.y < scroll.viewport.rect.yMin)
        {
            scroll.content.anchoredPosition += new Vector2(0, scroll.viewport.rect.yMin - localPoint.y);
        }
        if (localPoint.y > scroll.viewport.rect.yMax)
        {
            scroll.content.anchoredPosition -= new Vector2(0, localPoint.y - scroll.viewport.rect.yMax);
        }
    }
}