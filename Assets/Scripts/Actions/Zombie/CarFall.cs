using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class CarFall : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;

    [Tooltip("����ʱ�ĳ�")]
    public GameObject car1;
    [Tooltip("�����ĳ�")]
    public GameObject car2;
    [Tooltip("��Χ��")]
    public SpriteRenderer range;
    [Tooltip("�������ѷ�")]
    public GameObject crack;
    [Tooltip("����ʱ��������Ч")]
    public ParticleSystem earth;

    public float triggerRange = 1.5f;
    public bool isDestroy = true;

    private float curTimer;

    private Vector3 startPos;

    public float fallTimer = 0.5f;
    private float destroyTimer = 2f;
    private bool isFade;

    [ReadOnly]
    public int Damage;

    private void Start()
    {
        startPos = car1.transform.position;
        animator.speed = 2;
        animator.Play("AttackRange");
    }

    public void Resume()
    {
        car1.transform.position = new Vector3(1000, 1000, 0);
        curTimer = 0;
        animator.speed = 1;
        animator.Play("AttackRange");
        fallTimer = 1;
        isFade = false;
        car1.SetActive(true);
        range.enabled = true;
        car2.SetActive(false);
        crack.SetActive(false);
    }

    private void Update()
    {
        if (curTimer < fallTimer)
        {
            float process = curTimer / fallTimer;
            var pos = Vector3.Lerp(startPos, this.transform.position, process);
            car1.transform.position = pos;
            curTimer += Time.deltaTime;
        }
        else if (!isFade)
        {
            isFade = true;
            car1.SetActive(false);
            range.enabled = false;
            car2.SetActive(true);
            crack.SetActive(true);
            audioSource.Play();
            earth.Play();
            JudgeTrigger();
            StartCoroutine(DelayDestroy());
        }
    }

    private void JudgeTrigger()
    {
        var colliders = Physics2D.OverlapCircleAll(this.transform.position, triggerRange);

        foreach (var item in colliders)
        {
            if (GameManager.Instance.IsEnd)
            {
                var target = item.GetComponent<Character>();
                if (target != null)
                {
                    target.Health.DoDamage(Damage, DamageType.ZombieHurEachOther);
                }
            }
            else if (item.gameObject == GameManager.Instance.Player.gameObject)
            {
                GameManager.Instance.DoDamage(Damage);
            }
        }
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(destroyTimer - 0.5f);
        animator.Play("FadeOut");
        if (isDestroy)
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(this.gameObject);
        }
    }
}