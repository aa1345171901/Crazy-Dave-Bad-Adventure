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
    public UIButton btnBackInTime;

    private Image targetTips;
    private readonly float fadeTimer = 0.1f;
    private readonly float showTimer = 3f;

    DamageStatisticsPanel damageStatisticsPanel;

    private void Start()
    {
        btnBackInTime.OnClick.AddListener(OnBackInTime);
    }

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
        if (GameManager.Instance.IsEnd && tipsType != TipsType.GameOver)
            return;
        StopCoroutine(FadeInOut());
        switch (tipsType)
        {
            case TipsType.GameOver:
                GameOverTips.gameObject.SetActive(true);
                Approaching.gameObject.SetActive(false);
                FinalWave.gameObject.SetActive(false);
                targetTips = GameOverTips;

                UIManager.Instance.PushPanel(UIPanelType.AttributePanel);
                BagPanel bagpanel = UIManager.Instance.PushPanel(UIPanelType.BagPanel) as BagPanel;
                bagpanel.AutoClose = false;

                btnBackInTime.gameObject.SetActive(GameManager.Instance.canBackInTime);
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
        SceneManager.LoadScene(1);
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenDamageStatistics()
    {
        if (damageStatisticsPanel == null || !damageStatisticsPanel.gameObject.activeSelf)
        {
            damageStatisticsPanel = UIManager.Instance.PushPanel(UIPanelType.DamageStatisticsPanel) as DamageStatisticsPanel;
        }
        else
        {
            UIManager.Instance.PopPanel();
        }
    }

    void OnBackInTime()
    {
        // 回档
        SaveManager.Instance.BackInTime();
        ReStartGame();
    }
}
