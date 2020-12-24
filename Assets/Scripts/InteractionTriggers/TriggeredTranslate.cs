using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UUID))]
public class TriggeredTranslate : MonoBehaviour, IDataPersistance
{
    public InteractionTrigger[] Triggers;
    public Vector3 TriggeredPositionOffset;
    public float Speed = 5.0f;
    private Vector3 origin;
    private Vector3 targetPosition;

    private UUID uuid;


    public void Awake()
    {
        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger += translate;
        }

        origin = transform.position;
        targetPosition = origin;

        uuid = GetComponent<UUID>();
    }

    public void OnDestroy()
    {
        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger -= translate;
        }
    }

    private void translate(bool triggered, InteractionTrigger trigger)
    {
        //check if all triggers are met
        if (InteractionTrigger.AnyFalse(Triggers))
        {
            targetPosition = origin;
            return;
            
        }

        targetPosition = origin - TriggeredPositionOffset;
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
    }

    public Dictionary<string, object> Save()
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
        if (!uuid)
            return dataDictionary;

        //Load currently saved values
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "TriggeredPositionOffset", TriggeredPositionOffset);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "targetPosition", targetPosition);

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

        TriggeredPositionOffset = DataPersitanceHelpers.GetValueFromDictionary<Vector3>(ref dataDictionary, "TriggeredPositionOffset");
        targetPosition = DataPersitanceHelpers.GetValueFromDictionary<Vector3>(ref dataDictionary, "targetPosition");

        return dataDictionary;
    }
}
