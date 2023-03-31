using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class UIPanelInfo :ISerializationCallbackReceiver{
    [NonSerialized]
    public UIPanelType panelType;
    public string panelTypeString;
    public string path;

    /// <summary>
    /// 在反序列化之后转化
    /// </summary>
    public void OnAfterDeserialize()
    {
        UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
        panelType = type;
    }

    public void OnBeforeSerialize()
    {
        
    }
}
