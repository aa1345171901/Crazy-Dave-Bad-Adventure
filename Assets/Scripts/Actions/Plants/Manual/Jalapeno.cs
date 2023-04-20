using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jalapeno : AshPlant
{
    protected override void Boom()
    {
        base.Boom();
        int sumHealth = 0;
        LayerMask targetLayer = increasedInjury > 0 ? TargetLayer : TargetLayer | BigTargetLayer;
        var colliders = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(finalRange, finalRange / Range), 0,targetLayer);
        DoDamage(colliders, ref sumHealth);

        if (increasedInjury > 0)
        {
            colliders = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(finalRange, finalRange / Range), 0, BigTargetLayer);
            IncreasedInjury(colliders, ref sumHealth);
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
        Boom();
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
