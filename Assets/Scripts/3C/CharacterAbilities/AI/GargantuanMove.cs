using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class GargantuanMove : AIMove
{
    [Space(10)]
    [Header("SoundEffect")]
    [Tooltip("播放走路声音的AudioSource")]
    public AudioSource WalkAudio;

    private void OnEnable()
    {
        AudioManager.Instance.AudioLists.Add(WalkAudio);
        WalkAudio.volume = AudioManager.Instance.EffectPlayer.volume;
    }

    private void OnDisable()
    {
        AudioManager.Instance.AudioLists.Remove(WalkAudio);
    }

    public override void UpdateAnimator()
    {
        base.UpdateAnimator();
        if (finalMoveSpeed == 0 || !canMove || isSwoop || Time.timeScale == 0)
        {
            WalkAudio.Stop();
        }
        else
        {
            if (!WalkAudio.isPlaying)
            {
                WalkAudio.Play();
            }
            WalkAudio.pitch = 1 * finalMoveSpeed / moveSpeed;
        }
    }
}
