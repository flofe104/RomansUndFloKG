using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    public void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public override float GetHorizontalInput()
    {
        return Input.GetAxis("Horizontal");
    }

    public override float GetVerticalInput()
    {
        return Input.GetAxis("Vertical");
    }
}
