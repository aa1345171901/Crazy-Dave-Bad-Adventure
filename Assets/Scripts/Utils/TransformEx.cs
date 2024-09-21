using UnityEngine;

/// <summary>
/// API扩展
/// </summary>
public static class TransformEx
{
    /// <summary>
    /// 销毁子节点
    /// </summary>
    public static void DestroyChild(this Transform t)
    {
        if (t == null)
        {
            return;
        }
        while (t.childCount > 0)
        {
            GameObject.DestroyImmediate(t.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// 还原坐标
    /// </summary>
    public static void ResetLocal(this Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localScale = Vector3.one;
        t.localEulerAngles = Vector3.zero;
    }
}
