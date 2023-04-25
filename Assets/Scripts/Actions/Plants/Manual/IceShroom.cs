using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class IceShroom : ManualPlant
{
    [Tooltip("����ʱ��")]
    public float FrostTime = 3;

    public AudioClip frost;

    private float finalFrostTime;
    private float finalFrostAttackSpeed;

    private readonly float LevelCoolTime = 0.8f;
    private readonly float LevelFrostTime = 0.5f;
    private readonly float LevelFrostAttackSpeed = 0.1f;

    public override void InitPlant(Card card, int sun)
    {
        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalDamage = Damage;
        finalCoolTime = CoolTime;
        finalFrostTime = FrostTime;
        finalFrostAttackSpeed = 1;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 ����ʱ��
                case 0:
                    finalFrostTime += LevelFrostTime * (int)fieldInfo.GetValue(plantAttribute);
                    break;
                // 1 ��ȴʱ��
                case 1:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // 3 �����ڼ乥������
                case 3:
                    finalFrostAttackSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelFrostAttackSpeed;
                    break;
                default:
                    break;
            }
        }
        base.InitPlant(card, sun);
    }

    protected override void PlacePlant()
    {
        base.PlacePlant();
        StartCoroutine(Frost());
    }

    IEnumerator Frost()
    {
        animator.Play("Frost");
        yield return new WaitForSeconds(1f);
        this.plant.enabled = false;
        audioSource.clip = frost;
        audioSource.Play();
        var enemys = new List<ZombieDicts>(LevelManager.Instance.Enemys);
        foreach (var item in enemys)
        {
            var zombies = new List<Character>(item.Zombies);
            foreach (var zombie in zombies)
            {
                var aiMove = zombie.FindAbility<AIMove>();
                if (aiMove)
                    aiMove.BeDecelerated(1, finalFrostTime);
            }
        }
        var fires = GameObject.FindGameObjectsWithTag("Fire");
        foreach (var item in fires)
        {
            Destroy(item.gameObject);
        }
        GameManager.Instance.Player.FindAbility<CharacterAttack>().IceShroomAttackSpeed += finalFrostAttackSpeed;
        yield return new WaitForSeconds(finalFrostTime - 1);
        GameManager.Instance.Player.FindAbility<CharacterAttack>().IceShroomAttackSpeed -= finalFrostAttackSpeed;
        Destroy(this.gameObject);
    }
}
