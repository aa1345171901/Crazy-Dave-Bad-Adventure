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
    [Tooltip(@"���ĵļ�������InputData�в鿴Assets\Scripts\InputSystem\Resources")]
    public string KeyName;

    [Tooltip("������")]
    public KeyEnum keyEnum;

    [Tooltip("Axis�д�Ļ���С��")]
    public bool isMin;

    [Tooltip("֮ǰ�ļ�")]
    public Text lastKey;

    [Tooltip("���ĺ�ļ�")]
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
