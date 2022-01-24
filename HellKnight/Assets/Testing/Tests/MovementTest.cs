using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementTest : Movement
{

    public override float GetHorizontalInput()
    {
        return simulatedHorizontal;
    }

    public override float GetVerticalInput()
    {
        return simulatedVertical;
    }
    public override bool GetDashInput()
    {
        return simulatedDash;
    }

    protected bool simulatedDash;
    protected float simulatedHorizontal;
    protected float simulatedVertical;

    // A Test behaves as an ordinary method
    [Test]
    public void MovementTestJumpPower()
    {
        simulatedVertical = 1;
        isGrounded = true;
        ApplyJumpForce();
        Assert.AreEqual(GetJumpPower, velocity.y);
    }

    [Test]
    public void MovementTestGravity()
    {
        isGrounded = false;
        float velY = velocity.y;
        ApplyGravity();
        Assert.Less(velocity.y, velY);
    }



    //[UnityTest]
    //public IEnumerator MovementTestWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}

}
