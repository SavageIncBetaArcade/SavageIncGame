using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public interface IDataPersistance
{
    //Dictionary<string, Dictionary<string, object>> context
    void Save(DataContext context);
    void Load(DataContext context, bool destroyUnloaded = false);
}

public class DataContext : IDisposable
{
    private Dictionary<string, Dictionary<string, object>> context;
    private readonly string path;

    private bool saved = true;

    public DataContext(string path)
    {
        context = new Dictionary<string, Dictionary<string, object>>();
        this.path = path;

        DataPersitanceHelpers.LoadDictionary(ref context, path);
    }


    public Dictionary<string, object> GetDictionaryWithUUID(string UUID)
    {
        if (!context.ContainsKey(UUID))
            context[UUID] = new Dictionary<string, object>();

        return context[UUID];
    }

    public bool ContainsKey(string UUID, string Key)
    {
        return context.ContainsKey(UUID) && context[UUID].ContainsKey(Key);
    }

    public T GetValue<T>(string UUID, string key)
    {
        try
        {
            if (!ContainsKey(UUID,key))
                return default(T);

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Boolean:
                    return (T)Convert.ChangeType(bool.Parse(context[UUID][key].ToString()), typeof(T));
                case TypeCode.Int16:
                    return (T)Convert.ChangeType(short.Parse(context[UUID][key].ToString()), typeof(T));
                case TypeCode.Int32:
                    return (T)Convert.ChangeType(int.Parse(context[UUID][key].ToString()), typeof(T));
                case TypeCode.Int64:
                    return (T)Convert.ChangeType(long.Parse(context[UUID][key].ToString()), typeof(T));
                case TypeCode.UInt16:
                    return (T)Convert.ChangeType(ushort.Parse(context[UUID][key].ToString()), typeof(T));
                case TypeCode.UInt32:
                    return (T)Convert.ChangeType(uint.Parse(context[UUID][key].ToString()), typeof(T));
                case TypeCode.UInt64:
                    return (T)Convert.ChangeType(ulong.Parse(context[UUID][key].ToString()), typeof(T));
                case TypeCode.Single:
                    return (T)Convert.ChangeType(float.Parse(context[UUID][key].ToString()), typeof(T));
                case TypeCode.Double:
                    return (T)Convert.ChangeType(double.Parse(context[UUID][key].ToString()), typeof(T));
                case TypeCode.String:
                    return (T)Convert.ChangeType(context[UUID][key].ToString(), typeof(T));
            }

            var jObject = context[UUID][key] as JObject;

            if (jObject != null)
                return jObject.ToObject<T>();

            return default(T);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed get value with key:{key} from dictionary. Exception:{ex}");
            return default(T);
        }
    }

    public void SaveTransform(Transform transform, string UUID, string key = "transform")
    {
        TransformInfo transformInfo = new TransformInfo
        {
            pos = transform.position,
            rot = transform.rotation,
            scale = transform.localScale,
            enabled = transform.gameObject.activeInHierarchy
        };

        SaveData(UUID,key,transformInfo);
    }

    public void LoadTransform(Transform transform, string UUID, string key = "transform")
    {
        TransformInfo transformInfo = GetValue<TransformInfo>(UUID, key);

        if (transformInfo != null)
        {
            transform.position = transformInfo.pos;
            transform.rotation = transformInfo.rot;
            transform.localScale = transformInfo.scale;
            transform.gameObject.SetActive(transformInfo.enabled);
        }
    }

    public void SaveData(string UUID, string key, object obj)
    {
        if (!context.ContainsKey(UUID))
            context[UUID] = new Dictionary<string, object>();

        if (context[UUID].ContainsKey(key))
            context[UUID][key] = obj;
        else
            context[UUID].Add(key,obj);

        saved = false;
    }

    public void SaveChanges()
    {
        DataPersitanceHelpers.SaveDictionary(ref context, "save");
        saved = true;
        
    }

    public void Dispose()
    {
        if(!saved)
            SaveChanges();
    }
}
