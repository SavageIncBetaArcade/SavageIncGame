using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TransformInfo
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
    public bool enabled;
}


public class DataPersitanceHelpers 
{
    public static void ClearSaves()
    {
        try
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SavageInc", "Save");
            Directory.Delete(path, true);

        }
        catch(Exception ex)
        {

        }
    }

    public static async Task WriteAsync(string path, string data)
    {
        using (var sw = new StreamWriter(path))
        {
            await sw.WriteAsync(data);
        }
    }

    public static void SaveDictionary(ref Dictionary<string, Dictionary<string, object>> dictionary, string fileName)
    {
        try
        {
            string json = JsonConvert.SerializeObject(dictionary, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SavageInc", "Save");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText($"{path}\\{fileName}.json", json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save dictionary to json file {e}");
        }
        
    }

    public static void LoadDictionary(ref Dictionary<string, Dictionary<string, object>> dictionary, string fileName)
    {
        try
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SavageInc", "Save");
            
            if (Directory.Exists(path) && File.Exists($"{path}\\{fileName}.json"))
            {
                string json = File.ReadAllText($"{path}\\{fileName}.json");
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);
                //RecurseDeserialize(dictionary);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to load dictionary from json file {e}");
        }
    }

    private static void RecurseDeserialize(Dictionary<string, object> result)
    {
        //Iterate throgh key/value pairs
        foreach (var keyValuePair in result.ToArray())
        {
            //Check to see if Newtonsoft thinks this is a JArray
            var jarray = keyValuePair.Value as JArray;

            if (jarray != null)
            {
                //We have a JArray

                //Convert JArray back to json and deserialize to a list of dictionaries
                var dictionaries = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jarray.ToString());

                //Set the result as the dictionary
                result[keyValuePair.Key] = dictionaries;

                //Iterate throught the dictionaries
                foreach (var dictionary in dictionaries)
                {
                    //Recurse
                    RecurseDeserialize(dictionary);
                }
            }
        }
    }

    public static List<T> FindAllGameObjects<T>()
    {
        // get root objects in scene
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        List<T> gameObjects = new List<T>();
        foreach (GameObject go in rootObjects)
        {
            var component = go.GetComponentsInChildren<T>(true);
            if (component != null)
                gameObjects.AddRange(component);
        }

        return gameObjects.ToList();
    }

    public static void SaveAll()
    {
        ClearSaves();

        using (DataContext context = new DataContext("save"))
        {
            var SavableObjects = FindAllGameObjects<MonoBehaviour>().OfType<IDataPersistance>().ToList();
            foreach (var obj in SavableObjects)
            {
                obj.Save(context);
            }
        }
    }

    public static void LoadAll()
    {
        var SavableObjects = FindAllGameObjects<MonoBehaviour>().OfType<IDataPersistance>();
        var interactionTriggers = SavableObjects.OfType<InteractionTrigger>();

        using (DataContext context = new DataContext("save"))
        {
            //load all triggers first
            foreach (var obj in interactionTriggers)
            {
                obj.Load(context, true);
            }

            //load everything else
            foreach (var obj in SavableObjects.Where(x => !interactionTriggers.Contains(x)))
            {
                obj.Load(context, true);
            }
        }
    }
}
