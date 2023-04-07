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

    private Vector3 originalLocation;  // ͷ��ԭ����λ��
    private float landingTime;  // ���ʱ��
    private Vector3 direction;
    private float timer;
    private float angle;
    private float flySpeed;
    private bool isTrigger;  // �Ƿ��Ѿ�������ײ

    private readonly float flyTime = 0.3f;  // ����ʱ�䣬����ģ�⣬0.3f���Ϊ��ʬ�߶�ʣ�����ʱ��Ϊ����

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

                // ��ͷ��ײ�˾��������ʣ�����
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

                // �����˾�����ִ�к�������
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
