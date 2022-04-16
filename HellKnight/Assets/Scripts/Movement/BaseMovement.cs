using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class BaseMovement : MonoBehaviour
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
    [SerializeField]
    protected new Collider collider;

    protected Collider Collider
    {
        get
        {
            if (collider == null)
            {
                collider = GetComponent<Collider>();
            }
            return collider;
        }
    }
    protected Vector3 velocity;
    protected bool isGrounded;
    protected bool turning = false;
    protected bool facedForward = true;
    protected Quaternion endRotation;

    public float speed;
    public float jumpPower;
    public float turnDuration;
    public const float GRAVITY = 80f;

    protected void ApplyGravity()
    {
        velocity.y -= GRAVITY * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
            velocity.y = 0;
    }
    protected void CheckGround()
    {
        RaycastHit hit;

        var sphereRadius = (Collider.bounds.max.x - Collider.bounds.min.x) / 2;
        var distanceToFoot = transform.position.y - Collider.bounds.min.y;

        if (Physics.SphereCast(transform.position, sphereRadius, transform.TransformDirection(Vector3.down), out hit))
        {
            if (hit.collider.gameObject != gameObject && !hit.collider.isTrigger && hit.collider.gameObject.layer != gameObject.layer)
            {
                var minDistanceToGround = Mathf.Max(distanceToFoot - sphereRadius, 0f) + Controller.skinWidth + 0.1f;
                if (hit.distance <= minDistanceToGround)
                {
                    //Debug.Log(gameObject.name+" grounded at " + hit.distance);
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            }
        }
    }
    protected void TurnRight()
    {
        turning = true;
        endRotation = Quaternion.Euler(0, 0, 0);
        facedForward = true;
    }
    protected void TurnLeft()
    {
        turning = true;
        endRotation = Quaternion.Euler(0, -180, 0);
        facedForward = false;
    }

    protected void UpdateTurn()
    {
        var step = 180f * Time.deltaTime / turnDuration;
        var q = Quaternion.RotateTowards(transform.rotation, endRotation, step);
        transform.rotation = q;
        if (endRotation.eulerAngles.y == transform.rotation.eulerAngles.y)
        {
            turning = false;
        }
    }
}
