using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Basketball : MonoBehaviour
{
    public float Speed;
    public Transform Target;

    public AudioSource audioSource;

    private float timer;
    public float upTimer;  // 计算上升时间，大致为初始时到目标地的一半时间
    private Vector3 startPos;
    private float upSpeed;
    private float fallSpeed;

    private float height;
    private float time;
    private Vector3 offsetSpeed;

    private bool isJump;

    private readonly float MaxLiveTime = 15;

    private void Start()
    {
        Invoke("DestroyDelay", MaxLiveTime);
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        Target = GameManager.Instance.Player.transform;
        upTimer = (Target.transform.position - this.transform.position).magnitude / Speed / 2;
        startPos = this.transform.position;

        upSpeed = fallSpeed = 5 / upTimer;
        float vOffset = Target.transform.position.y + 1.05f - this.transform.position.y;
        float speedOffset = 2 * vOffset / upTimer;
        if (speedOffset > 0)
            upSpeed = (5 + speedOffset) / upTimer;
        else
            fallSpeed = (5 - speedOffset) / upTimer;
    }

    private void Update()
    {
        if (!isJump)
        {
            if (timer < upTimer * 2)
            {
                if (timer < upTimer)
                {
                    transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
                }
                float process = timer / (upTimer * 2);
                var lerp = Vector3.Lerp(startPos, new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z), process);
                transform.position = transform.position = new Vector3(lerp.x, timer > upTimer ? lerp.y : transform.position.y, 0);

                timer += Time.deltaTime;
            }
            else
            {
                this.transform.position = Target.transform.position + Vector3.up;
                GameManager.Instance.balls.Remove(this.gameObject);
                audioSource.Play();
                isJump = true;

                Vector3 offset = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), transform.position.z);
                height = Random.Range(0.3f, 0.6f);
                time = Random.Range(0.4f, 0.6f);
                offsetSpeed = offset / time;
                startPos = this.transform.position;
                Invoke("DestroyDelay", time + 1);
                timer = 0;
            }
        }
        else
        {
            if (timer < time)
            {
                timer += Time.deltaTime;
                Vector3 newPos = startPos + offsetSpeed * timer;
                newPos.y += Mathf.Cos(timer / time * Mathf.PI - Mathf.PI / 2) * height;
                newPos.z = offsetSpeed.z;
                transform.position = newPos;
            }
        }
    }

    private void DestroyDelay()
    {
        GameObject.Destroy(this.gameObject);
    }
}
