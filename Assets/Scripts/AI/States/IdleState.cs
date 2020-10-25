using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStates/Idle")]
public class IdleState : MovingState
{
    private float stateExpireTime;

    public override void OnUpdate(ref StackFSM stackStates)
    {
        //MG need to make any idle animation play here.

        stateExpireTime -= Time.deltaTime;
        if(stateExpireTime <= 0.0f)
        {
            stackStates.PopState();
        }
    }

    public override void OnPush()
    {
        stateExpireTime = 5.0f;
    }
}
