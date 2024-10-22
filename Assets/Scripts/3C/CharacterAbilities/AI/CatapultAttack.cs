using Spine;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[AddComponentMenu("TopDownPlate/AI/Ability/Attack/Catapult Attack")]
public class CatapultAttack : AIAttack
{
    [Tooltip("会发动攻击的距离")]
    public float AttackRange = 5f;

    [Tooltip("该僵尸攻击动画名")]
    public string AttackAnimation = "Attack";
    [Tooltip("该僵尸攻击结束充能动画名")]
    public string AttackAfterAnimation = "Attack_After";

    public Transform basketballPos;
    public GameObject basketball;

    private TrackEntry trackEntry;
    private float timer;

    [ReadOnly]
    public float realAttackRange;

    public override void Reuse()
    {
        base.Reuse();
        trackEntry = null;
        int waveIndex = LevelManager.Instance.IndexWave + 1;
        this.realAttackRange = AttackRange + waveIndex / 10f;
        // 不能造成伤害，会使不能使用普通攻击,有道具时能攻击
        if (waveIndex < 4)
        {
            this.realDamage = Damage;
        }
        else if (waveIndex < 9)
        {
            this.realDamage = (int)((Damage + 1.5f) * (waveIndex / 4f));
        }
        else if (waveIndex < 13)
        {
            this.realDamage = (int)((Damage + 2.5f) * (waveIndex / 3f));
        }
        else if (waveIndex < 17)
        {
            this.realDamage = (int)((Damage + 3.5f) * (waveIndex / 1.5f));
        }
        else if (waveIndex < 21)
        {
            this.realDamage = (int)((Damage + 4.5f) * waveIndex);
        }
        else
        {
            this.realDamage = (int)((Damage + 5.5f) * waveIndex * 1.5f);
        }
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();
        if (aiMove.decelerationPercentage == 0)
            return;
        if (character.State.AIStateType == AIStateType.Init || character.IsDead)
        {
            audioSource?.Stop();
            return;
        }

        if (Time.time - timer < AttackJudgmentTime)
            return;
        timer = Time.time;
        if (trackEntry != null)
            return;
        // 判断此时是否攻击
        float random = Random.Range(0, 1f);
        if (random > realAttackProbability)
            return;

        Attack();
    }

    private void Attack()
    {
        float distance = aiMove.AIParameter.Distance;
        if (distance < realAttackRange)
        {
            audioSource = AudioManager.Instance.RandomPlayZombieSounds();

            aiMove.MoveSpeed = 0;
            trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackAnimation, false);
            trackEntry.Complete += (e) =>
            {
                var ball = GameObject.Instantiate(basketball);
                ball.transform.position = basketballPos.position;
                GameManager.Instance.balls.Add(ball);
                trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackAfterAnimation, false);
                trackEntry.Complete += (e) =>
                {
                    aiMove.SpeedRecovery();
                    trackEntry = null;
                    skeletonAnimation.AnimationState.ClearTrack(1);
                    audioSource = null;
                };
            };
        }
    }
}
