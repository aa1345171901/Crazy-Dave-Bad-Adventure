using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGroup : MonoBehaviour
{
    public List<CarFall> cars;

    public int Damage;

    public void Init()
    {
        foreach (var item in cars)
        {
            item.gameObject.SetActive(true);
            item.Resume();
            item.Damage = Damage;
            var randomPos = item.transform.position;
            randomPos.x += Random.Range(-0.3f, 0.3f);
            randomPos.y += Random.Range(-0.3f, 0.3f);
            item.transform.position = randomPos;
        }
        int index = Random.Range(0, cars.Count);
        cars[index].gameObject.SetActive(false);
    }
}
