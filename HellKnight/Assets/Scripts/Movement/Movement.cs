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
            if(controller == null)
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
    protected Quaternion endRotation;


    public float speed = 10;
    public float jumpPower = 0.02f;
    public float gravity = 0.5f;
    public float rotationSpeed = 500;

    public void ApplyJumpForce()
    {
        if (isGrounded)
        {
            velocity.y = GetJumpPower;
            isGrounded = false;
        }
    }

    protected float GetJumpPower =>  Mathf.Sqrt(jumpPower * gravity);

    public void ApplyGravity()
    {
        velocity.y -= gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
            velocity.y = 0;
    }

    public void CheckGround()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            if (hit.rigidbody != null)
            {
                //Debug.Log("Hit " + hit.rigidbody.gameObject.name + " dist " + hit.distance);
                if (hit.rigidbody.gameObject.name == "Ground")
                {
                    float contactPoint = gameObject.GetComponent<CapsuleCollider>().height / 2 + Controller.skinWidth;
                    if (hit.distance + velocity.y * Time.deltaTime <= contactPoint)
                    {
                        isGrounded = true;
                    }
                }
            }
        }
    }

    public void Move(float input)
    {
        velocity.x = input * speed * Time.deltaTime;
        //Debug.Log("Moving with velocity " + velocity);
        Controller.Move(velocity);        
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
    public abstract float GetVerticalInput();

    public void Update()
    {
        ApplyGravity();

        float horizontalInput = GetHorizontalInput(); 

        bool jumpTriggered = GetVerticalInput() > 0;
        if (jumpTriggered)
            ApplyJumpForce();

        Move(horizontalInput);

        UpdateTurn(horizontalInput);
        if (turning)
            Turn();

        CheckGround();
    }


}
