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
}
