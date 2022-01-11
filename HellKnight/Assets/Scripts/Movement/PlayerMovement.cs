using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Movement playerMovement;

    void Start()
    {
        playerMovement = new Movement(GetComponent<CharacterController>());
    }

    void FixedUpdate()
    {
        playerMovement.ApplyGravity(Time.deltaTime);

        // Get horizontal input
        float horizontalInput = Input.GetAxis("Horizontal");

        // Get vertical input
        bool jumpTriggered = Input.GetAxis("Vertical") > 0;
        if (jumpTriggered)
        {
            playerMovement.ApplyJumpForce();
        }

        playerMovement.Move(horizontalInput, Time.deltaTime);
    }
}