using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public InteractionTrigger MoveTrigger;
    public InteractionTrigger PainTrigger;
    public Transform Origin;
    public AudioSource TriggerAudioSource;

    private void Awake()
    {
        if (MoveTrigger)
        {
            MoveTrigger.OnTrigger += moveUp;
        }
        if (PainTrigger)
        {
            MoveTrigger.OnTrigger += hurtPlayer;
        }
    }

    private void Destroy()
    {
        if (MoveTrigger)
        {
            MoveTrigger.OnTrigger -= moveUp;
        }
        if (PainTrigger)
        {
            MoveTrigger.OnTrigger -= hurtPlayer;
        }
    }

    private void moveUp(bool triggered, InteractionTrigger trigger)
    {
        if (!triggered || !Origin)
            return;

        transform.position = Origin.position;
        if (TriggerAudioSource && TriggerAudioSource.clip)
            TriggerAudioSource.PlayOneShot(TriggerAudioSource.clip);
    }

    private void hurtPlayer(bool triggered, InteractionTrigger trigger)
    {
        if (!triggered || !Origin)
            return;

    }
}
