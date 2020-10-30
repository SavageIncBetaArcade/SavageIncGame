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
        float distanceToPlayer = Vector3.Distance(aiBase.transform.position, player.position);
        if (distanceToPlayer < aiBase.SenseRange)
        {
            if (distanceToPlayer > aiBase.AttackDistance)
            {
                aiBase.currentDestination = player.position;
                navAgent.SetDestination(player.position);// set the enemys destination to the players position
            }
            else
            {
                aiBase.currentDestination = navAgent.transform.position;
                navAgent.SetDestination(navAgent.transform.position);
            }

            //TODO:: when get close enough attack the player.
            //if (distanceToPlayer <= 1.5f)
            //{
            //    aiBase.RightAbilitiy.Attack();
            //    aiBase.LeftAbility.Attack();
            //}
        }
        else
        {
            stackStates.PopState();
        }
    }
}
