using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryPanel : BasePanel
{
    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        this.transform.SetSiblingIndex(this.transform.parent.childCount - 1);  // �������һ����Ⱦ
        AudioManager.Instance.PlayMenuMusic(0.2f);
        UIManager.Instance.PushPanel(UIPanelType.AttributePanel);
        BagPanel bagpanel = UIManager.Instance.PushPanel(UIPanelType.BagPanel) as BagPanel;
        bagpanel.AutoClose = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
        AudioManager.Instance.SaveVolumeData();  // ֻ���˳�����ʱ������������
        AudioManager.Instance.ResumeMusic();
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(1);
        SaveManager.Instance.DeleteUserData();
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
