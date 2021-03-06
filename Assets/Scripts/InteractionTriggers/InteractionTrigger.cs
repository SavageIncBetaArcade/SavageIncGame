﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum TriggerType
{
    ALL_TRUE,
    ALL_FALSE,
    ANY_TRUE,
    ANY_FALSE
}

[RequireComponent(typeof(UUID))]
[RequireComponent(typeof(AudioSource))]
public class InteractionTrigger : MonoBehaviour, IInteractable, IDataPersistance
{
    #region events
    public delegate void TriggerDelegate(bool triggered, InteractionTrigger trigger);
    public event TriggerDelegate OnTrigger;
    #endregion

    #region members
    public bool IsInteractable = false;
    public bool Toggle = false;
    public Animator TriggerAnimator;
    public string OnTriggerAnimation;
    public string PopupText;
    public InventorySectionHandler InventorySection;
    public Item[] RequiredItems;
    public int InteractionCount = 1;
    public int AlertAmount = 0;

    private TextMeshProUGUI textMesh;
    [SerializeField]
    private bool triggered = false;
    public bool Triggered => triggered;
    int InteractionCounter = 0;

    private AudioSource triggerSound;
    private UUID uuid;

    private static Dictionary<InteractionTrigger, bool> popupDisplayed = new Dictionary<InteractionTrigger, bool>();
    #endregion

    #region methods
    protected virtual void Awake()
    {
        textMesh = GameObject.FindGameObjectWithTag("InteractionText")?.GetComponent<TextMeshProUGUI>();
        triggerSound = GetComponent<AudioSource>();
        uuid = GetComponent<UUID>();
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
        bool hasItems = HasRequiredItems();
        if (!IsInteractable || !hasItems || (!Toggle && triggered) || !InteractionCountTest())
        {
            if (IsInteractable && !triggered && !hasItems)
            {
                ShowPopupText(true);
                StartCoroutine(hidePopupText(2));
            }
            return;
        }

        triggered = !triggered;

        if (TriggerAnimator != null)
        {
            TriggerAnimator.SetBool(OnTriggerAnimation, triggered);

        }

        OnTrigger?.Invoke(triggered, this);
        FindObjectOfType<PortalManager>().AlertMeter += AlertAmount;

        //play sound
        if(triggerSound)
            triggerSound.Play();
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

        OnTrigger?.Invoke(triggered, this);
    }

    public void ShowPopupText(bool active)
    {
        if (!IsInteractable)
            return;

        popupDisplayed[this] = active;

        if(Input.GetJoystickNames().Length > 0)
        {
            PopupText = PopupText.Replace("F", "X");
        }

        if (active && !string.IsNullOrWhiteSpace(PopupText) && textMesh)
        {
            if (!HasRequiredItems())
                textMesh.text = "Requires a certain item!";
            else
                textMesh.text = PopupText;
        }
    }

    private bool HasRequiredItems()
    {
        if (!InventorySection || RequiredItems.Length == 0)
            return true;

        return RequiredItems.All(requiredItem => InventorySection.itemInventory.FindItemIndex(requiredItem) >= 0);
    }

    private IEnumerator hidePopupText(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ShowPopupText(false);
    }

    public bool InteractionCountTest()
    {
        if (InteractionCount == 1) return true;

        InteractionCounter++;
        if (InteractionCounter >= InteractionCount) return true;

        return false;
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

    public static bool CheckTriggers(TriggerType type, IList<InteractionTrigger> triggers)
    {
        switch (type)
        {
            case TriggerType.ALL_TRUE:
                return AllTrue(triggers);
            case TriggerType.ALL_FALSE:
                return AllFalse(triggers);
            case TriggerType.ANY_TRUE:
                return AnyTrue(triggers);
            case TriggerType.ANY_FALSE:
                return AnyFalse(triggers);
            default:
                return false;
        }
    }
    #endregion

    #region IDataPersistance
    public void Save(DataContext context)
    {
        if (!uuid)
            return;

        context.SaveData(uuid.ID, "IsInteractable", IsInteractable);
        context.SaveData(uuid.ID, "Toggle", Toggle);
        context.SaveData(uuid.ID, "triggered", triggered);
    }

    public void Load(DataContext context, bool destroyUnloaded = false)
    {
        if (!uuid)
            return;


        IsInteractable = context.GetValue<bool>(uuid.ID, "IsInteractable");
        Toggle = context.GetValue<bool>(uuid.ID, "Toggle");
        triggered = context.GetValue<bool>(uuid.ID, "triggered");

        if (TriggerAnimator != null)
        {
            TriggerAnimator.SetBool(OnTriggerAnimation, triggered);

        }

        //OnTrigger?.Invoke(triggered, this);
    }
    #endregion
}