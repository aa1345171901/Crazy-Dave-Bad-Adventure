using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PeaShooter : Plant
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Sprite attack0Trigger;
    public Sprite attack1Trigger;
    public Sprite attack2Trigger;

    [Tooltip("攻击伤害")]
    public int Damage = 5;
    [Tooltip("攻击范围")]
    public float Range = 5;
    [Tooltip("攻击冷却时间")]
    public float CoolTime = 2;
    [Tooltip("攻击目标")]
    public LayerMask TargetLayer;

    [Tooltip("子弹发射位置")]
    public Transform BulletPos;
    [Tooltip("子弹预制体")]
    public PeaBullet PeaBullet;

    private float timer;
    private int finalDamage;
    private float finalRage;
    private float finalCoolTime;

    private readonly int LevelRange = 10;
    private readonly float LevelCoolTime = 0.2f;

    public override void Reuse()
    {
        base.Reuse();
        var levelBounds = LevelManager.Instance.LevelBounds;
        float randomX = Random.Range(levelBounds.min.x, levelBounds.max.x);
        // 0.5 刚好站在格子上
        float randomY = (int)Random.Range(levelBounds.min.y, levelBounds.max.y - 0.5f) + 0.5f;
        this.transform.position = new Vector3(randomX, randomY, 0);
        int y = (int)((-randomY + 10) * 10);
        spriteRenderer.sortingOrder = y;

        finalDamage = (int)((plantAttribute.value1 + Damage) * (GameManager.Instance.UserData.Botany + 100) / 100f);
        finalRage = (int)(Range * (plantAttribute.value2 * LevelRange + 100) / 100f);
        finalCoolTime = CoolTime - plantAttribute.value3 * LevelCoolTime;
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            var hit = Physics2D.Raycast(this.transform.position, Vector2.right, finalRage, TargetLayer);
            if (hit)
            {
                if (spriteRenderer.sprite == attack0Trigger)
                {
                    Attack("Attack0");
                }
                else if (spriteRenderer.sprite == attack1Trigger)
                {
                    Attack("Attack1-3");
                }
                else if (spriteRenderer.sprite == attack2Trigger)
                {
                    Attack("Attack2-3");
                }
            }
        }
    }

    private void Attack(string trigger)
    {
        animator.SetTrigger(trigger);
        timer = Time.time;
        Invoke("CreatePeaBullet", 0.05f);
        // 双发
        // Invoke("CreatePeaBullet", 0.15f);
    }

    private void CreatePeaBullet()
    {
        var peaBullet = GameObject.Instantiate(PeaBullet, BulletPos);
        peaBullet.Damage = finalDamage;
    }
}
