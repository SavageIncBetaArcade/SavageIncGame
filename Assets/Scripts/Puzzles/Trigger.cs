using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Trigger : MonoBehaviour, IInteractable
{
    public delegate void TriggerDelegate(bool triggered);
    public event TriggerDelegate OnTrigger;

    public bool IsInteractable = false;
    public bool Toggle = false;
    public Animator TriggerAnimator;
    public string OnTriggerAnimation;
    private bool triggered = false;

    public virtual void Interact()
    {
        if (!Toggle && triggered)
            return;

        triggered = !triggered;

        if (TriggerAnimator != null)
        {
            TriggerAnimator.SetBool(OnTriggerAnimation, triggered);
        }

        OnTrigger?.Invoke(triggered);
    }

    public bool Interactable()
    {
        return IsInteractable;
    }

    public bool InteractionComplete()
    {
        return triggered;
    }

    public void Reset()
    {
        triggered = false;

        if (TriggerAnimator != null)
        {
            TriggerAnimator.SetBool(OnTriggerAnimation, triggered);
        }

        OnTrigger?.Invoke(triggered);
    }
}