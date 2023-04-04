using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public PlantAttribute plantAttribute;

    public virtual PlantType PlantType => PlantType.None;

    protected SpriteRenderer spriteRenderer;

    private FacingDirections facingDirections;
    public virtual FacingDirections FacingDirections 
    { 
        get 
        {
            return facingDirections;
        }
        set
        {
            if (facingDirections != value)
            {
                facingDirections = value;
                if (facingDirections == FacingDirections.Left)
                    this.transform.localScale = new Vector3(-1, 1, 1);
                else
                    this.transform.localScale = Vector3.one;
            }
        }
    }

    public virtual void Reuse()
    {
        if (spriteRenderer == null)
            spriteRenderer = this.GetComponent<SpriteRenderer>();

        var levelBounds = LevelManager.Instance.LevelBounds;
        float randomX = Random.Range(levelBounds.min.x, levelBounds.max.x);
        // 0.5 刚好站在格子上
        float randomY = (int)Random.Range(levelBounds.min.y, levelBounds.max.y - 0.5f) + 0.5f;
        this.transform.position = new Vector3(randomX, randomY, 0);
        int y = (int)((-randomY + 10) * 10);
        spriteRenderer.sortingOrder = y;
        // 如果在右半部分则面向左
        if (randomX > levelBounds.center.x)
            FacingDirections = FacingDirections.Left;
        else
            FacingDirections = FacingDirections.Right;
    }
}
