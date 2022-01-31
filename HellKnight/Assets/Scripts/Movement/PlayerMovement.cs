using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{

    public override float GetHorizontalInput()
    {
        if (Input.GetKey(KeyCode.A))
            return -1f;
        else if (Input.GetKey(KeyCode.D))
            return 1f;
        else return 0f;
    }

    public override bool GetVerticalInput()
    {
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);
    }

    public override bool GetDashInput()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }
}
