using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class WaterElfBullet : MonoBehaviour
{
    public float speed;
    public int damage = 1;
    public int level = 1;
    public float finalRepulsiveForce = 1;
    public float finalDecelerationPercentage = 0;
    public Vector3 targetPos;
    public LayerMask targetLayer;
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject level4;

    /// <summary>
    /// 只能造成一次伤害
    /// </summary>
    public HashSet<Health> healths = new HashSet<Health>();

    private void Start()
    {
        StartCoroutine(DelayDoDamage());

        var direction = (targetPos - this.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        var ps = level4.GetComponent<ParticleSystem>().main;
        ps.startRotation = (angle) * Mathf.Deg2Rad;
        if (direction.x < 0)
            level4.transform.localScale = new Vector3(-1, 1, 1);
        ps = level2.GetComponent<ParticleSystem>().main;
        ps.startRotation = (angle + 180) * Mathf.Deg2Rad;

        level1.gameObject.SetActive(level >= 1);
        level2.gameObject.SetActive(level >= 2);
        level3.gameObject.SetActive(level >= 3);
        level4.gameObject.SetActive(level >= 4);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
        int y = (int)((-this.transform.position.y + 10) * 10);
        foreach (var item in GetComponentsInChildren<ParticleSystem>())
        {
            var renderer = item.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
                renderer.sortingOrder = y;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetLayer.Contains(collision.gameObject.layer))
        {
            var health = collision.GetComponent<Health>();
            if (health)
            {
                if (!healths.Contains(health))
                {
                    healths.Add(health);
                    health.DoDamage(damage, DamageType.WaterElf);
                    var aiMove = collision.GetComponent<Character>().FindAbility<AIMove>();
                    if (aiMove)
                    {
                        aiMove.BeDecelerated(finalDecelerationPercentage, 2f);
                        aiMove.SetRepulsiveForce(finalRepulsiveForce, this.transform.position);
                    }
                }
            }
        }
    }

    IEnumerator DelayDoDamage()
    {
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(gameObject);
    }
}
