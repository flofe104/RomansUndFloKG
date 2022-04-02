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
    protected Vector3 velocity;
    protected bool isGrounded;
    protected float yOffset;
    protected bool turning = false;
    protected bool facedForward;
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
        float sphereRadius = gameObject.GetComponent<Collider>().transform.localScale.x / 2;
        if (Physics.SphereCast(transform.position, sphereRadius, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            //Debug.Log(gameObject.name+ " hit " + hit.collider.gameObject.name + " dist " + hit.distance + " isGrounded: " + isGrounded);
            //var hitName = hit.collider.gameObject.name;
            if (hit.collider.gameObject != gameObject && !hit.collider.isTrigger)
            {
                if (hit.distance <= yOffset - sphereRadius + 0.001f)
                {
                    //Debug.Log("Distance when grounded: " + hit.distance);
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
