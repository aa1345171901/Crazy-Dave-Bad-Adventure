using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ZombiePole : MonoBehaviour
{
    public LayerMask zombie;
    public float Speed;
    public SpriteRenderer spriteRenderer;

    /// <summary>
    ///  是否被魅惑
    /// </summary>
    [ReadOnly]
    public bool IsEnchanted;
    /// <summary>
    /// 投射标枪的目标
    /// </summary>
    [ReadOnly]
    public Character character;
    [ReadOnly]
    public int Damage;

    private List<Health> healths;
    private bool hasDamage;

    private readonly float LiveTime = 5;

    private void Start()
    {
        if (IsEnchanted)
            healths = new List<Health>();
        Invoke("DestroyDelay", LiveTime);
    }

    private void DestroyDelay()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
        int y = (int)((-this.transform.position.y + 10) * 10);
        spriteRenderer.sortingOrder = y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsEnchanted)
        {
            if (zombie.Contains(collision.gameObject.layer) && collision.isTrigger && collision.gameObject != character.gameObject)
            {
                var health = collision.GetComponent<Health>();
                if (health != null && !healths.Contains(health))
                {
                    health.DoDamage(Damage, DamageType.ZombieHurEachOther);
                    healths.Add(health);
                }
            }
        }
        else if (!hasDamage)
        {
            if (collision.gameObject == GameManager.Instance.Player.gameObject)
            {
                GameManager.Instance.DoDamage(Damage);
                hasDamage = true;
            }
        }
    }
}
