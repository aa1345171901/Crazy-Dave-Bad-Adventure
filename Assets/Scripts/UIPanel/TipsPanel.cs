using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TipsType
{
    GameOver,
    Approaching,
    FinalWave,
}

public class TipsPanel : BasePanel
{
    public Image GameOverTips;
    public Image Approaching;
    public Image FinalWave;

    private Image targetTips;
    private readonly float fadeTimer = 0.1f;
    private readonly float showTimer = 3f;

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }

    public void SetTips(TipsType tipsType)
    {
        StopCoroutine(FadeInOut());
        switch (tipsType)
        {
            case TipsType.GameOver:
                GameOverTips.gameObject.SetActive(true);
                Approaching.gameObject.SetActive(false);
                FinalWave.gameObject.SetActive(false);
                targetTips = GameOverTips;
                break;
            case TipsType.Approaching:
                GameOverTips.gameObject.SetActive(false);
                Approaching.gameObject.SetActive(true);
                FinalWave.gameObject.SetActive(false);
                targetTips = Approaching;
                break;
            case TipsType.FinalWave:
                GameOverTips.gameObject.SetActive(false);
                Approaching.gameObject.SetActive(false);
                FinalWave.gameObject.SetActive(true);
                targetTips = FinalWave;
                break;
            default:
                break;
        }
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        this.gameObject.SetActive(true);
        for (int i = 0; i <= 1 / fadeTimer; i++)
        {
            targetTips.color = new Color(targetTips.color.r, targetTips.color.g, targetTips.color.b, i * fadeTimer);
            yield return new WaitForSeconds(fadeTimer);
        }

        if (targetTips != GameOverTips)
        {
            yield return new WaitForSeconds(showTimer);
            for (int i = 0; i <= 1 / fadeTimer; i++)
            {
                targetTips.color = new Color(targetTips.color.r, targetTips.color.g, targetTips.color.b, (1 / fadeTimer - i) * fadeTimer);
                yield return new WaitForSeconds(fadeTimer);
            }
            OnExit();
        }
    }

    public void ReStartGame()
    {
        // 重新设置模式为当前模式
        SaveManager.Instance.SetSpecialMode(GameManager.Instance.nowMode);
        SceneManager.LoadScene(1);
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
