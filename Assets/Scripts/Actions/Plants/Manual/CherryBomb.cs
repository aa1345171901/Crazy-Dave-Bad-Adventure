using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class CherryBomb : AshPlant
{
    protected override void Boom()
    {
        base.Boom();
        int sumHealth = 0;
        var colliders = Physics2D.OverlapCircleAll(this.transform.position, finalRange, TargetLayer);
        DoDamage(colliders, ref sumHealth);

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
        audioSource.clip = boom;
        audioSource.Play();
        animator.Play("Boom");
        yield return new WaitForSeconds(0.5f);
        Boom();
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
