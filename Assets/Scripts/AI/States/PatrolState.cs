using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStates/Patrol")]
public class PatrolState : State
{
    public override void OnUpdate(ref StackFSM stackStates)
    {
        Patrol(ref stackStates);
    }

    void Patrol(ref StackFSM stackStates)
    {
        stackStates.aiBase.navAgent.isStopped = false; // makes sure the enemy is moving

        if (stackStates.aiBase.patrolPoints.Length > 1) //checks if enemy is patroling.
        {
            stackStates.aiBase.navAgent.SetDestination(stackStates.aiBase.patrolPoints[stackStates.aiBase.currentPatrolPoint]); //sets next destination point

            //moves the enemy onto the next patrolpoint
            if (stackStates.aiBase.transform.position == stackStates.aiBase.patrolPoints[stackStates.aiBase.currentPatrolPoint] || 
                Vector3.Distance(stackStates.aiBase.transform.position, stackStates.aiBase.patrolPoints[stackStates.aiBase.currentPatrolPoint]) < 5.0f)
            {
                if (stackStates.aiBase.patrolPoints[stackStates.aiBase.currentPatrolPoint] == stackStates.aiBase.patrolPoints[stackStates.aiBase.nextPatrolPoint])
                {
                    stackStates.PushState(stackStates.aiBase.potentialStates[1]);
                }
                stackStates.aiBase.currentPatrolPoint = stackStates.aiBase.nextPatrolPoint;
                stackStates.aiBase.nextPatrolPoint++;
                //finds a random point for the roamers
                //if (randomPatrol)
                //    patrolPoint = Random.Range(0, patrolPoints.Length);
            }

            //checks if out of the patrol array of points
            if (stackStates.aiBase.nextPatrolPoint >= stackStates.aiBase.patrolPoints.Length)
            {
                stackStates.aiBase.nextPatrolPoint = 0;
            }
        }
    }
}
