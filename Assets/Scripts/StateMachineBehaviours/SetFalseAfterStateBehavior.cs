using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFalseAfterStateBehavior : StateMachineBehaviour
{
    public string TransitionBoolName;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(TransitionBoolName, false);
    }

}
