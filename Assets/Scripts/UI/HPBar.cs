using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Text HPText;
    public Slider HPSlider;

    private void Start()
    {
        if (HPText == null)
            HPText = this.GetComponentInChildren<Text>();
        if (HPSlider == null)
            HPSlider = this.GetComponentInChildren<Slider>();
        SetHPBar(GameManager.Instance.Player.Health.health, GameManager.Instance.Player.Health.maxHealth);
    }

    public void SetHPBar(int hp, int maxHp)
    {
        float value = (float)hp / maxHp;
        HPSlider.value = value;
        HPText.text = hp +  "/" + maxHp;
    }
}
