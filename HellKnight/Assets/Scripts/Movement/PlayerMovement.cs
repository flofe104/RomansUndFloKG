using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 10;
    public float jumpHeight = 10;
    public float gravity = -5;

    private Vector3 playerVelocity = new Vector3();

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        playerVelocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        if (controller.isGrounded && playerVelocity.y < 0)
            playerVelocity.y = 0;

        bool jumpTriggered = Input.GetAxis("Vertical") > 0;
        if (jumpTriggered && controller.isGrounded)
        {
            //Debug.Log("Jumping");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -0.01f * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity);
        //Debug.Log("Moving player by " + velocity);
    }
}