using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Sun : MoneyClick
{
    public GameObject Sunshine1;
    public GameObject Sunshine2;

    private float sunshine1Angle;
    private float sunshine2Angle;

    private void OnMouseEnter()
    {
        // todo µÀ¾ß½âËø
        OnClick();
    }

    private void OnMouseDown()
    {
        OnClick();
    }

    protected override void NumAdd()
    {
        GardenManager.Instance.Sun += Price;
    }

    protected override void PlayAnimation()
    {
        sunshine1Angle += 0.5f;
        sunshine2Angle -= 0.5f;
        Sunshine1.transform.rotation = Quaternion.Euler(0, 0, sunshine1Angle);
        Sunshine2.transform.rotation = Quaternion.Euler(0, 0, sunshine2Angle);
    }
}
