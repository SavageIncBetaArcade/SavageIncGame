using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UUID))]
public class TriggeredTranslate : MonoBehaviour, IDataPersistance
{
    public InteractionTrigger[] Triggers;
    public TriggerType TriggerType;
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
        if (InteractionTrigger.CheckTriggers(TriggerType, Triggers))
        {
            targetPosition = origin - TriggeredPositionOffset;
        }
        else
        {
            targetPosition = origin;
        }
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
    }

    public void Save(DataContext context)
    {
        if (!uuid)
            return;


        context.SaveData(uuid.ID, "TriggeredPositionOffset", TriggeredPositionOffset);
        context.SaveData(uuid.ID, "targetPosition", targetPosition);
    }

    public void Load(DataContext context, bool destroyUnloaded = false)
    {
        if (!uuid)
            return;


        TriggeredPositionOffset = context.GetValue<Vector3>(uuid.ID, "TriggeredPositionOffset");
        targetPosition = context.GetValue<Vector3>(uuid.ID, "targetPosition");
    }
}
