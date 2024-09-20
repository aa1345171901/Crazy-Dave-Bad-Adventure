using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowPage : MonoBehaviour
{
    public MainMenu mainMenu;

    public Button btnMenu;
    public Button btnReduction;

    private void Start()
    {
        btnMenu.onClick.AddListener(OnMainMenu);
    }

    void OnMainMenu()
    {
        this.gameObject.SetActive(false);
        mainMenu.animator.Play("Enter", 0, 0);
        var animator = mainMenu.btnGrow.GetComponent<Animator>();
        mainMenu.btnGrow.GetComponent<UIEventListener>().enabled = true;
        animator.Play("idel", 0, 0);
    }
}
