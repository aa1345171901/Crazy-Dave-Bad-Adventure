using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class RunButton : MonoBehaviour
{
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
            InputManager.GetKey("Run").IsDown = true;
            InputManager.GetKey("Run").Down?.Invoke();
        }
        else
        {
            InputManager.GetKey("Run").IsDown = false;
        }
    }
}
