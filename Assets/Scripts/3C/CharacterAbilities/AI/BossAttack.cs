using Spine;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("TopDownPlate/AI/Ability/Attack/Boss Attack")]
public class BossAttack : AIAttack
{
    [Space(10)]

    [Tooltip("距离小于该值才会使用脚和手攻击")]
    public float AttackRange = 2.5f;

    public string FireBallAttackAnimation = "FireBallAttack";
    public BossBall fireBall;
    public string IceBallAttackAnimation = "IceBallAttack";
    public BossBall iceBall;
    public int ballCoolTime = 20; // 球攻击冷却，不然会全屏
    private float lastBallTimer;

    public string SmashingCarAttackAnimation = "SmashingCar";
    [Tooltip("连续扔车数量")]
    public int carCount = 5;
    [Tooltip("车预制体")]
    public CarFall carFall;
    public string SmashingCar2AttackAnimation = "SmashingCar2";
    [Tooltip("车组预制体")]
    public CarGroup carGroupPrefab;
    [Tooltip("扔很多时冷却时间")]
    public int coolTimer = 2;

    public string HandLeftAttackAnimation = "HandLeftAttack";
    public string HandLeftBeforeAttackAnimation = "HandLeftAttack_Before";
    public string HandLeftAfterAttackAnimation = "HandLeftAttack_After";
    public string HandRightAttackAnimation = "HandRightAttack";
    public string HandRightBeforeAttackAnimation = "HandRightAttack_Before";
    public string HandRightAfterAttackAnimation = "HandRightAttack_After";

    public string LegLeftAttackAnimation = "LegLeftAttack";
    public string LegLeftBeforeAttackAnimation = "LegLeftAttack_Before";
    public string LegLeftAfterAttackAnimation = "LegLeftAttack_After";
    public string LegRightAttackAnimation = "LegRightAttack";
    public string LegRightBeforeAttackAnimation = "LegRightAttack_Before";
    public string LegRightAfterAttackAnimation = "LegRightAttack_After";

    public BoxCollider2D HandLeftAttackBoxColider;
    public BoxCollider2D HandRightAttackBoxColider;
    public CircleCollider2D LegLeftAttackBoxColider;
    public CircleCollider2D LegRightAttackBoxColider;

    public AudioSource AttackAudio;
    public AudioClip AttackBodyClip;
    public AudioClip AttackCarClip;
    public AudioSource AttackLegAfter;
    public AudioSource AttackHandAfter;
    public AudioSource AttackBallAfter;

    private TrackEntry trackEntry;
    private float timer;
    private Trigger2D handLeftAttackBoxColider;
    private Trigger2D handRightAttackBoxColider;
    private Trigger2D legLeftAttackBoxColider;
    private Trigger2D legRightAttackBoxColider;

    private int resumeCount; // 剩余车数量
    private CarGroup carGroup;
    private bool canSmashingCar = true;

    protected override void Initialization()
    {
        base.Initialization();
        handLeftAttackBoxColider = HandLeftAttackBoxColider.GetComponent<Trigger2D>();
        handRightAttackBoxColider = HandRightAttackBoxColider.GetComponent<Trigger2D>();
        legLeftAttackBoxColider = LegLeftAttackBoxColider.GetComponent<Trigger2D>();
        legRightAttackBoxColider = LegRightAttackBoxColider.GetComponent<Trigger2D>();
    }

    private void OnEnable()
    {
        AudioManager.Instance.AudioLists.Add(AttackAudio);
        AttackAudio.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(AttackLegAfter);
        AttackLegAfter.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(AttackHandAfter);
        AttackHandAfter.volume = AudioManager.Instance.EffectPlayer.volume;
    }

    private void OnDisable()
    {
        AudioManager.Instance.AudioLists.Remove(AttackAudio);
        AudioManager.Instance.AudioLists.Remove(AttackLegAfter);
        AudioManager.Instance.AudioLists.Remove(AttackHandAfter);
    }

    public override void Reuse()
    {
        base.Reuse();
        trackEntry = null;
        int waveIndex = LevelManager.Instance.IndexWave + 1;
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
            this.realDamage = (int)((Damage + 5f) * waveIndex);
        }
        else
        {
            this.realDamage = (int)((Damage + 7f) * waveIndex * 1.5f);
        }
        HandLeftAttackBoxColider.enabled = false;
        HandRightAttackBoxColider.enabled = false;
        LegLeftAttackBoxColider.enabled = false;
        LegRightAttackBoxColider.enabled = false;
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();
        if (aiMove.decelerationPercentage == 0)
            return;
        if (character.State.AIStateType == AIStateType.Init || character.IsDead)
        {
            return;
        }
        JudgeTrigger(handLeftAttackBoxColider, HandLeftAttackBoxColider);
        JudgeTrigger(handRightAttackBoxColider, HandRightAttackBoxColider);
        JudgeTrigger(legLeftAttackBoxColider, LegLeftAttackBoxColider);
        JudgeTrigger(legRightAttackBoxColider, LegRightAttackBoxColider);

        if (Time.time - timer < AttackJudgmentTime)
            return;

        timer = Time.time;
        if (trackEntry != null)
            return;

        // 判断此时是否攻击
        float random = Random.Range(0, 1f);
        if (random > realAttackProbability)
            return;

        if (aiMove.AIParameter.Distance < AttackRange)
        {
            // 使用手还是脚
            if (Random.Range(0, 2) == 0)
            {
                // 先使用左边还是右边
                if (Random.Range(0, 2) == 0)
                    AttackHandLeg(HandLeftBeforeAttackAnimation, HandLeftAttackAnimation, HandLeftAfterAttackAnimation, HandLeftAttackBoxColider,
                        HandRightBeforeAttackAnimation, HandRightAttackAnimation, HandRightAfterAttackAnimation, HandRightAttackBoxColider, AttackHandAfter);
                else
                    AttackHandLeg(HandRightBeforeAttackAnimation, HandRightAttackAnimation, HandRightAfterAttackAnimation, HandRightAttackBoxColider,
                         HandLeftBeforeAttackAnimation, HandLeftAttackAnimation, HandLeftAfterAttackAnimation, HandLeftAttackBoxColider, AttackHandAfter);
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                    AttackHandLeg(LegLeftBeforeAttackAnimation, LegLeftAttackAnimation, LegLeftAfterAttackAnimation, LegLeftAttackBoxColider,
                        LegRightBeforeAttackAnimation, LegRightAttackAnimation, LegRightAfterAttackAnimation, LegRightAttackBoxColider, AttackLegAfter);
                else
                    AttackHandLeg(LegRightBeforeAttackAnimation, LegRightAttackAnimation, LegRightAfterAttackAnimation, LegRightAttackBoxColider,
                         LegLeftBeforeAttackAnimation, LegLeftAttackAnimation, LegLeftAfterAttackAnimation, LegLeftAttackBoxColider, AttackLegAfter);
            }
        }
        else
        {
            // 扔车还是吐火球
            if (Random.Range(0, 2) == 0 && canSmashingCar)
            {
                // 一辆一辆扔还是一下扔
                if (Random.Range(0, 2) == 0)
                {
                    resumeCount = carCount;
                    AttackCar();
                }
                else
                    AttackCars();
            }
            else if (Time.time - lastBallTimer > ballCoolTime)
            {
                lastBallTimer = Time.time;
                if (Random.Range(0, 2) == 0)
                    AttackBall(true);
                else
                    AttackBall(false);
            }
        }
    }

    private void JudgeTrigger(Trigger2D trigger2D, Collider2D boxCollider2D)
    {
        // 角色在攻击触发器中且触发器打开
        if (boxCollider2D.enabled && trigger2D.IsTrigger)
        {
            if (GameManager.Instance.IsEnd)
            {
                var target = trigger2D.Target.GetComponent<Character>();
                if (target != null && !healths.Contains(target.Health))
                {
                    target.Health.DoDamage(realDamage, DamageType.ZombieHurEachOther);
                    healths.Add(target.Health);
                }
            }
            else if (trigger2D.Target == GameManager.Instance.Player.gameObject)
            {
                GameManager.Instance.DoDamage(realDamage);
                boxCollider2D.enabled = false;
            }
        }
    }

    private void AttackHandLeg(string attackBefore, string attack, string attackAfter, Collider2D collider2D1, string doubleHitAttackBefore, string doubleHitAttack, string doubleHitAttackAfter, Collider2D collider2D2, AudioSource audioSource)
    {
        aiMove.canMove = false;
        AttackAudio.clip = AttackBodyClip;
        AttackAudio.Play();
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, attackBefore, false);
        trackEntry.TimeScale = 1.8f;
        trackEntry.Complete += (e) =>
        {
            collider2D1.enabled = true;
            trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, attack, false);
            trackEntry.Complete += (e) =>
            {
                collider2D1.enabled = false;
                audioSource.Play();
                trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, attackAfter, false);
                trackEntry.Complete += (e) =>
                {
                    skeletonAnimation.AnimationState.ClearTrack(1);
                    AttackAudio.Play();
                    trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, doubleHitAttackBefore, false);
                    trackEntry.TimeScale = 1.8f;
                    trackEntry.Complete += (e) =>
                    {
                        collider2D2.enabled = true;
                        audioSource.Play();
                        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, doubleHitAttack, false);
                        trackEntry.Complete += (e) =>
                        {
                            collider2D2.enabled = false;
                            trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, doubleHitAttackAfter, false);
                            trackEntry.Complete += (e) =>
                            {
                                skeletonAnimation.AnimationState.ClearTrack(1);
                                aiMove.canMove = true;
                                trackEntry = null;
                            };
                        };
                    };
                };
            };
        };
    }

    private void AttackCar()
    {
        aiMove.canMove = false;
        AttackAudio.clip = AttackCarClip;
        AttackAudio.Play();
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, SmashingCarAttackAnimation, false);
        trackEntry.Complete += (e) =>
        {
            resumeCount--;
            Vector2 pos = GameManager.Instance.Player.transform.position;
            var car = GameObject.Instantiate(carFall);
            car.transform.position = pos;
            car.Damage = realDamage;
            if (character.FacingDirection == FacingDirections.Left)
                car.transform.localScale = new Vector3(-1, 1, 1);
            if (resumeCount > 0)
            {
                AttackCar();
            }
            else
            {
                skeletonAnimation.AnimationState.ClearTrack(1);
                aiMove.canMove = true;
                trackEntry = null;
            }
        };
    }

    private void AttackCars()
    {
        AttackAudio.clip = AttackCarClip;
        AttackAudio.Play();
        aiMove.canMove = false;
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, SmashingCarAttackAnimation, false);
        trackEntry.Complete += (e) =>
        {
            resumeCount--;
            Vector2 pos = GameManager.Instance.Player.transform.position;
            if (carGroup == null)
                carGroup = GameObject.Instantiate(carGroupPrefab);
            carGroup.transform.position = pos;
            carGroup.Damage = realDamage;
            if (character.FacingDirection == FacingDirections.Left)
                carGroup.transform.localScale = new Vector3(-1, 1, 1);
            else
                carGroup.transform.localScale = new Vector3(1, 1, 1);
            carGroup.Init();
            skeletonAnimation.AnimationState.ClearTrack(1);
            aiMove.canMove = true;
            trackEntry = null;
            canSmashingCar = false;
            Invoke("DelaySetSmashingCar", coolTimer);
        };
    }

    private void AttackBall(bool isIce)
    {
        aiMove.canMove = false;
        AttackAudio.clip = AttackBodyClip;
        AttackAudio.Play();
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, isIce ? IceBallAttackAnimation : FireBallAttackAnimation, false);
        trackEntry.Complete += (e) =>
        {        
            skeletonAnimation.AnimationState.ClearTrack(1);
            aiMove.canMove = true;
            trackEntry = null;
        };
        StartCoroutine("CreatBall", isIce ? iceBall : fireBall);
    }

    IEnumerator CreatBall(BossBall ballPrefab)
    {
        yield return new WaitForSeconds(0.5f);
        AttackBallAfter.Play();
        var hashSet = RandomUtils.RandomCreateNumber(11, carCount);
        foreach (var item in hashSet)
        {
            var ball = GameObject.Instantiate(ballPrefab);
            float y = item - 5;
            ball.isLeft = character.FacingDirection == FacingDirections.Right ? false : true;
            ball.transform.position = new Vector3(0, y);
            ball.Damage = realDamage;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void DelaySetSmashingCar()
    {
        canSmashingCar = true;
    }
}
