using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PropFall : MonoBehaviour
{
    public Trigger2D trigger2D;
    public AudioSource audioSource;
    public Character character;
    public DamageType damageType;
    public float RandomRange = 0.01f;

    private float height;
    private float time;
    private float curTime;
    private Vector3 offsetSpeed;

    private Vector3 direction;
    private float angle;
    private bool isTrigger;  // 是否已经触发碰撞
    private float speed;
    private float LiveTime = 2;
    private readonly float flyTime = 0.3f;

    public bool IsAbsorbed { get; set; }

    private void Start()
    {
        if (!IsAbsorbed)
        {
            Vector3 offset = new Vector3(Random.Range(0, RandomRange), Random.Range(-RandomRange, RandomRange), 0);
            height = Random.Range(RandomRange, RandomRange * 2);
            if (GameManager.Instance.IsZombieShock)
                time = Random.Range(0.6f, 0.9f);
            else
                time = Random.Range(0.4f, 0.6f);
            offsetSpeed = offset / time;
            Invoke("DestroyProp", LiveTime);
            audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
            speed = GameManager.Instance.UserData.Power / 5 + 2;
            direction = transform.position - GameManager.Instance.Player.transform.position;
            direction = new Vector3(direction.x, direction.y, transform.position.z).normalized;
        }
    }

    private void DestroyProp()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (!IsAbsorbed)
        {
            if (curTime < time)
            {
                if (GameManager.Instance.IsZombieShock && damageType == DamageType.Player)
                {
                    float process = 1 - curTime / time;

                    Trigger(trigger2D);

                    // 碰撞了就往反向飞剩余距离
                    if (!isTrigger)
                    {
                        transform.Translate(direction * speed * process * Time.deltaTime, Space.World);
                        angle += Random.Range(-1, 0);
                    }
                    else
                    {
                        transform.Translate(-direction * speed * process * Time.deltaTime, Space.World);
                        angle -= Random.Range(-1, 0);
                    }

                    transform.rotation = Quaternion.Euler(0, 0, angle);

                    if (curTime < flyTime)
                        transform.Translate(Vector3.down * curTime * 9.8f * Time.deltaTime, Space.World);
                }
                else
                {
                    curTime += Time.deltaTime;
                    Vector3 newPos = this.transform.position + offsetSpeed * curTime;
                    newPos.y += Mathf.Cos(curTime / time * Mathf.PI - Mathf.PI / 2) * height;
                    newPos.z = offsetSpeed.z;
                    transform.position = newPos;
                }
            }
        }
    }

    private void Trigger(Trigger2D trigger)
    {
        if (GameManager.Instance.IsZombieShock && trigger.IsTrigger && !isTrigger && curTime < flyTime * 2)
        {
            Character zombie = trigger.Target.GetComponent<Character>();
            if (zombie != null && zombie != character)
            {
                zombie.Health.DoDamage(GameManager.Instance.ZombieFlyDamage, DamageType.ZombieFly);
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.Play();
                isTrigger = true;
            }
        }
    }
}
