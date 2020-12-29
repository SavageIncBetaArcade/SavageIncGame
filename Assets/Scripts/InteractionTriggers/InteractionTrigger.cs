using System;
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

    private TextMeshProUGUI textMesh;
    [SerializeField]
    private bool triggered = false;
    public bool Triggered => triggered;

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
        if (!IsInteractable || !HasRequiredItems() || (!Toggle && triggered))
            return;

        triggered = !triggered;

        if (TriggerAnimator != null)
        {
            TriggerAnimator.SetBool(OnTriggerAnimation, triggered);

        }

        OnTrigger?.Invoke(triggered, this);

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
    public Dictionary<string, object> Save()
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
        if (!uuid)
            return dataDictionary;

        //Load currently saved values
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "IsInteractable", IsInteractable);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "Toggle", Toggle);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "triggered", triggered);

        //save json to file
        DataPersitanceHelpers.SaveDictionary(ref dataDictionary, uuid.ID);

        return dataDictionary;
    }

    public Dictionary<string, object> Load(bool destroyUnloaded = false)
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

        if (!uuid)
            return dataDictionary;

        //load dictionary
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        IsInteractable = DataPersitanceHelpers.GetValueFromDictionary<bool>(ref dataDictionary, "IsInteractable");
        Toggle = DataPersitanceHelpers.GetValueFromDictionary<bool>(ref dataDictionary, "Toggle");
        triggered = DataPersitanceHelpers.GetValueFromDictionary<bool>(ref dataDictionary, "triggered");

        if (TriggerAnimator != null)
        {
            TriggerAnimator.SetBool(OnTriggerAnimation, triggered);

        }

        //OnTrigger?.Invoke(triggered, this);

        return dataDictionary;
    }
    #endregion
}