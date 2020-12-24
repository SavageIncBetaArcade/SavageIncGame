using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerBase : CharacterBase
{
    public CharacterController Controller;

    private Vector3 playerVelocity;
    private bool isCrouching;

    private Vector3 lastPosition;
    protected override void Awake()
    {
        base.Awake();
        OnDeath += onDeath;
        Controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();

        if (!IsStunned)
            MovePlayer();

        lastPosition = transform.position;

        //temp kill player
        if (Input.GetKeyDown(KeyCode.F5))
        {
            TakeDamage(float.MaxValue);
        }


    }

    void MovePlayer()
    {
        //Check if on ground
        onGround = Controller.isGrounded;
        if (onGround && playerVelocity.y < 0)
        {
            Controller.slopeLimit = 45;
            playerVelocity.y = -2.0f;
        }

        //Calculate move direction
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        //Toggle crouching
        if (Input.GetButtonDown("Crouch") && !isCrouching)
        {
            isCrouching = true;
            Controller.height = Controller.height / 2;
            Speed = Speed / 2;
        }
        else if (Input.GetButtonDown("Crouch") && isCrouching)
        {
            isCrouching = false;
            Controller.height = Controller.height * 2;
            Speed = Speed * 2;
        }

        //Check if sprinting and move
        if (Input.GetButton("Sprint") && !isCrouching)
        {
            Move(move * (Speed * 1.5f) * Time.deltaTime);
        }
        else
        {
            Move(move * Speed * Time.deltaTime);
        }

        //Check if jump pressed
        if (Input.GetButtonDown("Jump") && onGround && !isCrouching)
        {
            Controller.slopeLimit = 90; //Prevents stuttering when jumping next to object
            playerVelocity.y = Mathf.Sqrt(JumpHeight * -2.0f * Gravity);
        }

        //Apply gravity
        playerVelocity.y += Gravity * Time.deltaTime;

        Controller.Move(playerVelocity * Time.deltaTime);
    }

    private void Move(Vector3 velocity)
    {
        Vector3 worldVelocity = velocity + (transform.position - lastPosition);
        Vector3 normlisedVelocity = worldVelocity.normalized;

        //check if the players velocity will result the player from going through a wall
        RaycastHit hit;
        Vector3 startRay = transform.position + (normlisedVelocity * Controller.radius);
        if (Physics.Raycast(startRay, normlisedVelocity, out hit, worldVelocity.magnitude) && hit.collider.tag != "Portal")
        {
            worldVelocity = normlisedVelocity * hit.distance;
            Controller.Move(worldVelocity);
        }
        else
        {
            Controller.Move(velocity);
        }
    }

    private void onDeath()
    {
        //Temp, if the player dies just reload the scene
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnDeath -= onDeath;
    }

    public override void PortalableObjectOnHasTeleported(Portal startPortal, Portal endPortal, Vector3 newposition, Quaternion newrotation)
    {
        base.PortalableObjectOnHasTeleported(startPortal, endPortal, newposition, newrotation);

        lastPosition = newposition;
    }

    public override Dictionary<string, object> Save()
    {
        var dataDictionary = base.Save();

        var UUID = GetComponent<UUID>()?.ID;
        if (string.IsNullOrWhiteSpace(UUID))
        {
            Debug.LogError("CharacterBase doesn't have an UUID (Can't load data from json)");
            return dataDictionary;
        }

        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "velocity", playerVelocity);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "onGround", onGround);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "isCrouching", isCrouching);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "lastPosition", lastPosition);

        DataPersitanceHelpers.SaveDictionary(ref dataDictionary, UUID);
        return dataDictionary;
    }

    public override Dictionary<string, object> Load(bool disableUnloaded = false)
    {
        var dataDictionary = base.Load(disableUnloaded);

        playerVelocity = DataPersitanceHelpers.GetValueFromDictionary<Vector3>(ref dataDictionary, "velocity");
        onGround = DataPersitanceHelpers.GetValueFromDictionary<bool>(ref dataDictionary, "onGround");
        isCrouching = DataPersitanceHelpers.GetValueFromDictionary<bool>(ref dataDictionary, "isCrouching");
        lastPosition = DataPersitanceHelpers.GetValueFromDictionary<Vector3>(ref dataDictionary, "lastPosition");

        return dataDictionary;
    }
}
