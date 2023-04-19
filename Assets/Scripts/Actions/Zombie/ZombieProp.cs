using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ZombieProp : MonoBehaviour
{
    public ZombieType zombieType;
    public Character character;

    [Tooltip("���𶯻�1")]
    [SpineAnimation]
    public string WornAnimation1;
    [Tooltip("���𶯻�2")]
    [SpineAnimation]
    public string WornAnimation2;
    [Tooltip("���ߵ��䶯��")]
    [SpineAnimation]
    public string PropLostAnimation;

    public PropFall fallProp;

    private int animIndex; // ��һ�����õĶ����� �����ظ����ö���

    public bool IsFall { get; set; }

    public void Reuse()
    {
        IsFall = false;
        animIndex = 0;
    }


    public void Injure(DamageType damageType)
    {
        int maxHp = character.Health.maxHealth;
        int hp = character.Health.health;
        if (hp <= maxHp / 4)
        {
            if (animIndex != 1)
            {
                animIndex = 1;
                character.SkeletonAnimation.AnimationState.SetAnimation(2, PropLostAnimation, false);
                IsFall = true;
                if (fallProp != null)
                {
                    var prop = GameObject.Instantiate(fallProp);
                    prop.damageType = damageType;
                    prop.character = character;
                    prop.transform.position = this.transform.position + new Vector3(0, 1, 0);
                    prop.GetComponent<SpriteRenderer>().sortingOrder = character.LayerOrder + 1;
                }
            }
        }
        else if (hp <= maxHp / 2)
        {
            if (animIndex != 2)
            {
                animIndex = 2;
                character.SkeletonAnimation.AnimationState.SetAnimation(2, WornAnimation2, false);
            }
        }
        else if (hp <= maxHp * 3 / 4)
        {
            if (animIndex != 3)
            {
                character.SkeletonAnimation.AnimationState.SetAnimation(2, WornAnimation1, false);
                animIndex = 3;
            }
        }
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
