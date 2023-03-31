using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class TransferGate : BaseProp
{
    public GameObject TransferGatePrefab;

    public AudioSource audioSource;

    private float CoolingTime = 8;
    private float timer;

    private bool isInit;
    private bool isCooling;

    private GameObject transferGate1;
    private GameObject transferGate2;
    private Trigger2D transferGateTrigger1;
    private Trigger2D transferGateTrigger2;

    public void SetTransferGate()
    {
        if (!isInit)
        {
            isInit = true;
            transferGate1 = Instantiate(TransferGatePrefab, this.transform);
            transferGateTrigger1 = transferGate1.GetComponent<Trigger2D>();
            transferGate2 = Instantiate(TransferGatePrefab, this.transform);
            transferGateTrigger2 = transferGate2.GetComponent<Trigger2D>();
            AudioManager.Instance.AudioLists.Add(this.audioSource);
            this.audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        }
    }

    public override void Reuse()
    {
        base.Reuse();
        var levelBounds = LevelManager.Instance.LevelBounds;
        float randomX = Random.Range(levelBounds.min.x, levelBounds.max.x / 2);
        float randomY = Random.Range(levelBounds.min.y, levelBounds.max.y / 2);
        transferGate1.transform.position = new Vector3(randomX, randomY, 0);

        randomX = Random.Range(levelBounds.max.x / 2, levelBounds.max.x);
        randomY = Random.Range(levelBounds.max.y / 2, levelBounds.max.y);
        transferGate2.transform.position = new Vector3(randomX, randomY, 0);
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();
        if (Time.time - timer > CoolingTime)
        {
            if (isCooling)
            {
                TransferGateEnable(true);
                isCooling = false;
            }

            if (transferGateTrigger1.IsTrigger)
            {
                Transmit(transferGateTrigger1.Target, transferGate2);
            }
            if (transferGateTrigger2.IsTrigger)
            {
                Transmit(transferGateTrigger2.Target, transferGate1);
            }
        }
    }

    private void Transmit(GameObject character, GameObject target)
    {
        audioSource.pitch = Random.Range(0.9f,1.1f);
        audioSource.Play();
        character.transform.position = target.transform.position;
        TransferGateEnable(false);
        timer = Time.time;
        isCooling = true;
    }

    private void TransferGateEnable(bool enable)
    {
        if (enable)
        {
            transferGate1.GetComponent<SpriteRenderer>().color = Color.white;
            transferGate2.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            transferGate1.GetComponent<SpriteRenderer>().color = Color.gray;
            transferGate2.GetComponent<SpriteRenderer>().color = Color.gray;
        }
        transferGate1.transform.GetChild(0).gameObject.SetActive(enable);
        transferGate2.transform.GetChild(0).gameObject.SetActive(enable);
    }
}
