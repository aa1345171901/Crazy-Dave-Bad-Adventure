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
    public GameObject AchievementPage;
    public GameObject StartGamePage;
    public GameObject GrowPage;
    public Button btnGrow;
    public Slider process;
    public Collider2D collider2d;

    private AsyncOperation asyncOperation;

    private void Start()
    {
        AudioManager.Instance.PlayBackMusic();
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
        Dave.AnimationState.Complete += PlayPotAudio;
        btnGrow.onClick.AddListener(GrowOpen);
    }

    private void PlayPotAudio(TrackEntry e)
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    public void StartGame()
    {
        if (SaveManager.Instance.JudgeData())
        {
            StartGamePage.SetActive(true);
        }
        else
        {
            PlayStartGameAnim();
        }
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        // 使用协程异步加载场景
        asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false; // 如果为true，那么加载结束后直接就会跳转
        yield return null;
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

    private void PlayStartGameAnim()
    {
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
        void SetProcess(float value)
        {
            process.value = value;
            process.handleRect.localScale = Mathf.Max((1 - value), 0.7f) * Vector3.one;
            process.handleRect.rotation = Quaternion.Euler(0, 0, -value * 720);
        }
        float nowProgress = 0;
        while (nowProgress < asyncOperation.progress)
        {
            nowProgress += 0.002f;
            SetProcess(nowProgress);
            yield return new WaitForEndOfFrame();
        }
        while (nowProgress < 1)
        {
            nowProgress += 0.01f;
            SetProcess(nowProgress);
            yield return new WaitForEndOfFrame();
        }
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

    public void Setting()
    {
        Time.timeScale = 0;
        SettingPage.SetActive(true);
    }

    public void Achievement()
    {
        // 打打僵王
        SaveManager.Instance.SetBossMode();
        PlayStartGameAnim();
        StartCoroutine(LoadScene());

        // 成就暂时不开放
        //AchievementPage.SetActive(true);
        //Dave.AnimationState.SetAnimation(0, "MainAchievement", true);
    }

    public void ExitGame()
    {
        var track = Dave.AnimationState.SetAnimation(0, "MainMenuSelect", false);
        track.TimeScale = 2;
        track.Complete += (e) =>
        {
            animator.SetTrigger("Exit");
            var track =  Dave.AnimationState.SetAnimation(0, "Dead", false);
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

    private void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
	        UnityEngine.Application.Quit();
#endif
    }
}
