using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RandomSeedData
{
    public int seed;

    public void InitSeed()
    {
        seed = (int)System.DateTime.Now.Ticks + GameTool.Random(int.MinValue, int.MaxValue);
    }

    public void RandomNexeSeed()
    {
        System.Random random = new System.Random(seed);
        while (true)
        {
            int curSeed = random.Next(int.MinValue, int.MaxValue);
            if (seed != curSeed)
            {
                seed = curSeed;
                return;
            }
        }
    }
}

/// <summary>
/// 游戏常用工具
/// </summary>
public static class GameTool
{
    private static int curClickFrameCount;
    private static GameObject curClickGameObject;

    /// <summary>
    /// 当前鼠标选择的UI缓存
    /// </summary>
    private static readonly List<GraphicRaycaster> graphicRaycasters = new List<GraphicRaycaster>();
    private static readonly List<RaycastResult> raycastResults = new List<RaycastResult>();

    private static RandomSeedData worldRandomSeed = new RandomSeedData() { seed = System.DateTime.Now.Millisecond };
    private static List<ValueTuple<int, System.Random>> allRandom = new List<(int, System.Random)>();

    private static string soleID = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";

    /// <summary>
    /// 设置层
    /// </summary>
    public static void SetLayer(GameObject go, string layerName)
    {
        SetLayer(go, LayerMask.NameToLayer(layerName));
    }

    /// <summary>
    /// 设置层
    /// </summary>
    public static void SetLayer(GameObject go, int layer, int setFixLayer = -1)
    {
        Transform[] ts = go.transform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < ts.Length; i++)
        {
            if (setFixLayer == -1 || ts[i].gameObject.layer == setFixLayer)
            {
                ts[i].gameObject.layer = layer;
            }
        }
    }

    /// <summary>
    /// 世界转UI世界坐标
    /// </summary>
    public static Vector2 WorldToUILocalPosi(Vector2 worldPosi, Camera worldCamera)
    {
        if (worldCamera != null)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.CanvasTransform as RectTransform,
                worldCamera.WorldToScreenPoint(worldPosi), UIManager.Instance.UICamera, out pos))
            {
                return pos;
            }
        }

        return Vector2.zero;
    }

    /// <summary>
    /// 获取鼠标世界坐标
    /// </summary>
    public static Vector2 GetMouseWorldPosi(Camera worldCamera)
    {
        return worldCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
    }

    /// <summary>
    /// 创建唯一ID
    /// </summary>
    public static string SoleID(int idLength = 6, RandomSeedData seed = null)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < idLength; i++)
        {
            stringBuilder.Append(soleID[Random(0, soleID.Length, seed)]);
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 随机数（1:10 随机1到9的值）
    /// </summary>
    public static int Random(int min, int max, RandomSeedData seed = null)
    {
        if (seed == null)
        {
            seed = worldRandomSeed;
            // todo
            seed.RandomNexeSeed();
        }
        var ran = GetRandom(seed.seed);

        int r;
        try
        {
            // 最小的数比最大的数大时会报错
            if (min > max)
            {
                int temp = min;
                min = max;
                max = temp;
                Debug.LogWarning("取随机数最大值需要大于最小值");
            }
            r = ran.Next(min, max);
            if (seed != worldRandomSeed)
            {
                seed.RandomNexeSeed();
            }
            return r;
        }
        catch (Exception e)
        {
            try
            {
                Debug.LogError(e.Message);
                // 多线程不能使用UnityEngine
                return UnityEngine.Random.Range(min, max);
            }
            catch (Exception ex)
            {
                try
                {
                    Debug.LogError(ex.Message);
                    return new Unity.Mathematics.Random((uint)seed.seed).NextInt(min, max);
                }
                catch (Exception exc)
                {
                    Debug.LogError(exc.Message);
                    return min + (int)(DateTime.Today.Ticks % Mathf.Abs(max - min));
                }
            }
        }
    }

    public static System.Random GetRandom(int seed)
    {
        for (int i = 0; i < allRandom.Count; i++)
        {
            if (allRandom[i].Item1 == seed)
            {
                return allRandom[i].Item2;
            }
        }

        System.Random random = new System.Random(seed);
        return random;
    }

    /// <summary>
    /// 获取语言文本
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string LocalText(string key)
    {
        if (ConfManager.Instance == null || ConfManager.Instance.confMgr == null)
            return key;
        return ConfManager.Instance.confMgr.localText.GetText(key);
    }

    /// <summary>
    /// 浮点类型转百分号
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToPercentage(this float value)
    {
        float percentageValue = value * 100;
        return percentageValue.ToString("F2");
    }
}