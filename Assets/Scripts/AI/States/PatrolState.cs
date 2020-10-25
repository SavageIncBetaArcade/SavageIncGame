using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AIStates/Patrol")]
public class PatrolState : MovingState
{
    public override void OnUpdate(ref StackFSM stackStates)
    {
        base.OnUpdate(ref stackStates);
        Patrol(ref stackStates);
    }

    void Patrol(ref StackFSM stackStates)
    {
        ref AIBase aiBase = ref stackStates.aiBase;
        NavMeshAgent navAgent = aiBase.NavAgent;
        navAgent.isStopped = false; // makes sure the enemy is moving

        if (aiBase.PatrolPoints.Length > 1) //checks if enemy is patroling.
        {
            navAgent.SetDestination(aiBase.PatrolPoints[aiBase.CurrentPatrolPoint].position); //sets next destination point

            //moves the enemy onto the next patrolpoint
            if (aiBase.transform.position == aiBase.PatrolPoints[aiBase.CurrentPatrolPoint].position || 
                Vector3.Distance(aiBase.transform.position, aiBase.PatrolPoints[aiBase.CurrentPatrolPoint].position) < 5.0f)
            {
                if (aiBase.PatrolPoints[aiBase.CurrentPatrolPoint].position == aiBase.PatrolPoints[aiBase.NextPatrolPoint].position)
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
        else if(aiBase.PatrolPoints.Length == 0)
        {
            if (aiBase.transform.position == navAgent.destination ||
                Vector3.Distance(aiBase.transform.position, navAgent.destination) < 5.0f)
            {
                Vector3 randomDirection = Random.insideUnitSphere * aiBase.WalkDistance;
                randomDirection += aiBase.transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, aiBase.WalkDistance, 1);
                Vector3 finalPosition = hit.position;
                navAgent.destination = finalPosition;
            }
        }
    }
}
