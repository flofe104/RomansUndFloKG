using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementTest : PlayerMovement
{
    bool simulatedVertical;
    protected override bool GetVerticalInput()
    {
        return simulatedVertical;
    }

    // A Test behaves as an ordinary method
    [Test]
    public void MovementTestJumpPower()
    {
        simulatedVertical = true;
        isGrounded = true;
        ApplyJumpForce();
        Assert.AreEqual(jumpPower, velocity.y);
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
