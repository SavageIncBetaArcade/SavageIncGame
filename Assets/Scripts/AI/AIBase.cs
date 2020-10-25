using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AIBase : CharacterBase
{
    #region member varibles
    public float SenseRange = 60.0f;
    public float AngleOfVision = 45.0f;
    public float WalkDistance = 50.0f;

    public Transform[] PatrolPoints;
    public int CurrentPatrolPoint = 0;
    public int NextPatrolPoint = 1;
    public State[] PotentialStates;

    private NavMeshAgent navAgent;
    private StackFSM stackOfStates;
    private GameObject player;
    #endregion

    #region Properties
    public NavMeshAgent NavAgent    =>  navAgent;
    public GameObject   Player      =>  player;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        navAgent = GetComponent<NavMeshAgent>();
        stackOfStates = GetComponent<StackFSM>();

        if (PotentialStates.Length > 0)
        {
            State patrol = PotentialStates.FirstOrDefault(x => x.StateName == StateNames.PatrolState);
            State idle = PotentialStates.FirstOrDefault(x => x.StateName == StateNames.IdleState);
            if (patrol)
                stackOfStates.PushState(patrol);
            else if (idle)
                stackOfStates.PushState(idle);
        }

        GetPlayer();

        //TODO when speed changes also set the navAgentSpeed
        navAgent.speed = Speed;
    }

    private void Update()
    {
        if(PotentialStates.Length > 0 && stackOfStates.GetCurrentState() == null)
        {
            State idle = PotentialStates.FirstOrDefault(x => x.StateName == StateNames.IdleState);
            if (idle)
                stackOfStates.PushState(idle);
        }
    }

    public bool LineSightToPlayer()
    {
        if (player && PlayerInFieldOfVision())
        {
            if (Physics.Raycast(transform.position, transform.forward, SenseRange))
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
            if(vectorBetweenThisAndPlayer.magnitude <= SenseRange && Vector3.Angle(vectorBetweenThisAndPlayer, transform.forward) <= AngleOfVision)
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
