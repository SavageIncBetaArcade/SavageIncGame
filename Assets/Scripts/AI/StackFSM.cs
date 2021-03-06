﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackFSM : MonoBehaviour
{
    public AIBase aiBase;
    private List<State> stateStack = new List<State>();

    private void Start()
    {
        aiBase = GetComponent<AIBase>();
    }

    // Update is called once per frame
    void Update()
    {
        State currentState = GetCurrentState();

        if(currentState != null)
        {
            if (!aiBase.BossStopped)
            {
                StackFSM stackfsm = gameObject.GetComponent<StackFSM>();
                currentState.OnUpdate(ref stackfsm);
            }
        }
    }

    public void PopState()
    {
        State currentState = GetCurrentState();
        if (currentState != null)
        {
            currentState.OnPop();
            stateStack.Remove(currentState);
        }
    }

    public void PushState(State newState)
    {
        State currentState = GetCurrentState();
        if (currentState != newState)
        {
            newState.OnPush();
            stateStack.Add(newState);
        }
    }

    public void ChangeState(State newState)
    {
        if (GetCurrentState() != newState)
        {
            PopState();
            PushState(newState);
        }
    }

    public State GetCurrentState()
    {
        return stateStack.Count > 0 ? stateStack[stateStack.Count - 1] : null;
    }

    public void Clear()
    {
        stateStack.Clear();
    }
}
