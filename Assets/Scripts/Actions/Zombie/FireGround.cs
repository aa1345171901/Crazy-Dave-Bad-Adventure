using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class FireGround : MonoBehaviour
{
    public int damage = 1;

    [Tooltip("存在时间")]
    public float liveTime = 20;

    private float lastTimer;  // 上次受伤时间

    private void Start()
    {
        Invoke("DestroyDelay", liveTime);
        this.damage = (int)(LevelManager.Instance.IndexWave / 5f);
    }

    private void DestroyDelay()
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time - lastTimer < 1)
            return;
        if (collision.gameObject == GameManager.Instance.Player.gameObject)
        {
            lastTimer = Time.time;
            GameManager.Instance.DoDamage(damage, DamageType.Fire);
        }
    }
}
