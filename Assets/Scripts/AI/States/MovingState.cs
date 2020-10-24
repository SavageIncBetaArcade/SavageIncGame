using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MovingState : State
{
    public override void OnUpdate(ref StackFSM stackStates)
    {
        if(stackStates.aiBase.PlayerInFieldOfVision())
        {
            State follow = stackStates.aiBase.PotentialStates.FirstOrDefault(x => x.StateName == StateNames.FollowState);
            if (follow)
                stackStates.PushState(follow);
        }
    }
}
