using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Sprite attack0Trigger;
    public Sprite attack1Trigger;
    public Sprite attack2Trigger;

    [Tooltip("¹¥»÷·¶Î§")]
    public float Range = 5;
    [Tooltip("¹¥»÷ÉËº¦")]
    public int Damage = 5;
    [Tooltip("¹¥»÷ÀäÈ´Ê±¼ä")]
    public float CoolTime = 2;
    [Tooltip("¹¥»÷Ä¿±ê")]
    public LayerMask TargetLayer;

    [Tooltip("×Óµ¯·¢ÉäÎ»ÖÃ")]
    public Transform BulletPos;
    [Tooltip("×Óµ¯Ô¤ÖÆÌå")]
    public PeaBullet PeaBullet;

    private float timer;

    private void Start()
    {
        int y = (int)((-this.transform.position.y + 10) * 10);
        spriteRenderer.sortingOrder = y;
    }

    private void Update()
    {
        if (Time.time - timer > CoolTime)
        {
            var hit = Physics2D.Raycast(this.transform.position, Vector2.right, Range, TargetLayer);
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
        Invoke("CreatePeaBullet", 0.15f);
    }

    private void CreatePeaBullet()
    {
        var peaBullet = GameObject.Instantiate(PeaBullet, BulletPos);
        peaBullet.Damage = Damage;
    }
}
