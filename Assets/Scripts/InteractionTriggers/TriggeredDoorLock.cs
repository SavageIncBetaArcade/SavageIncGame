using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggeredDoorLock : MonoBehaviour
{
    public InteractionTrigger[] InteractTriggers;
    public InteractionTrigger[] LockTriggers;
    public InteractionTrigger[] UnlockTriggers;
    public Animator Animator;
    public string OnTriggerAnimation;

    public bool IsLocked = false;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var trigger in InteractTriggers)
        {
            trigger.OnTrigger += onTrigger;
        }

        foreach (var trigger in LockTriggers)
        {
            trigger.OnTrigger += lockTrigger;
        }

        foreach (var trigger in UnlockTriggers)
        {
            trigger.OnTrigger += unlockTrigger;
        }
    }

    void onTrigger(bool triggered)
    {
        if(IsLocked)
            return;

        if (Animator && !string.IsNullOrWhiteSpace(OnTriggerAnimation))
        {
            if (InteractionTrigger.AnyFalse(InteractTriggers))
            {
                Animator.SetBool(OnTriggerAnimation,false);
            }
            else
            {
                Animator.SetBool(OnTriggerAnimation, true);
            }
        }
    }

    void lockTrigger(bool triggered)
    {
        IsLocked = true;

        if(IsLocked)
            Animator.SetBool(OnTriggerAnimation, false);
    }

    void unlockTrigger(bool triggered)
    {
        IsLocked = false;
    }
}
