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
    public float AttackDistance = 2.0f;

    public Transform[] PatrolPoints;
    public int CurrentPatrolPoint = 0;
    public int NextPatrolPoint = 1;
    public State[] PotentialStates;

    public AIUseableAbilitiy LeftAbility;
    public AIUseableAbilitiy RightAbilitiy;

    private NavMeshAgent navAgent;
    private StackFSM stackOfStates;
    private GameObject player;
    public Vector3? currentDestination;
    #endregion

    #region Properties
    public NavMeshAgent NavAgent => navAgent;
    public GameObject Player => player;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        OnDeath += onDeath;

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

    protected override void Update()
    {
        base.Update();

        if (PotentialStates.Length > 0 && stackOfStates.GetCurrentState() == null)
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
            RaycastHit HitSomething;

            //get direction to player
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;
            if (Portal.RaycastRecursive(transform.position, playerDirection, 1, out HitSomething, SenseRange))
            {
                //traverse up the parents to see if any object was the player
                if(HitSomething.collider)
                    return ParentHasPlayerTag(HitSomething.collider.transform);
            }
        }
        return false;
    }

    private bool ParentHasPlayerTag(Transform currentTransform)
    {
        if (currentTransform.tag == "Player")
            return true;

        if (!currentTransform.parent)
            return false;

        return ParentHasPlayerTag(currentTransform.parent);
    }

    public bool PlayerInFieldOfVision()
    {
        if (player)
        {
            Vector3 vectorBetweenThisAndPlayer = player.transform.position - transform.position;
            if (vectorBetweenThisAndPlayer.magnitude <= SenseRange && Vector3.Angle(vectorBetweenThisAndPlayer, transform.forward) <= AngleOfVision)
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

    public override void PortalableObjectOnHasTeleported(Portal sender, Portal destination, Vector3 newPosition, Quaternion newRotation)
    {
        if (!navAgent.Warp(newPosition))
            Debug.LogWarning($"Warp failed for {gameObject.name} NavMeshAgent.");

        if (currentDestination != null)
        {
            var path = new NavMeshPath();
            navAgent.CalculatePath(currentDestination.Value, path);
            navAgent.SetPath(path);
        }
    }

    private void FixedUpdate()
    {
        if (navAgent.isOnOffMeshLink)
        {
            // Move character towards OffMeshLink start point
            // Should only really be used if the AI reaches the link without finishing going through the portal.

            transform.Translate(Vector3.ProjectOnPlane(navAgent.currentOffMeshLinkData.startPos - transform.position, Vector3.up).normalized * (navAgent.speed * Time.fixedDeltaTime));
        }
    }

    private void onDeath()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnDeath -= onDeath;
    }
}
