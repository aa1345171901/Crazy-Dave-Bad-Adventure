using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UILocalText : MonoBehaviour
{
    public string key;

    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        ChangeText();
        ConfManager.Instance.languageChange += ChangeText;
    }

    void ChangeText()
    {
        text.text = GameTool.LocalText(key);
    }

    private void OnDestroy()
    {
        ConfManager.Instance.languageChange -= ChangeText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
