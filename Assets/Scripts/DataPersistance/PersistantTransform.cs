using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UUID))]
public class PersistantTransform : MonoBehaviour, IDataPersistance
{
    private UUID uuid;

    void Awake()
    {
        uuid = GetComponent<UUID>();
        if (!uuid)
        {
            Debug.LogError("PersistantTransform doesn't have an UUID (Can't load data from json)");
        }
    }

    public void Load(DataContext context, bool destroyUnloaded = false)
    {
        
        if(!uuid || !context.ContainsKey(uuid.ID, "persistantTransform"))
            return;

        context.LoadTransform(transform, uuid.ID, "persistantTransform");
    }

    public void Save(DataContext context)
    {
        if (!uuid)
            return;

        context.SaveTransform(transform, uuid.ID, "persistantTransform");
    }
}
