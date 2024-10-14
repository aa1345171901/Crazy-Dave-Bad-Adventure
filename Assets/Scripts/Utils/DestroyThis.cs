using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    public float delayTime = 1f;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(this.gameObject);
    }
}
