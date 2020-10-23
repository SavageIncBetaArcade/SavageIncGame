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
        NavMeshAgent navAgent = stackStates.aiBase.NavAgent;
        navAgent.isStopped = false; // makes sure the enemy is moving
        ref AIBase aiBase = ref stackStates.aiBase;

        if (stackStates.aiBase.PatrolPoints.Length > 1) //checks if enemy is patroling.
        {
            navAgent.SetDestination(aiBase.PatrolPoints[aiBase.CurrentPatrolPoint]); //sets next destination point

            //moves the enemy onto the next patrolpoint
            if (aiBase.transform.position == aiBase.PatrolPoints[aiBase.CurrentPatrolPoint] || 
                Vector3.Distance(aiBase.transform.position, aiBase.PatrolPoints[aiBase.CurrentPatrolPoint]) < 5.0f)
            {
                if (aiBase.PatrolPoints[aiBase.CurrentPatrolPoint] == aiBase.PatrolPoints[aiBase.NextPatrolPoint])
                {
                    State idle = aiBase.PotentialStates.FirstOrDefault(x => x.StateName == StateNames.IdleState);
                    if(idle)
                        stackStates.PushState(idle);
                }
                aiBase.CurrentPatrolPoint = aiBase.NextPatrolPoint;
                aiBase.NextPatrolPoint++;               
            }

            //checks if out of the patrol array of points
            if (aiBase.NextPatrolPoint >= aiBase.PatrolPoints.Length)
            {
                aiBase.NextPatrolPoint = 0;
            }
        }
    }
}
