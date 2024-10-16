using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class DoomShroom : AshPlant
{
    private bool canPlace;
    bool isSeed;
    private CraterPos craterPos;

    public override void Reuse(bool randomPos = true)
    {
        isSeed = !randomPos;
        base.Reuse(randomPos);
    }

    protected override void Processblity()
    {
        base.Processblity();

        craterPos = new CraterPos(this.transform.position.x, this.transform.position.y);
        if (JudgePlace())
        {
            canPlace = true;
            plant.enabled = true;
        }
        else
        {
            canPlace = false;
            plant.enabled = false;
        }
    }

    private bool JudgePlace()
    {
        var craterPoses = GardenManager.Instance.CraterPoses;
        foreach (var item in craterPoses)
        {
            if (item.x == craterPos.x && item.y == craterPos.y)
                return false;
        }
        return true;
    }

    protected override void PlacePlant()
    {
        if (canPlace || isSeed)
            base.PlacePlant();
    }

    protected override void Boom()
    {
        base.Boom();
        int sumHealth = 0;
        var enemys = new List<ZombieDicts>(LevelManager.Instance.Enemys);
        foreach (var item in enemys)
        {
            var zombies = new List<Character>(item.Zombies);
            foreach (var zombie in zombies)
            {
                var health = zombie.Health;
                float random = Random.Range(0, 1f);
                // 立即死亡
                if (random < immediateMortalityRate && zombie.tag != "BigZombie")
                {
                    sumHealth += health.maxHealth;
                    health.DoDamage(health.maxHealth, DamageType.Bomb, true);
                }
                else 
                {
                    if (increasedInjury > 0 && zombie.tag == "BigZombie")
                    {
                        int damage = (int)(finalDamage * increasedInjury);
                        sumHealth += damage > health.health ? health.health : damage;
                        health.DoDamage(damage, DamageType.Bomb);
                    }
                    else
                    {
                        sumHealth += finalDamage > health.health ? health.health : finalDamage;
                        health.DoDamage(finalDamage, DamageType.Bomb);
                    }
                }
            }
        }

        if (sunConversionRate != 0)
        {
            var sunItem = GameObject.Instantiate(sun, this.transform);
            sunItem.Price = (int)(sunConversionRate * sumHealth * 5);
            sunItem.Digest();
        }
    }

    protected override IEnumerator BoomDelay()
    {
        yield return new WaitForSeconds(0.05f);
        animator.Play("Boom");
        yield return new WaitForSeconds(0.8f);
        audioSource.clip = boom;
        audioSource.Play();
        yield return new WaitForSeconds(0.3f);
        Boom();
        GardenManager.Instance.CreateCrater(this.transform.position.x, transform.position.y);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
