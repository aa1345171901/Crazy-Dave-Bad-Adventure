using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ZombieProp : MonoBehaviour
{
    public ZombieType zombieType;
    public Character character;

    [Tooltip("受损动画1")]
    [SpineAnimation]
    public string WornAnimation1;
    [Tooltip("受损动画2")]
    [SpineAnimation]
    public string WornAnimation2;
    [Tooltip("道具掉落动画")]
    [SpineAnimation]
    public string PropLostAnimation;
    [Tooltip("爆胎")]
    [SpineAnimation]
    public string PunctureAnimation;

    public PropFall fallProp;

    private int animIndex; // 上一次设置的动画， 放置重复设置动画

    public bool IsFall { get; set; }

    public bool isPuncture { get; set; }

    public void Reuse()
    {
        IsFall = false;
        isPuncture = false;
        animIndex = 0;
    }

    public void Injure(DamageType damageType)
    {
        int maxHp = character.Health.maxHealth;
        int hp = character.Health.health;
        if (hp <= maxHp / 4 && !string.IsNullOrEmpty(PropLostAnimation))
        {
            if (animIndex != 1)
            {
                animIndex = 1;
                character.SkeletonAnimation.AnimationState.SetAnimation(2, PropLostAnimation, false);
                IsFall = true;
                if (fallProp != null)
                {
                    var prop = GameObject.Instantiate(fallProp);
                    if (zombieType != ZombieType.Paper)
                        prop.damageType = damageType;
                    else
                    {
                        var attack = character.FindAbility<NormalZombieAttack>();
                        attack.realCanSwoop = true;
                        attack.realAttackProbability *= 2;
                        var move = character.FindAbility<AIMove>();
                        move.realSpeed *= 3;
                    }
                    prop.character = character;
                    prop.transform.position = this.transform.position + new Vector3(0, 1, 0);
                    prop.GetComponent<SpriteRenderer>().sortingOrder = character.LayerOrder + 1;
                }
            }
        }
        else if (hp <= maxHp / 2 && !string.IsNullOrEmpty(WornAnimation2))
        {
            if (animIndex != 2)
            {
                animIndex = 2;
                character.SkeletonAnimation.AnimationState.SetAnimation(2, WornAnimation2, false);
            }
        }
        else if (hp <= maxHp * 3 / 4 && !string.IsNullOrEmpty(WornAnimation1))
        {
            if (animIndex != 3)
            {
                character.SkeletonAnimation.AnimationState.SetAnimation(2, WornAnimation1, false);
                animIndex = 3;
            }
        }
    }

    public void Injure()
    {
        int maxHp = character.Health.maxHealth;
        int hp = character.Health.health;
        if (hp <= maxHp / 3 && !string.IsNullOrEmpty(WornAnimation2))
        {
            if (animIndex != 2)
            {
                animIndex = 2;
                character.SkeletonAnimation.AnimationState.SetAnimation(2, WornAnimation2, false);
            }
        }
        else if (hp <= maxHp * 2 / 3 && !string.IsNullOrEmpty(WornAnimation1))
        {
            if (animIndex != 3)
            {
                character.SkeletonAnimation.AnimationState.SetAnimation(2, WornAnimation1, false);
                animIndex = 3;
            }
        }
    }

    public void Puncture()
    {
        isPuncture = true;
        var tracy = character.SkeletonAnimation.AnimationState.SetAnimation(0, PunctureAnimation, false);
        var aiMove = this.character.FindAbility<ZamboniMove>();
        aiMove.MoveSpeed = 0;
        tracy.Complete += (e) =>
        {
            this.character.Health.DoDamage(this.character.Health.maxHealth);
        };
    }

    public GameObject MagnetShroomAttack()
    {
        this.character.Health.health = this.character.Health.maxHealth / 4;
        var prop = GameObject.Instantiate(fallProp);
        prop.IsAbsorbed = true;
        IsFall = true;
        prop.transform.position = this.transform.position + new Vector3(0, 1, 0);
        return prop.gameObject;
    }
}
