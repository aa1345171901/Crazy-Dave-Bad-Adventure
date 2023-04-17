using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class WallNut : Plant
{
    public override PlantType PlantType => PlantType.WallNut;

    [Tooltip("�����˺�")]
    public int Damage = 5;
    [Tooltip("������ȴʱ��")]
    public float CoolTime = 12;
    [Tooltip("�����ٶ�")]
    public float Speed = 6;
    [Tooltip("����Ŀ��")]
    public LayerMask TargetLayer;

    public Collider2D Collider2D;
    public AudioSource audioSource;
    public AudioClip wallNut;
    public AudioClip boom;
    public AudioClip roll;
    public ParticleSystem colliderParticle;

    private float timer;
    protected int finalDamage;
    protected float finalCoolTime;
    protected float finalBoomNutRate;
    protected float bulletSpeedMul = 1;

    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.6f;
    private readonly float LevelBoomRate = 0.03f;

    private Vector3 direction; // ǰ������
    private Bounds levelBounds;
    private bool isBoomWallNut;

    private void Start()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    public override void Reuse()
    {
        if (spriteRenderer == null)
            spriteRenderer = this.GetComponent<SpriteRenderer>();
        levelBounds = LevelManager.Instance.LevelBounds;

        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalDamage = Damage;
        finalBoomNutRate = 0;
        finalCoolTime = CoolTime;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 Ϊ�����˺�
                case 0:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 1 Ϊ�ٷֱ��˺�
                case 1:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // �����ٶ�
                case 2:
                    bulletSpeedMul = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // ��ȴʱ��
                case 3:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // �����˺�
                case 4:
                    finalBoomNutRate = (int)fieldInfo.GetValue(plantAttribute) * LevelBoomRate;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);
        animator.speed = bulletSpeedMul;
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            timer = Time.time;
            float random = Random.Range(0, 1f);
            timer -= random;
            Invoke("Init", random);
        }

        if (this.transform.position.x < levelBounds.max.x)
        {
            if (this.transform.position.y > levelBounds.max.y)
            {
                direction = new Vector3(1, -1, 0);
                StartCoroutine(AudioPlayRoll());
            }
            if (this.transform.position.y < levelBounds.min.y)
            {
                direction = new Vector3(1, 1, 0);
                StartCoroutine(AudioPlayRoll());
            }
        }

        this.transform.Translate(direction * Speed * bulletSpeedMul * Time.deltaTime);
    }

    private void Init()
    {
        float randomX = levelBounds.min.x;
        float randomY = (int)Random.Range(levelBounds.min.y + 1, levelBounds.max.y - 1);
        this.transform.position = new Vector3(randomX, randomY, 0);
        direction = Vector3.right;

        isBoomWallNut = Random.Range(0, 1f) < finalBoomNutRate;
        this.spriteRenderer.enabled = true;
        if (isBoomWallNut)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
        Collider2D.enabled = true;
        colliderParticle.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TargetLayer.Contains(collision.gameObject.layer))
        {
            if (isBoomWallNut)
            {
                audioSource.clip = boom;
                audioSource.Play();
                this.spriteRenderer.enabled = false;
                colliderParticle.Play();
                var colliders = Physics2D.OverlapCircleAll(this.transform.position, 3, TargetLayer);
                foreach (var item in colliders)
                {
                    if (item.isTrigger)
                    {
                        var health = collision.GetComponent<Health>();
                        health.DoDamage(finalDamage * 5, DamageType.Bomb);
                    }
                }
                Collider2D.enabled = false;
                Invoke("BoomAfter", 1f);
            }
            else
            {
                StartCoroutine(AudioPlayRoll());
                var health = collision.GetComponent<Health>();
                health.DoDamage(finalDamage, DamageType.WallNut);
                if (direction.y > 0)
                {
                    direction.y = -1;
                }
                else if (direction.y < 0)
                {
                    direction.y = 1;
                }
                else
                {
                    direction = Random.Range(1, 3) == 1 ? new Vector3(1, -1, 0) : new Vector3(1, 1, 0);
                }
            }
        }
    }

    private void BoomAfter()
    {
        this.transform.position = new Vector3(levelBounds.max.x + 10, levelBounds.max.y, 0);
    }

    IEnumerator AudioPlayRoll()
    {
        audioSource.clip = wallNut;
        audioSource.Play();
        colliderParticle.Play();
        yield return new WaitForSeconds(2);
        colliderParticle.Stop();
        audioSource.clip = roll;
        audioSource.Play();
    }
}
