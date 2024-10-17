using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class LifeResumeProp : MonoBehaviour
{
    public float LifeResume = 0.1f;

    public AudioSource audioSource;
    public AudioClip eat;

    private float height;  // 跳跃高度
    private float time;  // 跳跃持续时间
    private Vector3 offsetSpeed;  // 跳跃偏移速度
    private float curTime;

    private bool isEat;

    public PumpkinHead PumpkinHead { get; set; }

    private void Start()
    {
        Vector3 offset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
        height = Random.Range(0.05f, 0.1f);
        time = Random.Range(0.05f, 0.1f);
        offsetSpeed = offset / time;
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
    }

    private void Update()
    {
        if (curTime < time)
        {
            curTime += Time.deltaTime;
            Vector3 newPos = this.transform.position + offsetSpeed * curTime;
            newPos.y += Mathf.Cos(curTime / time * Mathf.PI - Mathf.PI / 2) * height;
            transform.position = newPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEat || GameManager.Instance.Player.Health.health == GameManager.Instance.Player.Health.maxHealth)
            return;
        var character = collision.GetComponent<Character>();
        if (character == GameManager.Instance.Player)
        {
            isEat = true;
            audioSource.clip = eat;
            audioSource.Play();
            GameManager.Instance.AddHP((int)(character.Health.maxHealth * LifeResume));
            Invoke("DelayDestroy", 0.15f);
        }
    }

    private void DelayDestroy()
    {
        PumpkinHead.RemovePumpkin(this);
        Destroy(this.gameObject);
    }
}
