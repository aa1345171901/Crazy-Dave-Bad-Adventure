using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public void Play(string name)
    {
        AudioManager.Instance.PlayEffectSoundByName(name);
    }
}
