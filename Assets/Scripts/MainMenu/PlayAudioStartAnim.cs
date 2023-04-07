using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PlayAudioStartAnim : MonoBehaviour
{
    public AudioSource audioSource;
    public ParticleSystem particleSystem1;

    private void Start()
    {
        this.audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
    }

    public void PlayAudio()
    {
        audioSource.Play();
        int index = Random.Range(1, 7);
        string animName = "Zombie" + index;
        AudioManager.Instance.PlayEffectSoundByName(animName);
    }

    public void PlayParticle()
    {
        particleSystem1.Play();
    }
}
