using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerBase : CharacterBase
{
    public CharacterController Controller;

    private Vector3 playerVelocity;
    private bool onGround;
    private bool isCrouching;

    protected override void Awake()
    {
        base.Awake();

        Controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
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
            Controller.Move(move * (Speed * 1.5f) * Time.deltaTime);
        }
        else
        {
            Controller.Move(move * Speed * Time.deltaTime);
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
}
