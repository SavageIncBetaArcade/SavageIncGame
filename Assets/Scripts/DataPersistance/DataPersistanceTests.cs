using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetKeyUp(KeyCode.F5))
        {
            Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

            DataPersitanceHelpers.SaveTransform(ref dataDictionary, transform);
            DataPersitanceHelpers.SaveDictionary(ref dataDictionary, "TestSave");

            characterBase.Save();
        }

        if (Input.GetKeyUp(KeyCode.F6))
        {
            Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

            DataPersitanceHelpers.LoadDictionary(ref dataDictionary, "TestSave");
            DataPersitanceHelpers.LoadTransform(ref dataDictionary, transform);

            characterBase.Load();
        }
    }
}
