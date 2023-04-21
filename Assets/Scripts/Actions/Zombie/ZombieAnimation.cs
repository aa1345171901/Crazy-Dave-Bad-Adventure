using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ZombieAnimation : MonoBehaviour
{
    [Tooltip("��ʬ����")]
    public ZombieType zombieType;

    [Tooltip("���ɵ�Ĺ���ĸ�����")]
    public Transform GraveMonumentContent;

    [Tooltip("Ĺ��Ԥ����")]
    public List<GameObject> GraveMonuments;

    [Tooltip("��������")]
    [SpineAnimation]
    public string EntranceAnimation = "Entrance";

    [Tooltip("�˳�����")]
    [SpineAnimation]
    public string WalkOffAnimation = "WalkOff";

    [Tooltip("��������1")]
    [SpineAnimation]
    public string DeadAnimation = "Dead/Dead";

    [Tooltip("��������2")]
    [SpineAnimation]
    public string DeadAnimation2 = "Dead/DeadHeadFly";

    [Tooltip("���������������")]
    [SpineAnimation]
    public string DeadFlyAnimation = "Dead/DeadBodyFly";

    [Tooltip("�����������������غ�")]
    [SpineAnimation]
    public string DeadFlyAfterAnimation = "Dead/DeadBodyFly_After";

    public ParticleSystem EarthParticle;

    public BoxCollider2D CollisionAttack;

    public ZombieFly zombieFly;

    public ZombieProp zombieProp;
    public RandomEquip randomEquip;

    [SpineSkin]
    public string charredSkin;
    [SpineSkin]
    public string defalutSkin;

    [Tooltip("��ը������")]
    [SpineAnimation]
    public string DeadCharredAnimation = "charred";

    private Animator graveMonumentAnimator;
    private Character character;
    private AIMove aiMove;

    private BoxCollider2D[] boxColliders;

    private void Start()
    {
        int index = Random.Range(0, GraveMonuments.Count);
        if (zombieType != ZombieType.Balloon)
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
    /// ������ظ�����
    /// </summary>
    public void Reuse()
    {
        character.SkeletonAnimation.ClearState();
        if (randomEquip != null)
            randomEquip.ResumeEquip();
        else
            character.SkeletonAnimation.Skeleton.Skin = character.SkeletonAnimation.SkeletonDataAsset.GetSkeletonData(true).FindSkin(defalutSkin);

        // �������ɶ���
        var entry = character.SkeletonAnimation.AnimationState.SetAnimation(0, EntranceAnimation, false);
        entry.Complete += (e) =>
        {
            character.State.AIStateType = AIStateType.Run;
            character.Health.Reuse();
            character.Reuse();
            CollisionAttack.enabled = true;
            LevelManager.Instance.Enemys.Add(zombieType, character);  // ���������ڼ䲻�ܱ�����
            SetBoxCollider(true);
        };

        SetBoxCollider(false);
        if (zombieType != ZombieType.Balloon)
        {
            graveMonumentAnimator.SetTrigger("Init");
            // ������Ⱦ�㼶
            graveMonumentAnimator.GetComponentInChildren<SpriteRenderer>().sortingOrder = EarthParticle.GetComponent<ParticleSystemRenderer>().sortingOrder = character.LayerOrder + 1;
            EarthParticle.Play();
        }
        character.State.AIStateType = AIStateType.Init;
        zombieFly?.Reuse();
        zombieProp?.Reuse();
        character.gameObject.SetActive(true);
    }

    /// <summary>
    /// ÿ�����ƽ�����û���Ľ�ʬ�����˳�
    /// </summary>
    public void WalkOff()
    {
        character.IsDead = true;

        // �����˳�����
        var entry = character.SkeletonAnimation.AnimationState.SetAnimation(0, WalkOffAnimation, false);
        entry.Complete += (e) =>
        {
            character.gameObject.SetActive(false);
            LevelManager.Instance.CacheEnemys.Add(zombieType, character);
        };
        if (zombieType != ZombieType.Balloon)
        {
            graveMonumentAnimator.SetTrigger("Init");
            graveMonumentAnimator.GetComponentInChildren<SpriteRenderer>().sortingOrder = EarthParticle.GetComponent<ParticleSystemRenderer>().sortingOrder = character.LayerOrder + 1;
            EarthParticle.Play();
        }
        CollisionAttack.enabled = false;
        SetBoxCollider(false);
    }

    public void Dead(DamageType damageType)
    {
        character.SkeletonAnimation.ClearState();
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
        else if (zombieType == ZombieType.Balloon || zombieType == ZombieType.Catapult || zombieType == ZombieType.Zamboni)
        {
            var entry = character.SkeletonAnimation.AnimationState.SetAnimation(0, DeadAnimation, false);
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

            if (damageType == DamageType.Pot && isZombieShock)
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
    }

    private void SetBoxCollider(bool enabled)
    {
        for (int i = 0; i < boxColliders.Length; i++)
        {
            boxColliders[i].enabled = enabled;
        }
    }
}
