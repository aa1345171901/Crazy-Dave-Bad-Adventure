using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ZombieAnimation : MonoBehaviour
{
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
    public string DeadAnimation = "Dead";

    [Tooltip("��������2")]
    [SpineAnimation]
    public string DeadAnimation2 = "DeadHeadFly";

    [Tooltip("���������������")]
    [SpineAnimation]
    public string DeadFlyAnimation = "DeadBodyFly";

    [Tooltip("�����������������غ�")]
    [SpineAnimation]
    public string DeadFlyAfterAnimation = "DeadBodyFly_After";

    public ParticleSystem EarthParticle;

    public BoxCollider2D CollisionAttack;

    public ZombieFly zombieFly;

    private Animator graveMonumentAnimator;
    private Character character;
    private AIMove aiMove;

    private BoxCollider2D[] boxColliders;

    private void Start()
    {
        int index = Random.Range(0, GraveMonuments.Count);
        var go = Instantiate(GraveMonuments[index], GraveMonumentContent);
        graveMonumentAnimator = go.GetComponent<Animator>();
        character = this.transform.GetComponentInParent<Character>();
        boxColliders = this.transform.GetComponentsInParent<BoxCollider2D>();
        aiMove = character.FindAbility<AIMove>();
        Reuse();
        CollisionAttack.enabled = false;
    }

    /// <summary>
    /// ������ظ�����
    /// </summary>
    public void Reuse()
    {
        character.SkeletonAnimation.ClearState();

        // ������Ⱦ�㼶
        graveMonumentAnimator.GetComponentInChildren<SpriteRenderer>().sortingOrder = EarthParticle.GetComponent<ParticleSystemRenderer>().sortingOrder = character.LayerOrder + 1;

        // �������ɶ���
        var entry = character.SkeletonAnimation.AnimationState.SetAnimation(0, EntranceAnimation, false);
        entry.Complete += (e) =>
        {
            character.State.AIStateType = AIStateType.Run;
            character.Health.Reuse();
            character.Reuse();
            CollisionAttack.enabled = true;
            LevelManager.Instance.Enemys.Add(character);  // ���������ڼ䲻�ܱ�����
            SetBoxCollider(true);
        };

        SetBoxCollider(false);
        graveMonumentAnimator.SetTrigger("Init");
        EarthParticle.Play();
        character.State.AIStateType = AIStateType.Init;
        zombieFly.Reuse();
        character.gameObject.SetActive(true);
    }

    /// <summary>
    /// ÿ�����ƽ�����û���Ľ�ʬ�����˳�
    /// </summary>
    public void WalkOff()
    {
        character.IsDead = true;
        character.SkeletonAnimation.ClearState();

        graveMonumentAnimator.GetComponentInChildren<SpriteRenderer>().sortingOrder = EarthParticle.GetComponent<ParticleSystemRenderer>().sortingOrder = character.LayerOrder + 1;

        // �����˳�����
        var entry = character.SkeletonAnimation.AnimationState.SetAnimation(0, WalkOffAnimation, false);
        entry.Complete += (e) =>
        {
            character.gameObject.SetActive(false);
            LevelManager.Instance.CacheEnemys.Add(character);
        };

        graveMonumentAnimator.SetTrigger("Init");
        EarthParticle.Play();
        CollisionAttack.enabled = false;
        SetBoxCollider(false);
    }

    public void Dead(DamageType damageType)
    {
        if (damageType == DamageType.Chomper || damageType == DamageType.Gravebuster)
        {
            character.SkeletonAnimation.ClearState();
            character.gameObject.SetActive(false);
            LevelManager.Instance.CacheEnemys.Add(character);
            character.SkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = -1;
        }
        else
        {
            character.SkeletonAnimation.ClearState();
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
                            LevelManager.Instance.CacheEnemys.Add(character);
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
                    LevelManager.Instance.CacheEnemys.Add(character);
                    character.SkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = -1;
                };
        }

        LevelManager.Instance.Enemys.Remove(character);
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
