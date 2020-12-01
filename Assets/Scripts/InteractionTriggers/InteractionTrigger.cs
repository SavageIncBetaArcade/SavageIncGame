using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractionTrigger : MonoBehaviour, IInteractable
{
    public delegate void TriggerDelegate(bool triggered);
    public event TriggerDelegate OnTrigger;

    public bool IsInteractable = false;
    public bool Toggle = false;
    public Animator TriggerAnimator;
    public string OnTriggerAnimation;
    public string PopupText;
    public InventorySectionHandler InventorySection;
    public Item[] RequiredItems;

    private TextMeshProUGUI textMesh;
    private bool triggered = false;
    public bool Triggered => triggered;

    private AudioSource triggerSound;

    private static Dictionary<InteractionTrigger, bool> popupDisplayed = new Dictionary<InteractionTrigger, bool>();

    protected virtual void Awake()
    {
        textMesh = GameObject.FindGameObjectWithTag("InteractionText")?.GetComponent<TextMeshProUGUI>();
        triggerSound = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        //if any all popups are not active set the text to empty (Hides the popup text)
        //Reason for using a static dictionary is to prevent other triggers hiding the popup when it need to be shown
        if(popupDisplayed.All(x => !x.Value) && textMesh)
            textMesh.text = "";
    }

    public virtual void Interact()
    {
        if (!IsInteractable || !HasRequiredItems() || (!Toggle && triggered))
            return;

        triggered = !triggered;

        if (TriggerAnimator != null)
        {
            TriggerAnimator.SetBool(OnTriggerAnimation, triggered);

        }

        OnTrigger?.Invoke(triggered);

        //play sound
        triggerSound?.Play();
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

        if (active && !string.IsNullOrWhiteSpace(PopupText) && textMesh && HasRequiredItems())
        {
            textMesh.text = PopupText;
        }
    }

    private bool HasRequiredItems()
    {
        if (!InventorySection || RequiredItems.Length == 0)
            return true;

        return RequiredItems.All(requiredItem => InventorySection.itemInventory.FindItemIndex(requiredItem) >= 0);
    }

    public static bool AllTrue(IList<InteractionTrigger> triggers)
    {
        return triggers.All(trigger => trigger.Triggered);
    }

    public static bool AllFalse(IList<InteractionTrigger> triggers)
    {
        return triggers.All(trigger => !trigger.Triggered);
    }

    public static bool AnyTrue(IList<InteractionTrigger> triggers)
    {
        return triggers.Any(trigger => trigger.Triggered);
    }

    public static bool AnyFalse(IList<InteractionTrigger> triggers)
    {
        return triggers.Any(trigger => !trigger.Triggered);
    }
}