using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerBase : CharacterBase
{
    public CharacterController Controller;

    private Vector3 playerVelocity;
    private bool isCrouching;

    protected override void Awake()
    {
        base.Awake();
        OnDeath += onDeath;
        OnDamage += onDamage;
        Controller = GetComponent<CharacterController>();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsStunned)
            MovePlayer();

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
        Vector3.ClampMagnitude(move, 1.0f);

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
        ////check if the players velocity will result the player from going through a wall
        //RaycastHit hit;
        //Vector3 startRay = transform.position + (normlisedVelocity * Controller.radius);
        //if (Physics.Raycast(startRay, normlisedVelocity, out hit, worldVelocity.magnitude) && hit.collider.tag != "Portal")
        //{
        //    worldVelocity = normlisedVelocity * hit.distance;
        //    Controller.Move(worldVelocity);
        //}
        //else
        //{
        //    Controller.Move(velocity);
        //}
        Controller.Move(velocity);
    }

    private void onDamage()
    {
        FindObjectOfType<PlayerCamera>()?.ShakePosition(0.1f, new Vector2(0.2f, 0.8f), -0.01f, 0.1f, 20.0f);
    }

    private void onDeath()
    {
        DataPersitanceHelpers.LoadAll();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnDeath -= onDeath;
        OnDamage -= onDamage;
    }

    public override void Save(DataContext context)
    {
        base.Save(context);

        var UUID = GetComponent<UUID>()?.ID;

        context.SaveData(UUID, "velocity", playerVelocity);
        context.SaveData(UUID, "onGround", onGround);
        context.SaveData(UUID, "isCrouching", isCrouching);
    }

    public override void Load(DataContext context, bool disableUnloaded = false)
    {
        base.Load(context, disableUnloaded);
        var UUID = GetComponent<UUID>()?.ID;

        playerVelocity = context.GetValue<Vector3>(UUID, "velocity");
        onGround = context.GetValue<bool>(UUID, "onGround");
        isCrouching = context.GetValue<bool>(UUID, "isCrouching");
    }
}
