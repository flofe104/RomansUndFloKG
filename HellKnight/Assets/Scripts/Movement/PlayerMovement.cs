using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : BaseMovement
{

    protected bool dashing = false;
    protected float dashTime = 0;
    protected float dashTimestamp = 0;

    public AnimationCurve dashPattern;
    public float dashCooldown = 1;
    public float dashDistance = 0.1f;
    public float dashSpeed = 0.25f;


    public void Start()
    {
        speed = 10;
        jumpPower = 30f;
        gravity = 80f;
        isGrounded = true;
        yOffset = Controller.height / 2 + Controller.skinWidth;
        rotationSpeed = 1000;
    }

    protected void ApplyJumpForce()
    {
        velocity.y = jumpPower;
        isGrounded = false;
    }

    protected void Dash(Vector3 direction)
    {
        if (dashing)
        {
            //Debug.Log("Direction: " + direction);
            dashTime += Time.deltaTime;
            var currentVal = dashPattern.Evaluate(dashTime / dashSpeed);
            if (currentVal == 0)
            {
                dashing = false;
            }
            else
            {
                Controller.Move(direction * 0.01f * dashDistance * currentVal);
            }
        }
    }

    protected void Move(float input)
    {
        velocity.x = input * speed;
        //Debug.Log("Moving with velocity " + velocity);
        Controller.Move(velocity * Time.deltaTime);
    }

    protected virtual float GetHorizontalInput()
    {
        if (Input.GetKey(KeyCode.A))
            return -1f;
        else if (Input.GetKey(KeyCode.D))
            return 1f;
        else return 0f;
    }

    protected virtual bool GetVerticalInput()
    {
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);
    }

    protected virtual bool GetDashInput()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    public void Update()
    {
        ApplyGravity();

        float horizontalInput = GetHorizontalInput();

        bool jumpTriggered = GetVerticalInput();
        if (jumpTriggered && isGrounded)
            ApplyJumpForce();

        bool dashTriggered = GetDashInput();
        if (dashTriggered && !dashing && dashTimestamp + dashCooldown < Time.time)
        {
            dashing = true;
            dashTimestamp = Time.time;
            dashTime = 0;
        }
        if (horizontalInput < 0)
            Dash(Vector3.left);
        else if (horizontalInput > 0)
            Dash(Vector3.right);

        Move(horizontalInput);

        if (!facedForward && horizontalInput > 0)
            TurnRight();
        else if (facedForward && horizontalInput < 0)
            TurnLeft();
        if (turning)
            UpdateTurn();

        CheckGround();
    }
}
