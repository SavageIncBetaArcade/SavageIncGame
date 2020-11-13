using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Trigger : MonoBehaviour, IInteractable
{
    public delegate void TriggerDelegate();
    public event TriggerDelegate OnTrigger;

    public bool IsInteractable = false;
    public bool SingleTrigger = false;
    private bool triggered = false;

    public virtual void Interact()
    {
        if (SingleTrigger && triggered)
            return;

        OnTrigger?.Invoke();
        triggered = true;
    }

    public bool Interactable()
    {
        return IsInteractable;
    }

    public bool InteractionComplete()
    {
        return triggered;
    }
}