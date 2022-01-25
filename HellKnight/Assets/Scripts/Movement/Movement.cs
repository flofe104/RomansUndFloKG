using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Movement : MonoBehaviour
{

    [SerializeField]
    protected CharacterController controller;

    protected CharacterController Controller
    {
        get
        {
            if (controller == null)
            {
                controller = GetComponent<CharacterController>();
            }
            return controller;
        }
    }

    protected Vector3 velocity;
    protected bool isGrounded = true;
    protected bool facedForward = true;
    protected bool turning = false;
    protected bool dashing = false;
    protected float dashTime = 0;
    protected float dashTimestamp = 0;
    protected Quaternion endRotation;


    public float speed = 10;
    public float jumpPower = 3.5f;
    public float gravity = 0.3f;
    public float rotationSpeed = 1000;
    public AnimationCurve dashPattern;
    public float dashCooldown = 1;
    public float dashDistance= 0.1f;
    public float dashSpeed = 0.25f;

    protected float GetJumpPower => Mathf.Sqrt(1000f * jumpPower * gravity);
    protected float GetDashEndTime => dashPattern.keys[dashPattern.keys.Length - 1].time;

    public void ApplyJumpForce()
    {
        velocity.y = GetJumpPower;
        isGrounded = false;
    }

    public void Dash(Vector3 direction)
    {
        if (dashing)
        {
            //Debug.Log("Direction: " + direction);
            dashTime += Time.deltaTime;
            var val = 0.01f * dashDistance * dashPattern.Evaluate(dashTime / dashSpeed);
            Controller.Move(direction * val);
            if (GetDashEndTime <= dashTime)
            {
                dashing = false;
            }
        }
    }

    public void ApplyGravity()
    {
        velocity.y -= gravity;
        if (isGrounded && velocity.y < 0)
            velocity.y = 0;
    }

    public void CheckGround()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            //Debug.Log("Hit " + hit.collider.gameObject.name + " dist " + hit.distance + "isGrounded: " + isGrounded);
            var hitName = hit.collider.gameObject.name;
            if (hit.collider.gameObject != gameObject && !hit.collider.isTrigger)
            {
                float contactPoint = gameObject.GetComponent<CapsuleCollider>().height / 2 + Controller.skinWidth;
                if (hit.distance + velocity.y * Time.deltaTime <= contactPoint)
                {
                    Debug.Log("Distance when grounded: " + hit.distance);
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            }
        }
    }

    public void Move(float input)
    {
        velocity.x = input * speed;
        //Debug.Log("Moving with velocity " + velocity);
        Controller.Move(velocity * Time.deltaTime);
    }

    public void UpdateTurn(float horizontalInput)
    {
        if (facedForward && horizontalInput < 0)
        {
            turning = true;
            endRotation = Quaternion.Euler(0, 270, 0);
        }
        else if (!facedForward && horizontalInput > 0)
        {
            turning = true;
            endRotation = Quaternion.Euler(0, 90, 0);
        }
    }

    public void Turn()
    {
        var q = Quaternion.RotateTowards(transform.rotation, endRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = q;
        if (System.Math.Abs(endRotation.eulerAngles.y - transform.rotation.eulerAngles.y) < 0.00001)
        {
            facedForward = !facedForward;
            turning = false;
        }
    }

    public abstract float GetHorizontalInput();
    public abstract bool GetVerticalInput();
    public abstract bool GetDashInput();

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

        UpdateTurn(horizontalInput);
        if (turning)
            Turn();

        CheckGround();
    }


}
