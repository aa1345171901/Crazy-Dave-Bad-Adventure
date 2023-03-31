using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeItem : MonoBehaviour
{
    public Text AttributeText;
    public Text Value;

    private int value;

    private void Start()
    {
        if (AttributeText == null)
            AttributeText = this.transform.Find("Text").GetComponent<Text>();
        if (Value == null)
            Value = this.transform.Find("ValueText").GetComponent<Text>();
        SetValue(value);
    }

    public void SetValue(int value)
    {
        this.value = value;
        if (Value == null)
            return;
        Value.text = value.ToString();
        if (value < 0)
        {
            Value.color = AttributeText.color = Color.red;
        }
        else if (value > 0)
        {
            Value.color = AttributeText.color = Color.green;
        }
        else
        {
            Value.color = AttributeText.color = Color.white;
        }
    }
}
