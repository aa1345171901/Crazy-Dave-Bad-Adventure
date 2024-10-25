using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class MobieKeyButton : MonoBehaviour
{
    public string keyName;
    private Camera UICamera;
    private RectTransform rectTransform;
    private Rect bounds;
    private bool isDown;

    private void Start()
    {
        UICamera = UIManager.Instance.UICamera;
        rectTransform = GetComponent<RectTransform>();
        bounds = BoundsUtils.GetSceneRect(UICamera, rectTransform);
    }

    private void Update()
    {
        isDown = false;
        if (Input.touchCount > 0)
        {
            var touches = Input.touches;
            for (int i = 0; i < touches.Length; i++)
            {
                if (bounds.Contains(touches[i].position))
                {
                    isDown = true;
                    break;
                }
            }
        }

        if (isDown)
        {
            InputManager.GetKey(keyName).IsDown = true;
            InputManager.GetKey(keyName).Down?.Invoke();
        }
        else
        {
            InputManager.GetKey(keyName).IsDown = false;
        }
    }
}
