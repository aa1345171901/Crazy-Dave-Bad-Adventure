using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SkeletonGraphic Dave;
    public AudioSource audioSource;
    public Animator animator;
    public ParticleSystem particleSystem1;
    public GameObject SettingPage;
    public AchievementPage AchievementPage;
    public GameObject StartGamePage;
    public GameObject GrowPage;
    public Button btnGrow;
    public Collider2D collider2d;
    public Sprite goldSunFlower;
    public Image achievementImg;

    private AsyncOperation asyncOperation;

    public bool playAudio { get; set; } = true;
    bool isLoadScene = false;

    private void Start()
    {
        AudioManager.Instance.PlayBackMusic();
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
        Dave.AnimationState.Complete += PlayPotAudio;
        btnGrow.onClick.AddListener(GrowOpen);
        if (AchievementManager.Instance.AllReach())
        {
            achievementImg.sprite = goldSunFlower;
        }
    }

    private void OnDestroy()
    {
        AudioManager.Instance.AudioLists.Remove(this.audioSource);
    }

    private void PlayPotAudio(TrackEntry e)
    {
        if (!playAudio)
            return;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    IEnumerator LoadLoadingScene()
    {
        if (isLoadScene)
            yield break;
        isLoadScene = true;
        // 使用协程异步加载场景
        asyncOperation = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false; // 如果为true，那么加载结束后直接就会跳转
        yield return null;
    }

    private void PlayStartGameAnim()
    {
        SaveManager.Instance.SetSpecialMode(BattleMode.None);
        StartCoroutine(LoadLoadingScene());
        collider2d.enabled = true;
        var track = Dave.AnimationState.SetAnimation(0, "MainMenuSelect", false);
        track.TimeScale = 2;
        track.Complete += (e) =>
        {
            Dave.AnimationState.SetAnimation(0, "ShopingIdel", true);
            Dave.AnimationState.Complete -= PlayPotAudio;
            PlayPotAudio(null);
            AudioManager.Instance.StopBackMusic();
            animator.SetTrigger("StartGame");
            StartCoroutine(SetAllowActivation());
        };
    }

    IEnumerator SetAllowActivation()
    {
        yield return new WaitForSeconds(0.8f);
        asyncOperation.allowSceneActivation = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (StartGamePage.activeSelf)
            {
                StartGamePage.SetActive(false);
            }
        }
    }

    #region 按钮事件
    public void StartGame()
    {
        if (isLoadScene)
            return;
        if (SaveManager.Instance.JudgeData())
        {
            StartGamePage.SetActive(true);
        }
        else
        {
            PlayStartGameAnim();
        }
    }

    public void RestartGame()
    {
        StartGamePage.SetActive(false);
        SaveManager.Instance.DeleteUserData();
        PlayStartGameAnim();
    }

    public void ContinueGame()
    {
        StartGamePage.SetActive(false);
        PlayStartGameAnim();
    }

    public void Setting()
    {
        Time.timeScale = 0;
        SettingPage.SetActive(true);
    }

    public void Achievement()
    {
        AchievementPage.OnEnter();
        Dave.AnimationState.SetAnimation(0, "MainAchievement", true);
    }

    public void ExitGame()
    {
        var track = Dave.AnimationState.SetAnimation(0, "MainMenuSelect", false);
        track.TimeScale = 2;
        track.Complete += (e) =>
        {
            animator.SetTrigger("Exit");
            var track = Dave.AnimationState.SetAnimation(0, "Dead", false);
            track.Delay = 0.3f;
            particleSystem1.Play();
            int index = Random.Range(1, 7);
            string animName = "Zombie" + index;
            AudioManager.Instance.PlayEffectSoundByName(animName);
            Invoke("Exit", 0.6f);
        };
    }

    public void GrowOpen()
    {
        Dave.AnimationState.SetAnimation(0, "Idel", true);
        playAudio = false;
        var animator = btnGrow.GetComponent<Animator>();
        btnGrow.GetComponent<UIEventListener>().enabled = false;
        animator.Play("click", 0, 0);
        IEnumerator delaySet()
        {
            yield return new WaitForSeconds(0.5f);
            GrowPage.SetActive(true);
        }
        StartCoroutine(delaySet());
    }

    public void OnIllustrationsOpen()
    {
        Dave.AnimationState.SetAnimation(0, "Idel", true);
        playAudio = false;
        var panle = UIManager.Instance.PushPanel(UIPanelType.IllustrationsPanel) as IllustrationsPanel;
        AudioManager.Instance.PlayMenuMusic(0);
        panle.mainMenu = this;
    }

    public void OnGardenOpen()
    {
        Dave.AnimationState.SetAnimation(0, "Idel", true);
        playAudio = false;
        var panle = UIManager.Instance.PushPanel(UIPanelType.ExternalGardenPanel) as ExternalGardenPanel;
        AudioManager.Instance.PlayGardenMusic();
        panle.mainMenu = this;
    }

    public void OnHelp()
    {
        var uiHelp = UIManager.Instance.PushPanel(UIPanelType.HelpPanel) as HelpPanel;
        uiHelp.mainMenu = this;
    }

    public void OnPropMode()
    {
        OnSelectMode(BattleMode.PropMode);
    }

    public void OnPlantMode()
    {
        OnSelectMode(BattleMode.PlantMode);
    }

    public void OnPlayerMode()
    {
        OnSelectMode(BattleMode.PlayerMode);
    }

    void OnSelectMode(BattleMode battleMode)
    {
        if (isLoadScene)
            return;
        Dave.AnimationState.SetAnimation(0, "Idel", true);
        playAudio = false;
        AudioManager.Instance.PlayMenuMusic(0);
        var uiOtherGameModesPanel = UIManager.Instance.PushPanel(UIPanelType.OtherGameModesPanel) as OtherGameModesPanel;
        uiOtherGameModesPanel.mainMenu = this;
        uiOtherGameModesPanel.InitData(battleMode);
    }
    #endregion

    public void OnEnterMainMenu(bool ReplayMusic = true)
    {
        animator.Play("Enter", 0, 0);
        Dave.AnimationState.SetAnimation(0, "MainMenuIdel", true);
        if (ReplayMusic)
            AudioManager.Instance.PlayBackMusic();
        playAudio = true;
    }

    private void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
	        UnityEngine.Application.Quit();
#endif
    }
}
