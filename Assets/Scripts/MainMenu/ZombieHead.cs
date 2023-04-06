using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ZombieHead : MonoBehaviour
{
    private void OnMouseEnter()
    {
        int index = Random.Range(1, 7);
        string animName = "Zombie" + index;
        AudioManager.Instance.PlayEffectSoundByName(animName);
    }
}
