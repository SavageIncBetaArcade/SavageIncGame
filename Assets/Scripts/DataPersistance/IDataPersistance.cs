using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    Dictionary<string, object> Save();
    Dictionary<string, object> Load();
}
