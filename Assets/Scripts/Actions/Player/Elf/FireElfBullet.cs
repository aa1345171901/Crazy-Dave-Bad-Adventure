using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class FireElfBullet : MonoBehaviour
{
    public int damage = 1;
    public float range = 1;
    public int level = 1;
    public LayerMask targetLayer;
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject level4;

    private void Start()
    {
        level1.gameObject.SetActive(level >= 1);
        level2.gameObject.SetActive(level >= 2);
        level3.gameObject.SetActive(level >= 3);
        level4.gameObject.SetActive(level >= 4);
        int y = (int)((-this.transform.position.y + 10) * 10);
        foreach (var item in GetComponentsInChildren<ParticleSystem>())
        {
            var renderer = item.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
                renderer.sortingOrder = y;
        }
        StartCoroutine(DelayDoDamage());
    }

    IEnumerator DelayDoDamage()
    {
        yield return new WaitForSeconds(0.2f);
        var colliders = Physics2D.OverlapCircleAll(this.transform.position, range, targetLayer);
        foreach (var item in colliders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                if (health)
                {
                    health.DoDamage(damage, DamageType.FireElf);
                }
            }
        }
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(gameObject);
    }
}
