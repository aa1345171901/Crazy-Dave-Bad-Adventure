using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum KeyEnum
{
    Key,
    Axis,
    KeyValue
}

public class KeyChangeItem : MonoBehaviour
{
    [Tooltip(@"更改的键名，从InputData中查看Assets\Scripts\InputSystem\Resources")]
    public string KeyName;

    [Tooltip("键类型")]
    public KeyEnum keyEnum;

    [Tooltip("Axis中大的还是小的")]
    public bool isMin;

    [Tooltip("之前的键")]
    public Text lastKey;

    [Tooltip("更改后的键")]
    public Text nowKey;

    public void SetKeyText()
    {
        string lastKeyName = "";
        switch (keyEnum)
        {
            case KeyEnum.Key:
                lastKeyName = InputManager.GetKey(KeyName).keyCode.ToString();
                break;
            case KeyEnum.Axis:
                if (isMin)
                    lastKeyName = InputManager.GetAxisKey(KeyName).min.ToString();
                else
                    lastKeyName = InputManager.GetAxisKey(KeyName).max.ToString();
                break;
            case KeyEnum.KeyValue:
                lastKeyName = InputManager.GetValueKey(KeyName).keyCode.ToString();
                break;
            default:
                break;
        }
        lastKey.text = lastKeyName;
    }
}
