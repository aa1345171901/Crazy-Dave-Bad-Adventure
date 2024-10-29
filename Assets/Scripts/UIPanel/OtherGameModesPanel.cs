using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class OtherGameModesPanel : BasePanel
{
    public Button btnMainmenu;
    public Text Title;
    public Text trophyNum;

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

    public void InitData(BattleMode battleMode)
    {
        switch (battleMode)
        {
            case BattleMode.PropMode:
                Title.text = GameTool.LocalText("mainmenu_prop");
                break;
            case BattleMode.PlantMode:
                Title.text = GameTool.LocalText("mainmenu_plant");
                break;
            case BattleMode.PlayerMode:
                Title.text = GameTool.LocalText("mainmenu_player");
                break;
            default:
                break;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }
}
