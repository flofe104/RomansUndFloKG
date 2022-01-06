using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    private CharacterController controller;
    private float speed;
    private float jumpHeight;
    private float gravity;
    private Vector3 velocity;

    public Movement(CharacterController controller, float speed = 10, float jumpHeight = 10, float gravity = -5)
    {
        this.controller = controller;
        this.speed = speed;
        this.jumpHeight = jumpHeight;
        this.gravity = gravity;
        velocity = new Vector3();
    }

    public void ApplyJumpForce()
    {
        if(controller.isGrounded)
            velocity.y += Mathf.Sqrt(jumpHeight * -0.01f * gravity);
    }

    public void ApplyGravity(float dT)
    {
        velocity.y += gravity * dT;
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = 0;
    }

    public void Move(float input, float dT)
    {
        velocity.x = input * speed * dT;
        Debug.Log("Moving with velocity " + velocity);
        controller.Move(velocity);
    }

}
