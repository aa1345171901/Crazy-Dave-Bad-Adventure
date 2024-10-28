using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : BasePanel
{
    public Button btnMainmenu;

    private Animator animator;

    public MainMenu mainMenu { get; set; }

    private void Start()
    {
        btnMainmenu.onClick.AddListener(OnClose);
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
