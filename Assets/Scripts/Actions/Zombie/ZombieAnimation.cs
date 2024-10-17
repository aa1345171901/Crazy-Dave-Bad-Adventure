using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.Events;

public class ZombieAnimation : MonoBehaviour
{
    [Tooltip("僵尸种类")]
    public ZombieType zombieType;

    [Tooltip("生成的墓碑的父物体")]
    public Transform GraveMonumentContent;

    [Tooltip("墓碑预制体")]
    public List<GameObject> GraveMonuments;

    [Tooltip("进场动画")]
    [SpineAnimation]
    public string EntranceAnimation = "Entrance";

    [Tooltip("退场动画")]
    [SpineAnimation]
    public string WalkOffAnimation = "WalkOff";

    [Tooltip("死亡动画1")]
    [SpineAnimation]
    public string DeadAnimation = "Dead/Dead";

    [Tooltip("死亡动画2")]
    [SpineAnimation]
    public string DeadAnimation2 = "Dead/DeadHeadFly";

    [Tooltip("死亡动画身体起飞")]
    [SpineAnimation]
    public string DeadFlyAnimation = "Dead/DeadBodyFly";

    [Tooltip("死亡动画身体起飞落地后")]
    [SpineAnimation]
    public string DeadFlyAfterAnimation = "Dead/DeadBodyFly_After";

    public ParticleSystem EarthParticle;

    public BoxCollider2D CollisionAttack;

    public ZombieFly zombieFly;

    public ZombieProp zombieProp;
    public RandomEquip randomEquip;

    public UnityEvent dead;

    [SpineSkin]
    public string charredSkin;
    [SpineSkin]
    public string defalutSkin;

    [Tooltip("被炸死动画")]
    [SpineAnimation]
    public string DeadCharredAnimation = "charred";

    private Animator graveMonumentAnimator;
    private Character character;
    private AIMove aiMove;

    private BoxCollider2D[] boxColliders;

    private void Start()
    {
        int index = Random.Range(0, GraveMonuments.Count);
        if (GraveMonuments.Count > 0)
        {
            var go = Instantiate(GraveMonuments[index], GraveMonumentContent);
            graveMonumentAnimator = go.GetComponent<Animator>();
        }
        character = this.transform.GetComponentInParent<Character>();
        boxColliders = this.transform.GetComponentsInParent<BoxCollider2D>();
        aiMove = character.FindAbility<AIMove>();
        Reuse();
        CollisionAttack.enabled = false;
        if (zombieProp != null)
        {
            zombieProp.zombieType = this.zombieType;
            zombieProp.character = this.character;
        }
    }

    /// <summary>
    /// 对象池重复利用
    /// </summary>
    public void Reuse()
    {
        character.SkeletonAnimation.ClearState();
        if (randomEquip != null)
            randomEquip.ResumeEquip();
        else
            character.SkeletonAnimation.Skeleton.Skin = character.SkeletonAnimation.SkeletonDataAsset.GetSkeletonData(true).FindSkin(defalutSkin);

        // 播放生成动画
        var entry = character.SkeletonAnimation.AnimationState.SetAnimation(0, EntranceAnimation, false);
        entry.Complete += (e) =>
        {
            character.State.AIStateType = AIStateType.Run;
            character.Health.Reuse();
            character.Reuse();
            CollisionAttack.enabled = true;
            LevelManager.Instance.Enemys.Add(zombieType, character);  // 进场动画期间不能被攻击
            SetBoxCollider(true);
        };

        SetBoxCollider(false);
        if (GraveMonuments.Count > 0)
        {
            graveMonumentAnimator.SetTrigger("Init");
            // 设置渲染层级
            graveMonumentAnimator.GetComponentInChildren<SpriteRenderer>().sortingOrder = EarthParticle.GetComponent<ParticleSystemRenderer>().sortingOrder = character.LayerOrder + 1;
            EarthParticle.Play();
        }
        character.State.AIStateType = AIStateType.Init;
        zombieFly?.Reuse();
        zombieProp?.Reuse();
        character.gameObject.SetActive(true);

        if (zombieType == ZombieType.Gargantuan && LevelManager.Instance.Enemys.Get(ZombieType.Boss).Zombies.Count <= 0)
            LevelManager.Instance.DurationPerWave = 180;
    }

    /// <summary>
    /// 每波攻势结束后没死的僵尸进行退场
    /// </summary>
    public void WalkOff()
    {
        character.IsDead = true;

        // 播放退场动画
        var entry = character.SkeletonAnimation.AnimationState.SetAnimation(0, WalkOffAnimation, false);
        entry.Complete += (e) =>
        {
            character.gameObject.SetActive(false);
            LevelManager.Instance.CacheEnemys.Add(zombieType, character);
        };
        if (GraveMonuments.Count > 0)
        {
            graveMonumentAnimator.SetTrigger("Init");
            graveMonumentAnimator.GetComponentInChildren<SpriteRenderer>().sortingOrder = EarthParticle.GetComponent<ParticleSystemRenderer>().sortingOrder = character.LayerOrder + 1;
            EarthParticle.Play();
        }
        CollisionAttack.enabled = false;
        SetBoxCollider(false);
    }

    private void DelayDeadAudio()
    {
        dead?.Invoke();
    }

    public void Dead(DamageType damageType)
    {
        character.SkeletonAnimation.AnimationState.ClearTrack(1);
        character.SkeletonAnimation.AnimationState.ClearTrack(0);
        if (damageType == DamageType.Bomb)
        {
            character.SkeletonAnimation.Skeleton.Skin = character.SkeletonAnimation.SkeletonDataAsset.GetSkeletonData(true).FindSkin(charredSkin);
            var entry = character.SkeletonAnimation.AnimationState.SetAnimation(0, DeadCharredAnimation, false);
            entry.Complete += (e) =>
            {
                character.gameObject.SetActive(false);
                LevelManager.Instance.CacheEnemys.Add(zombieType, character);
                character.SkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = -1;
            };
        }
        else if (damageType == DamageType.Chomper || damageType == DamageType.Gravebuster)
        {
            character.gameObject.SetActive(false);
            LevelManager.Instance.CacheEnemys.Add(zombieType, character);
            character.SkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = -1;
        }
        else if (zombieType == ZombieType.Balloon || zombieType == ZombieType.Catapult || zombieType == ZombieType.Zamboni || zombieType == ZombieType.Gargantuan)
        {
            var entry = character.SkeletonAnimation.AnimationState.SetAnimation(0, DeadAnimation, false);
            Invoke("DelayDeadAudio", 2.16f);
            entry.Complete += (e) =>
            {
                character.gameObject.SetActive(false);
                LevelManager.Instance.CacheEnemys.Add(zombieType, character);
                character.SkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = -1;
            };
        }
        else
        {
            int index = Random.Range(0, 11);
            string deadStr = index >= 6 ? DeadFlyAfterAnimation : DeadAnimation;
            bool isBodyFly = false;
            bool isZombieShock = GameManager.Instance.IsZombieShock;

            if (damageType == DamageType.Player && isZombieShock)
            {
                if (aiMove.AIParameter.attackPos == AIParameter.AttackPos.Body)
                {
                    isBodyFly = true;
                    deadStr = DeadFlyAnimation;
                    zombieFly.SetZombieFly(aiMove.RepulsiveForce, false, () => {
                        var entry = character.SkeletonAnimation.AnimationState.SetAnimation(0, DeadFlyAfterAnimation, false);
                        entry.Complete += (e) =>
                        {
                            zombieFly.CloseFly();
                            character.gameObject.SetActive(false);
                            LevelManager.Instance.CacheEnemys.Add(zombieType, character);
                            character.SkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = -1;
                        };
                    });
                }
                else
                {
                    deadStr = DeadAnimation2;
                    zombieFly.SetZombieFly(aiMove.RepulsiveForce);
                }
            }

            var entry = character.SkeletonAnimation.AnimationState.SetAnimation(0, deadStr, false);
            if (!isBodyFly)
                entry.Complete += (e) =>
                {
                    zombieFly.CloseFly();
                    character.gameObject.SetActive(false);
                    LevelManager.Instance.CacheEnemys.Add(zombieType, character);
                    character.SkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = -1;
                };
        }

        LevelManager.Instance.Enemys.Remove(zombieType, character);
        CollisionAttack.enabled = false;
        SetBoxCollider(false);

        if (zombieType == ZombieType.Gargantuan)
            GargantuanDead();

        GameManager.Instance.HeadNum += ConfManager.Instance.confMgr.growParam.GetGrowPrice((int)zombieType);
        SaveManager.Instance.externalGrowthData.HeadNum += ConfManager.Instance.confMgr.growParam.GetGrowPrice((int)zombieType);
        AchievementManager.Instance.SetAchievementType7((int)damageType);
        AchievementManager.Instance.SetAchievementType8((int)zombieType);
    }

    /// <summary>
    /// 巨人僵尸死亡并且没有巨人将该波时间重新设置为60
    /// </summary>
    private void GargantuanDead()
    {
        if (LevelManager.Instance.Enemys.Get(ZombieType.Gargantuan).Zombies.Count <= 0 && LevelManager.Instance.Enemys.Get(ZombieType.Boss).Zombies.Count <= 0)
        {
            if (LevelManager.Instance.IndexWave == 9 || LevelManager.Instance.IndexWave == 14)
                LevelManager.Instance.DurationPerWave = 60;
            else
                LevelManager.Instance.DurationPerWave = 92;
        }
    }

    private void SetBoxCollider(bool enabled)
    {
        for (int i = 0; i < boxColliders.Length; i++)
        {
            boxColliders[i].enabled = enabled;
        }
    }
}
