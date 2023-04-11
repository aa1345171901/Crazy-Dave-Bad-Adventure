using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Cornpult : Plant
{
    public override PlantType PlantType => PlantType.Cornpult;

    [Tooltip("�����˺�")]
    public int Damage = 5;
    [Tooltip("������ȴʱ��")]
    public float CoolTime = 2f;
    [Tooltip("����Ŀ��")]
    public LayerMask TargetLayer;

    [Tooltip("�ӵ�����λ��")]
    public Transform BulletPos;
    [Tooltip("�ӵ�Ԥ����")]
    public CornBullet CornBullet;

    private float timer;
    private float finalCoolTime;
    private int finalDamage;
    private float bulletSpeedMul = 1;
    private float finalAttackAnimSpeed = 1;
    private int finalButterRate = 5;
    private float finalButterControlTime = 2;

    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.15f;
    private readonly float LevelButterControlTime = 0.2f;
    private readonly int LevelButterRate = 3;

    public override void Reuse()
    {
        base.Reuse();

        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalDamage = Damage;
        finalCoolTime = CoolTime;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 �������ֵ�� 1Ϊ�����˺�
                case 1:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 2 Ϊ�ٷֱ��˺�
                case 2:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 3 ���͸���
                case 3:
                    finalButterRate = 5 + (int)fieldInfo.GetValue(plantAttribute) * LevelButterRate;
                    break;
                // 4 ��ȴʱ��
                case 4:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    finalAttackAnimSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime * 2;
                    break;
                // �ӵ��ٶ�
                case 5:
                    bulletSpeedMul = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // ���Ϳ���ʱ��
                case 6:
                    finalButterControlTime = 2 + (int)fieldInfo.GetValue(plantAttribute) * LevelButterControlTime;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            var direction = FacingDirections == FacingDirections.Right ? Vector2.right : Vector2.left;
            // 30������Ϊ�����
            var hit = Physics2D.Raycast(this.transform.position, direction, 30, TargetLayer);
            if (hit)
            {
                var character = hit.collider.GetComponent<Character>();
                if (character)
                {
                    animator.SetTrigger("Attack");
                    animator.speed = finalAttackAnimSpeed;
                    StartCoroutine("CreateBullet", character);
                    timer = Time.time;
                }
            }
        }
    }

    IEnumerator CreateBullet(Character character)
    {
        yield return new WaitForSeconds(0.55f / finalAttackAnimSpeed);
        var cornBullet = GameObject.Instantiate(CornBullet, BulletPos);
        cornBullet.Speed *= bulletSpeedMul;
        cornBullet.ControlTimer = finalButterControlTime;
        bool isButter = Random.Range(0, 100) < finalButterRate ? true : false;
        cornBullet.IsButter = isButter;
        cornBullet.Damage = finalDamage;
        cornBullet.TargetZombie = character;
        cornBullet.sortLayer = spriteRenderer.sortingOrder + 5;
    }
}
