using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AIStates/Patrol")]
public class PatrolState : State
{
    public override void OnUpdate(ref StackFSM stackStates)
    {
        Patrol(ref stackStates);
    }

    void Patrol(ref StackFSM stackStates)
    {
        NavMeshAgent navAgent = stackStates.aiBase.GetNavMeshAgent();
        navAgent.isStopped = false; // makes sure the enemy is moving
        ref AIBase aiBase = ref stackStates.aiBase;

        if (stackStates.aiBase.patrolPoints.Length > 1) //checks if enemy is patroling.
        {
            navAgent.SetDestination(aiBase.patrolPoints[aiBase.currentPatrolPoint]); //sets next destination point

            //moves the enemy onto the next patrolpoint
            if (aiBase.transform.position == aiBase.patrolPoints[aiBase.currentPatrolPoint] || 
                Vector3.Distance(aiBase.transform.position, aiBase.patrolPoints[aiBase.currentPatrolPoint]) < 5.0f)
            {
                if (aiBase.patrolPoints[aiBase.currentPatrolPoint] == aiBase.patrolPoints[aiBase.nextPatrolPoint])
                {
                    State idle = aiBase.potentialStates.Where(x => x.stateName == StateNames.IdleState).FirstOrDefault();
                    if(idle)
                        stackStates.PushState(idle);
                }
                aiBase.currentPatrolPoint = aiBase.nextPatrolPoint;
                aiBase.nextPatrolPoint++;               
            }

            //checks if out of the patrol array of points
            if (aiBase.nextPatrolPoint >= aiBase.patrolPoints.Length)
            {
                aiBase.nextPatrolPoint = 0;
            }
        }
    }
}
