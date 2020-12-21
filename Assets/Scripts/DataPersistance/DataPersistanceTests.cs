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
        var SavableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        foreach (var obj in SavableObjects)
        {
            obj.Save();
        }
    }

    void LoadAll()
    {
        var SavableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        foreach (var obj in SavableObjects)
        {
            obj.Load();
        }
    }
}
