using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PotatoMine : AshPlant
{
    public float ExcavationTime = 8f;

    public ParticleSystem earth;
    public AudioClip emerge;

    private float finalExcavationTime;

    private bool isExcavation;

    private readonly float LevelExcavationTime = 0.6f;

    public override void InitPlant(Card card, int sun)
    {
        base.InitPlant(card, sun);

        finalExcavationTime = ExcavationTime;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // ×Ö¶ÎÓ³Éä
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                case 9:
                    finalExcavationTime -= LevelExcavationTime * (int)fieldInfo.GetValue(plantAttribute);
                    break;
                default:
                    break;
            }
        }
    }

    protected override void PlacePlant()
    {
        base.PlacePlant();
        StartCoroutine(Emerge());
    }

    IEnumerator Emerge()
    {
        yield return new WaitForSeconds(finalExcavationTime);
        animator.SetBool("IsEmerge", true);
        audioSource.clip = emerge;
        audioSource.Play();
        earth.Play();
        isExcavation = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isExcavation)
            return;
        if (TargetLayer.Contains(collision.gameObject.layer))
        {
            StartCoroutine(BoomTrigger());
        }
    }

    protected override void Boom()
    {
        base.Boom();
        int sumHealth = 0;
        LayerMask targetLayer = increasedInjury > 0 ? TargetLayer : TargetLayer | BigTargetLayer;
        var colliders = Physics2D.OverlapCircleAll(this.transform.position, finalRange, targetLayer);
        DoDamage(colliders, ref sumHealth);

        if (increasedInjury > 0)
        {
            colliders = Physics2D.OverlapCircleAll(this.transform.position, finalRange, BigTargetLayer);
            IncreasedInjury(colliders, ref sumHealth);
        }

        if (sunConversionRate != 0)
        {
            var sunItem = GameObject.Instantiate(sun, this.transform);
            sunItem.Price = (int)(sunConversionRate * sumHealth * 5);
            sunItem.Digest();
        }
    }

    IEnumerator BoomTrigger()
    {
        yield return new WaitForSeconds(0.05f);
        audioSource.clip = boom;
        audioSource.Play();
        animator.Play("Boom");
        yield return new WaitForSeconds(0.1f);
        Boom();
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
