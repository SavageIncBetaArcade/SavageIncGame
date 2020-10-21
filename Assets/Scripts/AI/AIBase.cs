using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBase : CharacterBase
{
    public float senseRange = 60.0f;
    public float angleOfVision = 45.0f;
    public Vector3[] patrolPoints;
    public int currentPatrolPoint = 0;
    public int nextPatrolPoint = 1;
    public NavMeshAgent navAgent;
    private StackFSM stackOfStates;
    public State[] potentialStates;
    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        stackOfStates = GetComponent<StackFSM>();

        if (potentialStates.Length > 0)
        {
            stackOfStates.PushState(potentialStates[0]);
        }        
    }

    private void Update()
    {
        if(potentialStates.Length > 0 && stackOfStates.GetCurrentState() == null)
        {
            stackOfStates.PushState(potentialStates[0]);
        }
    }

    public bool LineSightToPlayer()
    {
        if (player && PlayerInFieldOfVision())
        {
            if (Physics.Raycast(transform.position, transform.forward, senseRange))
            {
                return true;
            }
        }
        return false;
    }

    public bool PlayerInFieldOfVision()
    {
        if(player)
        {
            Vector3 vectorBetweenThisAndPlayer = player.transform.position - transform.position;
            if(vectorBetweenThisAndPlayer.magnitude <= senseRange && Vector3.Angle(vectorBetweenThisAndPlayer, transform.forward) <= angleOfVision)
            {
                return true;
            }
        }
        return false;
    }

    public void GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
