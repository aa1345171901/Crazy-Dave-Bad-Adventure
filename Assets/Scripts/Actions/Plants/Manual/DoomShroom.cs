using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class DoomShroom : AshPlant
{
    private bool canPlace;

    private CraterPos craterPos;

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
        if (canPlace)
            base.PlacePlant();
    }

    protected override void Boom()
    {
        base.Boom();
        int sumHealth = 0;
        var enemys = new List<ZombieDicts>(LevelManager.Instance.Enemys);
        foreach (var item in enemys)
        {
            foreach (var zombie in item.Zombies)
            {
                var health = zombie.Health;
                float random = Random.Range(0, 1f);
                // ¡¢º¥À¿Õˆ
                if (random < immediateMortalityRate)
                {
                    sumHealth += health.maxHealth;
                    health.DoDamage(health.maxHealth, DamageType.Bomb, true);
                }
                else
                {
                    sumHealth += finalDamage > health.health ? health.health : finalDamage;
                    health.DoDamage(finalDamage, DamageType.Bomb);
                }
            }
        }

        if (increasedInjury > 0)
        {
            // todo ¥Û–ÕΩ© ¨
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
