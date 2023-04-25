using Spine;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[AddComponentMenu("TopDownPlate/AI/Ability/Attack/Catapult Attack")]
public class CatapultAttack : AIAttack
{
    [Tooltip("�ᷢ�������ľ���")]
    public float AttackRange = 5f;

    [Tooltip("�ý�ʬ����������")]
    public string AttackAnimation = "Attack";
    [Tooltip("�ý�ʬ�����������ܶ�����")]
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
        // ��������˺�����ʹ����ʹ����ͨ����
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
        // �жϴ�ʱ�Ƿ񹥻�
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
