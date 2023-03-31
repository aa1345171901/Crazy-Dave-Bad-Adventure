using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class FinishGetGold : MonoBehaviour
{
    public Animator animator;
    public Image image;
    public Sprite sliver;
    public Sprite goldSprite;
    public Sprite diamond;

    private int playAudioCount;
    private int gold;
    private int finishGold;

    private readonly int MaxAudioCount = 4;  // 最多播放4次

    public void GetGold()
    {
        image.SetNativeSize();
        gold = GameManager.Instance.UserData.GoldCoins;
        finishGold = gold + ShopManager.Instance.Money;
        if (gold <= 50)
        {
            playAudioCount = (9 + gold) / 10;
            image.sprite = sliver;
        }
        else if (gold <= 250)
        {
            playAudioCount = (49 + gold) / 50;
            image.sprite = goldSprite;
        }
        else
        {
            playAudioCount = (249 + gold) / 250;
            image.sprite = diamond;
        }
        playAudioCount = playAudioCount > MaxAudioCount ? MaxAudioCount : playAudioCount;
        StartCoroutine(PlayGoldAnimAndSound());
    }

    IEnumerator PlayGoldAnimAndSound()
    {
        for (int i = 0; i < playAudioCount; i++)
        {
            animator.SetTrigger("GetMoney");
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.PlayEffectSoundByName("finalGoldCoin", Random.Range(0.8f,1.2f));
            ShopManager.Instance.Money += gold / playAudioCount;
        }
        ShopManager.Instance.Money = finishGold;
    }
}
