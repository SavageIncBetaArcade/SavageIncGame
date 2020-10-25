using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AIStates/Follow")]
public class FollowState : State
{
    public override void OnUpdate(ref StackFSM stackStates)
    {
        ref AIBase aiBase = ref stackStates.aiBase;
        NavMeshAgent navAgent = aiBase.NavAgent;
        Transform player = aiBase.Player.transform;

        //when the enemy is closer than 20 follow the player
        if (Vector3.Distance(aiBase.transform.position, player.position) < aiBase.SenseRange)
        {
            aiBase.currentDestination = player.position;
            navAgent.SetDestination(player.position);// set the enemys destination to the players position

            //TODO:: when get close enough attack the player.
        }
        else
        {
            stackStates.PopState();
        }
    }
}
