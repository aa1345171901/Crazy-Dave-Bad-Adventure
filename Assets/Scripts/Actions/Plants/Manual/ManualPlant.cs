using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ManualPlant : Plant
{
    [Tooltip("攻击伤害")]
    public int Damage = 50;
    [Tooltip("冷却时间")]
    public float CoolTime = 15f;
    [Tooltip("攻击目标")]
    public LayerMask TargetLayer;
    [Tooltip("在手上时一直跟着鼠标的图片")]
    public SpriteRenderer image;
    [Tooltip("播放植物动画的图片")]
    public SpriteRenderer plant;

    public AudioSource audioSource;

    protected int finalDamage;
    protected float finalCoolTime;
    protected int sunPrice;

    protected Card card;

    /// <summary>
    /// 是否还在手上
    /// </summary>
    public bool IsManual { get; protected set; } = true;

    public virtual void InitPlant(Card card, int sun)
    {
        this.card = card;
        this.sunPrice = sun;
        card.coolTimer = finalCoolTime;
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
    }

    private void Update()
    {
        Processblity();
    }

    protected virtual void Processblity()
    {
        if (Input.GetMouseButtonDown(0) && IsManual)
        {
            PlacePlant();
        }
        // 鼠标右键取消
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1) && IsManual)
        {
            GameObject.Destroy(this.gameObject);
        }

#if UNITY_ANDROID
        if (IsManual)
        {
            bool isPlace = true;
            Vector2 touchPos = Vector2.zero;
            if (Input.touchCount > 0)
            {
                var touches = Input.touches;
                for (int i = 0; i < touches.Length; i++)
                {
                    if (GetBounds().Contains(touches[i].position))
                    {
                        isPlace = false;
                        touchPos = touches[i].position;
                        break;
                    }
                }
            }

            if (isPlace)
                PlacePlant();
            else
            {
                var targetPos = Camera.main.ScreenToWorldPoint(touchPos);
                float x = targetPos.x - targetPos.x % 0.5f;
                // 0.5 刚好站在格子上
                float y = targetPos.y - targetPos.y % 0.5f;
                this.transform.position = new Vector3(x, y, 0);
                int sortingOrder = (int)((-y + 10) * 10);
                plant.sortingOrder = sortingOrder;
                image.sortingOrder = sortingOrder;
                image.transform.position = new Vector3(targetPos.x, targetPos.y, 0);
            }
        }
#else
        if (IsManual)
        {
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = targetPos.x - targetPos.x % 0.5f;
            // 0.5 刚好站在格子上
            float y = targetPos.y - targetPos.y % 0.5f;
            this.transform.position = new Vector3(x, y, 0);
            int sortingOrder = (int)((-y + 10) * 10);
            plant.sortingOrder = sortingOrder;
            image.sortingOrder = sortingOrder;
            image.transform.position = new Vector3(targetPos.x, targetPos.y, 0);
        }
#endif
    }

    private Rect GetBounds()
    {
        var screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        screenPos.x -= 500 / 2;
        screenPos.y -= 500 / 2;
        return new Rect(screenPos, new Vector2(500, 500));
    }

    protected virtual void PlacePlant()
    {
        audioSource.Play();
        IsManual = false;
        GardenManager.Instance.Sun -= sunPrice;
        image.gameObject.SetActive(false);
        plant.color = Color.white;
        card.PlacePlant();
        AchievementManager.Instance.SetAchievementType9((int)plantAttribute.plantCard.plantType);
    }
}
