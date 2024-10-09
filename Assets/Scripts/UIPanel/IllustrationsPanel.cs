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

        UpdateUI();
    }

    void UpdateUI()
    {
        CreatePlant();
        CreateZombie();
    }

    void CreatePlant()
    {
        plantRoot.DestroyChild();
        var index = Random.Range(0, ConfManager.Instance.confMgr.plantIllustrations.items.Count);
        var confPlant = ConfManager.Instance.confMgr.plantIllustrations.items[index];
        var plantGo = Resources.Load(confPlant.prefabPath);
        if (plantGo == null)
        {
            Debug.Log(confPlant.prefabPath);
        }
        GameObject.Instantiate(plantGo, plantRoot);
    }

    void CreateZombie()
    {
        zombieRoot.DestroyChild();
        var index = Random.Range(0, ConfManager.Instance.confMgr.zombieIllustrations.items.Count);
        var confZombie = ConfManager.Instance.confMgr.zombieIllustrations.items[index];
        var zombieGo = Resources.Load(confZombie.prefabPath);
        GameObject.Instantiate(zombieGo, zombieRoot);
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }
}
