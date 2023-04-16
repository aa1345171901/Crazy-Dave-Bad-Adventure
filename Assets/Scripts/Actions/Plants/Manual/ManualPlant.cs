using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ManualPlant : Plant
{
    [Tooltip("�����˺�")]
    public int Damage = 50;
    [Tooltip("��ȴʱ��")]
    public float CoolTime = 15f;
    [Tooltip("����Ŀ��")]
    public LayerMask TargetLayer;
    [Tooltip("������ʱһֱ��������ͼƬ")]
    public SpriteRenderer image;
    [Tooltip("����ֲ�ﶯ����ͼƬ")]
    public SpriteRenderer plant;

    public AudioSource audioSource;

    protected int finalDamage;
    protected float finalCoolTime;
    protected int sunPrice;

    protected Card card;

    /// <summary>
    /// �Ƿ�������
    /// </summary>
    public bool IsManual { get; protected set; } = true;

    public virtual void InitPlant(Card card, int sun)
    {
        this.card = card;
        this.sunPrice = sun;
        card.coolTimer = finalCoolTime;
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
    }

    private void Update()
    {
        Processblity();
    }

    protected virtual void Processblity()
    {
        if (Input.GetMouseButtonDown(0) && IsManual)
        {
            PlacePlant();
        }
        // ����Ҽ�ȡ��
        if (Input.GetMouseButtonDown(1) && IsManual)
        {
            GameObject.Destroy(this.gameObject);
        }

        if (IsManual)
        {
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = targetPos.x - targetPos.x % 0.5f;
            // 0.5 �պ�վ�ڸ�����
            float y = targetPos.y - targetPos.y % 0.5f;
            this.transform.position = new Vector3(x, y, 0);
            int sortingOrder = (int)((-y + 10) * 10);
            plant.sortingOrder = sortingOrder;
            image.sortingOrder = sortingOrder;
            image.transform.position = new Vector3(targetPos.x, targetPos.y, 0);
        }
    }

    protected virtual void PlacePlant()
    {
        audioSource.Play();
        IsManual = false;
        GardenManager.Instance.Sun -= sunPrice;
        image.gameObject.SetActive(false);
        plant.color = Color.white;
        card.PlacePlant();
    }
}
