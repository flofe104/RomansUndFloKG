using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Testing;

[TestMonoBehaviour(CallStartBeforeTesting =true)]
public class MeleeEnemyMovement : BaseMovement
{
    public const float JUMP_POWER = 30f;
    public const float TURN_DURATION = 1f;
    public float range = 10f;

    protected GameObject player;

    public void Start()
    {
        jumpPower = JUMP_POWER;
        isGrounded = true;
        facedForward = true;
        turning = false;
        turnDuration = TURN_DURATION;
        //Debug.Log("yOffset: " + yOffset);
        player = GameObject.Find("Player");
    }

    public void JumpAtAngle()
    {
        float power = Mathf.Sqrt(2 * jumpPower * jumpPower) / 2;
        velocity.y = power;
        isGrounded = false;
        if (facedForward)
            velocity.x = power;
        else
            velocity.x = -power;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    protected void Move()
    {
        Controller.Move(velocity * Time.deltaTime);
    }

    public void Update()
    {
        ApplyGravity();

        if (player.transform.position.x < transform.position.x)
            TurnLeft();
        else
            TurnRight();
        if (turning)
            UpdateTurn();

        Move();
        CheckGround();
        if (isGrounded) velocity = Vector3.zero;
    }

    #region tests

    [Test]
    public void TestJump()
    {
        JumpAtAngle();
        Assert.AreEqual(JUMP_POWER, velocity.magnitude);
        Assert.AreEqual(velocity.x, velocity.y);
    }
    #endregion
}
