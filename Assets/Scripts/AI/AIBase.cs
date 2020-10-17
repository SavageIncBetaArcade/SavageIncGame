using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBase : CharacterBase
{
    public Vector3[] patrolPoints;
    private int currentPatrolPoint = 0;
    private NavMeshAgent navAgent;
    private StackFSM stackOfStates;
    public State[] potentialStates;


    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        stackOfStates = GetComponent<StackFSM>();
    }

}
