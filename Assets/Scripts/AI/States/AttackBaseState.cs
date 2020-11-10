using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStates/AttackBase")]
public class AttackBaseState : State
{
    public override void OnUpdate(ref StackFSM stackStates)
    {
        ref AIBase aiBase = ref stackStates.aiBase;
        Transform player = aiBase.Player.transform;

        float distanceToPlayer = Vector3.Distance(aiBase.transform.position, player.position);
        if (distanceToPlayer <= aiBase.AttackDistance)
        {
            aiBase.RightAbilitiy.Attack();
            aiBase.LeftAbility.Attack();
        }
        else
        {
            stackStates.PopState();
        }
    }
}
