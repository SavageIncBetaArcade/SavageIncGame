using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateNames
{
    IdleState,
    PatrolState,
    FollowState,
    NumStates
}

public abstract class State : ScriptableObject
{
    public virtual void OnPush()
    {

    }

    public virtual void OnPop()
    {

    }

    public abstract void OnUpdate(ref StackFSM stackStates);

    public StateNames stateName;

}
