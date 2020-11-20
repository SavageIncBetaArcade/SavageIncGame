using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public abstract class InteractionTrigger : MonoBehaviour, IInteractable
{
    public delegate void TriggerDelegate(bool triggered);
    public event TriggerDelegate OnTrigger;

    public bool IsInteractable = false;
    public bool Toggle = false;
    public Animator TriggerAnimator;
    public string OnTriggerAnimation;
    public string PopupText;
    private TextMeshProUGUI textMesh;
    private bool triggered = false;
    public bool Triggered => triggered;

    private static Dictionary<InteractionTrigger, bool> popupDisplayed = new Dictionary<InteractionTrigger, bool>();

    protected virtual void Awake()
    {
        textMesh = GameObject.FindGameObjectWithTag("InteractionText")?.GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        //if any all popups are not active set the text to empty (Hides the popup text)
        //Reason for using a static dictionary is to prevent other triggers hiding the popup when it need to be shown
        if(popupDisplayed.All(x => !x.Value))
            textMesh.text = "";
    }

    public virtual void Interact()
    {
        if (!IsInteractable || (!Toggle && triggered))
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

    public void ShowPopupText(bool active)
    {
        if (!IsInteractable)
            return;

        popupDisplayed[this] = active;

        if (active && !string.IsNullOrWhiteSpace(PopupText) && textMesh)
        {
            textMesh.text = PopupText;
        }
    }
}