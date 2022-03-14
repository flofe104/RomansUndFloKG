using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMovement : BaseMovement
{
    public float jumpCooldown = 2f;
    public float range = 10f;

    protected GameObject player;
    protected float t;

    public void Start()
    {
        jumpPower = 30f;
        gravity = 80f;
        isGrounded = true;
        facedForward = true;
        turning = false;
        rotationSpeed = 500;
        yOffset = 0.92f;
        player = GameObject.Find("Player");
        t = Random.value * jumpCooldown;
    }

    protected void JumpAtAngle()
    {
        velocity.y = jumpPower;
        isGrounded = false;
        if (facedForward)
            velocity.x = jumpPower / 2;
        else
            velocity.x = -jumpPower / 2;
    }

    protected void Move()
    {
        Controller.Move(velocity * Time.deltaTime);
    }

    public void Update()
    {
        t += Time.deltaTime;
        ApplyGravity();
        if (isGrounded && t >= jumpCooldown)
        {
            JumpAtAngle();
            t = 0;
        }

        if (player.transform.position.x < transform.position.x)
            TurnLeft();
        else
            TurnRight();
        if (turning)
            UpdateTurn();

        Move();
        CheckGround();
        if (isGrounded) velocity = Vector3.zero;
    }
}
