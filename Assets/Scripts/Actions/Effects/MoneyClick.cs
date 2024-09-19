using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class MoneyClick : MonoBehaviour
{
    [Tooltip("钱币或阳光价格")]
    public int Price;

    [Tooltip("可存在时间")]
    public int AvailableTime;

    public AudioSource audioSource;

    public bool IsExit { get; protected set; }

    protected bool isDigest;
    protected Vector3 target;
    protected Vector3 speed;
    protected readonly float destroyTime = 0.5f;

    private float radius = 200;

    private void Start()
    {
        Destroy(gameObject, AvailableTime);
    }

    private void Update()
    {
        if (IsExit)
            PlayExitAnimation();
        else
        {
            PlayAnimation();
#if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                var touches = Input.touches;
                for (int i = 0; i < touches.Length; i++)
                {
                    if (GetBounds().Contains(touches[i].position))
                    {
                        OnClick();
                        break;
                    }
                }
            }
#endif
        }
    }

    private Rect GetBounds()
    {
        var screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        screenPos.x -= radius / 2;
        screenPos.y -= radius / 2;
        return new Rect(screenPos, new Vector2(radius, radius));
    }

    private void OnMouseEnter()
    {
        if (GameManager.Instance.HaveMagnetic)
            OnClick();
    }

    private void OnMouseDown()
    {
        OnClick();
    }

    public virtual void OnClick()
    {
        if (IsExit)
            return;
        audioSource.pitch = Random.Range(0.8f, 1.5f);
        NumAdd();
        audioSource.Play();
        IsExit = true;
        target = Camera.main.ViewportToWorldPoint(new Vector3(-10, 10f, -Camera.main.transform.position.z));
        speed = (target - transform.position) / destroyTime / 10;
        Destroy(gameObject, destroyTime);
    }

    public void Digest()
    {
        isDigest = true;
        OnClick();
    }

    public void Collect()
    {
        audioSource.pitch = Random.Range(0.8f, 1.5f);
        NumAdd();
        audioSource.Play();
        Destroy(gameObject, 0.1f);
    }

    protected virtual void NumAdd()
    {
        ShopManager.Instance.Money += Price;
    }

    protected virtual void PlayAnimation()
    {

    }

    protected virtual void PlayExitAnimation()
    {
        transform.Translate(speed * Time.deltaTime);
    }

    private void OnEnable()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
    }
}
