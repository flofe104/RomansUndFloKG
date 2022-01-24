using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{

    public override float GetHorizontalInput()
    {
        return Input.GetAxis("Horizontal");
    }

    public override float GetVerticalInput()
    {
        return Mathf.Max(Input.GetAxis("Vertical"), Input.GetAxis("Jump"));
    }

    public override bool GetDashInput()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }
}
