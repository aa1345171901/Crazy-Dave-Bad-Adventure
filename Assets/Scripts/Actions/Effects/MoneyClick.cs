using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class MoneyClick : MonoBehaviour
{
    [Tooltip("钱币价格")]
    public int Price;

    [Tooltip("可存在时间")]
    public int AvailableTime;

    public AudioSource audioSource;

    public bool IsExit { get; private set; }

    private Vector3 target;
    private Vector3 speed;
    private readonly float destroyTime = 0.5f;

    private void Start()
    {
        Destroy(gameObject, AvailableTime);
    }

    private void Update()
    {
        if (IsExit)
            PlayExitAnimation();
        else
            PlayAnimation();
    }

    private void OnMouseEnter()
    {
        OnClick();
    }

    private void OnMouseDown()
    {
        OnClick();
    }

    protected virtual void OnClick()
    {
        audioSource.pitch = Random.Range(0.8f, 1.5f);
        audioSource.Play();
        ShopManager.Instance.Money += Price;
        IsExit = true;
        target = Camera.main.ViewportToWorldPoint(new Vector3(-10, 10f, -Camera.main.transform.position.z));
        speed = (target - transform.position) / destroyTime / 10;
        Destroy(gameObject, destroyTime);
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
