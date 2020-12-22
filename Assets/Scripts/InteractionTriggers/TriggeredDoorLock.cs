using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UUID))]
public class TriggeredDoorLock : MonoBehaviour, IDataPersistance
{
    public InteractionTrigger[] InteractTriggers;
    public InteractionTrigger[] LockTriggers;
    public InteractionTrigger[] UnlockTriggers;

    public AudioSource LockSound;
    public AudioSource UnlockSound;

    public Animator Animator;
    public string OnTriggerAnimation;

    public bool IsLocked = false;

    private UUID uuid;

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

        uuid = GetComponent<UUID>();
    }

    void onTrigger(bool triggered, InteractionTrigger trigger)
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

    void lockTrigger(bool triggered, InteractionTrigger trigger)
    {
        if (!IsLocked)
        {
            IsLocked = true;

            if (IsLocked)
                Animator.SetBool(OnTriggerAnimation, false);

            LockSound?.Play();
        }
    }

    void unlockTrigger(bool triggered, InteractionTrigger trigger)
    {
        if (IsLocked)
        {
            IsLocked = false;
            UnlockSound?.Play();
        }
    }

    public Dictionary<string, object> Save()
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
        if (!uuid)
            return dataDictionary;

        //Load currently saved values
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "IsLocked", IsLocked);

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

        IsLocked = DataPersitanceHelpers.GetValueFromDictionary<bool>(ref dataDictionary, "IsLocked");

        if (Animator != null)
        {
            Animator.SetBool(OnTriggerAnimation, InteractionTrigger.AnyTrue(InteractTriggers));

        }

        return dataDictionary;
    }
}
