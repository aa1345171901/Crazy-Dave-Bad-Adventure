using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    public float speed;
    public int damage = 1;
    public GameObject boom;
    public GameObject trail;

    public Quaternion rotation;
    public LayerMask targetLayer;
    public SpriteRenderer spriteRenderer;

    AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(DelayDoDamage());

        transform.rotation = rotation;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
        int y = (int)((-this.transform.position.y + 10) * 10);
        spriteRenderer.sortingOrder = y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetLayer.Contains(collision.gameObject.layer))
        {
            var health = collision.GetComponent<Health>();
            if (health)
            {
                health.DoDamage(damage, DamageType.Gun);
                StopAllCoroutines();
                StartCoroutine(DelayDestroy());
            }
        }
    }

    IEnumerator DelayDestroy()
    {
        boom.gameObject.SetActive(true);
        speed = 0;
        spriteRenderer.gameObject.SetActive(false);
        trail.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        GameObject.Destroy(gameObject);
    }

    IEnumerator DelayDoDamage()
    {
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(gameObject);
    }
}
