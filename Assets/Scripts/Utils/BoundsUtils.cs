using System.Collections;
using UnityEngine;

public static class BoundsUtils 
{
    public static Rect GetSceneRect(Camera targetCamera, RectTransform rectTransform)
    {
        var scenePos = targetCamera.WorldToScreenPoint(rectTransform.position);
        float width = UIManager.Instance.CanvasScaleFactor * rectTransform.rect.width;
        float height = UIManager.Instance.CanvasScaleFactor * rectTransform.rect.height;
        Rect bounds = new Rect(scenePos.x - width / 2, scenePos.y - height / 2, width, height);
        return bounds;
    }
}