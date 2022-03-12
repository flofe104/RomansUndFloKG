using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Testing;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : BaseMovement
{
    protected const float dashDistance = 5;
    protected const float dashDuration = 0.5f;
    protected const float dashCooldown = 1;

    protected bool dashing = false;
    protected float dashTime = 0;
    protected float dashTimestamp = 0;
    protected Vector3 dashDirection;


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
        if (dashing && dashTime < dashDuration)
        {
            //Debug.Log("Direction: " + direction);
            dashTime += Time.deltaTime;
            Controller.Move(direction * (dashDistance / dashDuration) * Time.deltaTime);
        }
        else
        {
            dashing = false;
            dashTime = 0;
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
            if (horizontalInput < 0)
                dashDirection = Vector3.left;
            else if (horizontalInput > 0)
                dashDirection = Vector3.right;
        }
        if(dashing)
            Dash(dashDirection);

        Move(horizontalInput);

        if (!facedForward && horizontalInput > 0)
            TurnRight();
        else if (facedForward && horizontalInput < 0)
            TurnLeft();
        if (turning)
            UpdateTurn();

        CheckGround();
    }


    #region Tests
    [Test]
    public IEnumerator MovementTestDashDistance()
    {
        var preDistance = transform.position;
        var preTime = Time.fixedTime;

        dashing = true;
        yield return new WaitForSeconds(dashDuration);

        var postDistance = transform.position;
        var postTime = Time.fixedTime;

        Assert.AreEqual(postDistance.magnitude - dashDistance, preDistance.magnitude);
        Assert.AreEqual(postTime - dashDuration, preTime);
    }
    #endregion
}
