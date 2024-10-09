using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class IllustrationsPanel : BasePanel
{
    public Transform plantRoot;
    public Button btnLookPlant;
    public Transform zombieRoot;
    public Button btnLookZombie;
    public Button btnClose;

    private Animator animator;

    public MainMenu mainMenu;

    private void Start()
    {
        btnClose.onClick.AddListener(OnClose);
    }

    void OnClose()
    {
        UIManager.Instance.PopPanel();
        mainMenu.OnEnterMainMenu();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        animator = GetComponent<Animator>();
        animator.Play("show");
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }
}
