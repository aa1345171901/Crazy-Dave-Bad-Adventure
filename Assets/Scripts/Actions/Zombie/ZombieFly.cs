using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieFly : MonoBehaviour
{
    public Trigger2D headTrigger2D;
    public SkeletonUtilityBone head;

    public Trigger2D bodyTrigger2D;
    public GameObject body;

    public AudioSource audioSource;

    private bool isHeadFly;
    private bool isBodyFly;

    private Vector3 originalLocation;  // 头部原本的位置
    private float landingTime;  // 落地时间
    private Vector3 direction;
    private float timer;
    private float angle;
    private float flySpeed;
    private bool isTrigger;  // 是否已经触发碰撞

    private readonly float flyTime = 0.3f;  // 飞行时间，重力模拟，0.3f差不多为僵尸高度剩余落地时间为滚动

    private Action bodyAction;

    private void Start()
    {
        originalLocation = head.transform.position;
        Reuse();
    }

    private void Update()
    {
        if (Time.time - timer < landingTime)
        {
            float offset = Time.time - timer;
            float process = 1 - offset / landingTime;

            if (isHeadFly)
            {
                Trigger(headTrigger2D);

                // 飞头碰撞了就往反向飞剩余距离
                if (!isTrigger)
                {
                    head.transform.Translate(direction * flySpeed * process * Time.deltaTime, Space.World);
                    angle += UnityEngine.Random.Range(-1, -3);
                }
                else
                {
                    head.transform.Translate(-direction * flySpeed * process * Time.deltaTime, Space.World);
                    angle -= UnityEngine.Random.Range(-1, -3);
                }

                head.transform.rotation = Quaternion.Euler(0, 0, angle);

                if (Time.time - timer < flyTime)
                    head.transform.Translate(Vector3.down * offset * 9.8f * Time.deltaTime, Space.World);
            }

            if (isBodyFly)
            {
                Trigger(bodyTrigger2D);

                // 碰到了就立即执行后续动画
                if (isTrigger)
                {
                    bodyAction.Invoke();
                    isBodyFly = false;
                }
                else
                {
                    body.transform.Translate(direction * flySpeed * process * Time.deltaTime, Space.World);
                }

                if (Time.time - timer > flyTime * 2)
                {
                    bodyAction.Invoke();
                    isBodyFly = false;
                }
            }
        }
    }

    private void Trigger(Trigger2D trigger)
    {
        if (GameManager.Instance.IsZombieShock && trigger.IsTrigger && !isTrigger && Time.time - timer < flyTime * 2)
        {
            Character zombie = trigger.Target.GetComponent<Character>();
            if (zombie != null)
            {
                zombie.Health.DoDamage(GameManager.Instance.ZombieFlyDamage, DamageType.ZombieFly);
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.Play();
                isTrigger = true;
            }
        }
    }

    public void Reuse()
    {
        head.mode = SkeletonUtilityBone.Mode.Follow;
        head.transform.position = originalLocation;
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        CloseFly();
    }

    public void SetZombieFly(float flySpeed, bool isHead = true, Action action = null)
    {
        if (isHead)
        {
            head.mode = SkeletonUtilityBone.Mode.Override;
            isHeadFly = true;
            headTrigger2D.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            isBodyFly = true;
            bodyAction = action;
            bodyTrigger2D.GetComponent<BoxCollider2D>().enabled = true;
        }

        isTrigger = false;
        timer = Time.time;
        angle = 0;
        direction = this.transform.parent.position - GameManager.Instance.Player.transform.position;
        direction = new Vector3(direction.x, direction.y, transform.position.z).normalized;
        landingTime = UnityEngine.Random.Range(0.7f, 1.4f);
        this.flySpeed = flySpeed * 2;
    }

    public void CloseFly()
    {
        isHeadFly = false;
        isBodyFly = false;
        bodyTrigger2D.GetComponent<BoxCollider2D>().enabled = false;
        headTrigger2D.GetComponent<BoxCollider2D>().enabled = false;
    }
}
