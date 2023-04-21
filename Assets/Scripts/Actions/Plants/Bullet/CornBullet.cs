using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class CornBullet : MonoBehaviour
{
    public float Speed;
    public int Damage = 5;
    public Character TargetZombie;
    public float ControlTimer;
    public bool IsButter;
    public int sortLayer;

    private float upSpeed;
    private float fallSpeed;

    public SpriteRenderer spriteRenderer;
    public Sprite butter;
    public Sprite butterControl;

    public AudioSource audioSource;
    public AudioClip hit1;
    public AudioClip hit2;

    private float timer;
    public float upTimer;  // 计算上升时间，大致为初始时到目标地的一半时间
    private Vector3 startPos;

    private readonly float MaxLiveTime = 15;

    private void Start()
    {
        Invoke("DestroyBullet", MaxLiveTime);
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;

        upTimer = (TargetZombie.transform.position - this.transform.position).magnitude / Speed / 2;
        if (IsButter)
        {
            spriteRenderer.sprite = butter;
        }
        startPos = this.transform.position;
        spriteRenderer.sortingOrder = sortLayer;

        upSpeed = fallSpeed = 5 / upTimer;
        float vOffset = TargetZombie.transform.position.y + 1.05f - this.transform.position.y;
        float speedOffset = 2 * vOffset / upTimer;
        if (speedOffset > 0)
            upSpeed = (5 + speedOffset) / upTimer;
        else
            fallSpeed = (5 - speedOffset) / upTimer;
    }

    private void Update()
    {
        if (timer < upTimer * 2)
        {
            if (timer < upTimer)
            {
                transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            }
            float process = timer / (upTimer * 2);
            var lerp = Vector3.Lerp(startPos, TargetZombie.transform.position, process);
            transform.position = new Vector3(lerp.x, transform.position.y, 0);

            timer += Time.deltaTime;
        }
        else
        {
            if (TargetZombie == null || TargetZombie.IsDead)
                GameObject.Destroy(this.gameObject);
            this.transform.position = TargetZombie.transform.position + Vector3.up;
            if (IsButter)
            {
                var aiMove = TargetZombie.FindAbility<AIMove>();
                if (aiMove != null)
                {
                    aiMove.canMove = false;
                    spriteRenderer.sprite = butterControl;
                }
                TargetZombie.Health.DoDamage(Damage, DamageType.Cornpult, true);
                Invoke("DestroyButter", ControlTimer);
            }
            else
            {
                TargetZombie.Health.DoDamage(Damage, DamageType.Cornpult);
                spriteRenderer.enabled = false;
                Invoke("DestroyButter", 0.1f);
                timer = upTimer;
            }
            audioSource.clip = Random.Range(0, 2) == 0 ? hit1 : hit2;
            audioSource.Play();
        }
    }

    private void DestroyButter()
    {
        if (IsButter)
        {
            var aiMove = TargetZombie.FindAbility<AIMove>();
            if (aiMove != null)
            {
                aiMove.canMove = true;
            }
        }
        GameObject.Destroy(this.gameObject);
    }
}
