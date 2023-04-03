using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public PlantAttribute plantAttribute;

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

    }
}
