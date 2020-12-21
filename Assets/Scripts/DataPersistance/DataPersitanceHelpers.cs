using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

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
    public static void SaveValueToDictionary(ref Dictionary<string,object> dictionary, string key, object data)
    {
        if (dictionary == null)
            return;

        if (!dictionary.ContainsKey(key))
            dictionary.Add(key, data);
        else
            dictionary[key] = data;
    }

    public static T GetValueFromDictionary<T>(ref Dictionary<string, object> dictionary, string key)
    {
        try
        {
            if (dictionary == null || !dictionary.ContainsKey(key))
                return default(T);

            var jObject = dictionary[key] as JObject;

            return jObject.ToObject<T>();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed get value with key:{key} from dictionary. Exception:{ex}");
            throw;
        }
    }

    public static void SaveDictionary(ref Dictionary<string, object> dictionary, string fileName)
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
            throw;
        }
        
    }

    public static void LoadDictionary(ref Dictionary<string, object> dictionary, string fileName)
    {
        try
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SavageInc", "Save");
            string json = File.ReadAllText($"{path}\\{fileName}.json");

            dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            RecurseDeserialize(dictionary);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load dictionary from json file {e}");
            throw;
        }
    }

    public static void SaveTransform(ref Dictionary<string, object> dictionary, Transform transform, string key = "transform")
    {
        TransformInfo transformInfo = new TransformInfo
        {
            pos = transform.position,
            rot = transform.rotation,
            scale = transform.localScale,
            enabled = transform.gameObject.activeSelf
        };

        SaveValueToDictionary(ref dictionary, key, transformInfo);
    }

    public static void LoadTransform(ref Dictionary<string, object> dictionary, ref Transform transform, string key = "transform")
    {
        TransformInfo transformInfo = GetValueFromDictionary<TransformInfo>(ref dictionary, key);

        if (transformInfo != null)
        {
            transform.position = transformInfo.pos;
            transform.rotation = transformInfo.rot;
            transform.localScale = transformInfo.scale;
            transform.gameObject.SetActive(transformInfo.enabled);
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
}
