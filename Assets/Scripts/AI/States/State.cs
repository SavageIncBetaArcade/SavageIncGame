using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public virtual void OnPush()
    {

    }

    public virtual void OnPop()
    {

    }

    public abstract void OnUpdate(ref StackFSM stackStates);
}
