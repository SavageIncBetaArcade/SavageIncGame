using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public InteractionTrigger Trigger;

    void Awake()
    {
        if (Trigger)
            Trigger.OnTrigger += trigger;
    }

    void OnDestroy()
    {
        if (Trigger)
            Trigger.OnTrigger -= trigger;
    }

    private void trigger(bool triggered, InteractionTrigger trigger)
    {
        if (triggered)
        {
            DataPersitanceHelpers.SaveAll();
        }
    }
}
