using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class RunButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        InputManager.GetKey("Run").IsDown = true;
        InputManager.GetKey("Run").Down?.Invoke();
    }

    private void OnMouseUp()
    {
        InputManager.GetKey("Run").IsDown = false;
    }
}
