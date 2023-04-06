using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SkeletonGraphic Dave;
    public AudioSource audioSource;
    public Animator animator;
    public ParticleSystem particleSystem1;
    public GameObject SettingPage;
    public GameObject AchievementPage;

    public List<Collider2D> Buttons;

    private void Start()
    {
        AudioManager.Instance.PlayBackMusic();
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
        Dave.AnimationState.Complete += (e) =>
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
        };
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Setting()
    {
        Time.timeScale = 0;
        SettingPage.SetActive(true);
        foreach (var item in Buttons)
        {
            item.enabled = false;
        }
    }

    public void Achievement()
    {
        AchievementPage.SetActive(true);
        Dave.AnimationState.SetAnimation(0, "MainAchievement", true);
    }

    public void ExitGame()
    {
        var track = Dave.AnimationState.SetAnimation(0, "MainMenuSelect", false);
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

    private void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
	        UnityEngine.Application.Quit();
#endif
    }
}
