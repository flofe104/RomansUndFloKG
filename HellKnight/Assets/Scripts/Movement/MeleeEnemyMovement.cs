using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Testing;

[TestMonoBehaviour(CallStartBeforeTesting =true)]
public class MeleeEnemyMovement : BaseMovement
{
    public const float ATTACK_COOLDOWN = 2f;
    public const float ROTATION_SPEED = 500f;
    public const float JUMP_POWER = 30f;
    public float range = 10f;

    protected GameObject player;
    protected float timeSinceAttack;



    public static string prefabForTestName = "TestMeleeEnemyPrefab";

    public void Start()
    {
        jumpPower = JUMP_POWER;
        isGrounded = true;
        facedForward = true;
        turning = false;
        rotationSpeed = ROTATION_SPEED;
        yOffset = 0.92f;
        player = GameObject.Find("Player");
        timeSinceAttack = Random.value * ATTACK_COOLDOWN;
    }

    protected void JumpAtAngle()
    {
        float power = Mathf.Sqrt(2 * jumpPower * jumpPower) / 2;
        velocity.y = power;
        isGrounded = false;
        if (facedForward)
            velocity.x = power;
        else
            velocity.x = -power;
    }

    protected void Move()
    {
        Controller.Move(velocity * Time.deltaTime);
    }

    public void Update()
    {
        timeSinceAttack += Time.deltaTime;
        ApplyGravity();
        if (isGrounded && timeSinceAttack >= ATTACK_COOLDOWN)
        {
            JumpAtAngle();
            timeSinceAttack = 0;
        }

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

    [TestEnumerator]
    public IEnumerator TestCooldown()
    {
        float before = timeSinceAttack;
        yield return new WaitForSeconds(ATTACK_COOLDOWN);
        float after = timeSinceAttack;
        Assert.ApproxEqual(after, before);
    }

    [Test]
    public void TestJump()
    {
        JumpAtAngle();
        Assert.AreEqual(JUMP_POWER, velocity.magnitude);
        Assert.AreEqual(velocity.x, velocity.y);
    }
    #endregion
}
