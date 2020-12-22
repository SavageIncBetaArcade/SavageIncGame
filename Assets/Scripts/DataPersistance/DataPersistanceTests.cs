using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistanceTests : MonoBehaviour
{
    public Transform transform;
    public CharacterBase characterBase;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F6))
        {
            SaveAll();
        }

        if (Input.GetKeyUp(KeyCode.F7))
        {
            LoadAll();
        }
    }

    void SaveAll()
    {
        //TODO clear all saves before, then we can check if an object doesn't have a UUID in the save directory it no longer exists (E.G deleted)
        DataPersitanceHelpers.ClearSaves();

        var SavableObjects = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<IDataPersistance>();
        foreach (var obj in SavableObjects)
        {
            obj.Save();
        }
    }

    void LoadAll()
    {
        var SavableObjects = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<IDataPersistance>();
        var interactionTriggers = SavableObjects.OfType<InteractionTrigger>();

        //load all triggers first
        foreach (var obj in interactionTriggers)
        {
            obj.Load(true);
        }

        //load everything else
        foreach (var obj in SavableObjects.Where(x => !interactionTriggers.Contains(x)))
        {
            obj.Load(true);
        }

        //check for all UUID that don't have a saved against them

    }
}
