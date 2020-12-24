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

    public Dictionary<string, object> Load(bool destroyUnloaded = false)
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

        if (!uuid)
            return dataDictionary;

        //load dictionary
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        ////if dictionary is empty then we assume no saved data for the object, so destroy it
        //if (destroyUnloaded && (dataDictionary == null || dataDictionary.Count == 0))
        //    gameObject.SetActive(false);

        //load transform
        DataPersitanceHelpers.LoadTransform(ref dataDictionary, transform, "persistantTransform");

        return dataDictionary;
    }

    public Dictionary<string, object> Save()
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
        if (!uuid)
            return dataDictionary;

        //Load currently saved values
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        //save transform
        DataPersitanceHelpers.SaveTransform(ref dataDictionary, transform , "persistantTransform");

        //save json to file
        DataPersitanceHelpers.SaveDictionary(ref dataDictionary, uuid.ID);

        return dataDictionary;
    }
}
