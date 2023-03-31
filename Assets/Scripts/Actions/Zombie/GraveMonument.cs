using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class GraveMonument : MonoBehaviour
{
    public List<Sprite> Earths;

    void Start()
    {
        int index = Random.Range(0, Earths.Count);
        GetComponent<SpriteRenderer>().sprite = Earths[index];
    }
}
