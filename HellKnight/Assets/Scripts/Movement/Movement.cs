using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    protected CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded = true;

    private bool facedForward = true;
    private bool turning = false;
    private Quaternion endRotation;


    public float speed = 10;
    public float jumpHeight = 15;
    public float gravity = -4;
    public float rotationSpeed = 500;

    public void ApplyJumpForce()
    {
        if (isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -0.01f * gravity);
            isGrounded = false;
        }
    }

    public void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
            velocity.y = 0;
    }

    public void CheckGround()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            if(hit.rigidbody != null)
            {
                Debug.Log("Hit " + hit.rigidbody.gameObject.name + " dist " + hit.distance);
                if (hit.distance <= 1.1 && hit.rigidbody.gameObject.name == "Ground")
                {
                    isGrounded = true;
                }
            }
        }
    }

    public void Move(float input)
    {
        velocity.x = input * speed * Time.deltaTime;
        //Debug.Log("Moving with velocity " + velocity);
        controller.Move(velocity);

    }

    public void Turn(float horizontalInput)
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

        if (turning)
        {
            var q = Quaternion.RotateTowards(transform.rotation, endRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = q;
            if (System.Math.Abs(endRotation.eulerAngles.y - transform.rotation.eulerAngles.y) < 0.00001)
            {
                facedForward = !facedForward;
                turning = false;
            }
        }           
    }

    public abstract float GetHorizontalInput();
    public abstract float GetVerticalInput();

    public void Update()
    {
        CheckGround();
        ApplyGravity();

        float horizontalInput = GetHorizontalInput(); 

        bool jumpTriggered = GetVerticalInput() > 0;
        if (jumpTriggered)
            ApplyJumpForce();

        Move(horizontalInput);

        Turn(horizontalInput);
    }

}
