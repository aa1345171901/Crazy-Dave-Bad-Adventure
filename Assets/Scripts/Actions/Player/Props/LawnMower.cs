using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class LawnMower : BaseProp
{
    [Tooltip("小推车默认速度")]
    public float defaultSpeed = 6;

    [Tooltip("造成伤害的LayerMask")]
    public LayerMask triggerLayer;

    public AudioSource audioSource;

    private int finalDamage;
    private float finalSpeed;
    private float finalAttackCoolingTime;

    private float timer;

    public override void Reuse()
    {
        base.Reuse();
        var userData = GameManager.Instance.UserData;
        finalDamage = Mathf.RoundToInt((userData.Power + DefaultDamage) * (100f + userData.PercentageDamage) / 100);
        finalSpeed = defaultSpeed * (100f + userData.Speed) / 100;
        finalAttackCoolingTime = DefaultAttackCoolingTime * (1 - userData.Speed / (100f + userData.Speed));

        if (!AudioManager.Instance.AudioLists.Contains(this.audioSource))
        {
            AudioManager.Instance.AudioLists.Add(this.audioSource);
            this.audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        }
    }

    private void OnDisable()
    {
        AudioManager.Instance.AudioLists.Remove(this.audioSource);
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();
        if (GameManager.Instance.IsDaytime || Time.timeScale == 0)
            return;
        timer += Time.deltaTime;
        if (timer > finalAttackCoolingTime)
        {
            timer = 0;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
            var levelBounds = LevelManager.Instance.LevelBounds;
            float x = levelBounds.min.x;
            float randomY = Random.Range(levelBounds.min.y + 1, levelBounds.max.y - 1);
            this.transform.position = new Vector3(x, randomY, 0);
        }
        transform.Translate(Vector3.right * finalSpeed * Time.deltaTime);
    }

    public override void DayEnd()
    {
        base.DayEnd();
        audioSource.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerLayer.Contains(collision.gameObject.layer))
        {
            var helath = collision.GetComponent<Health>();
            if (helath)
            {
                helath.DoDamage(finalDamage, DamageType.LawnMower);
            }
        }
    }
}
