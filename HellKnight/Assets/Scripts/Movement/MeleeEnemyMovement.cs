using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Testing;

[TestMonoBehaviour(CallStartBeforeTesting =true)]
public class MeleeEnemyMovement : BaseMovement
{
    public const float ATTACK_COOLDOWN = 2f;
    public const float JUMP_POWER = 30f;
    public const float TURN_DURATION = 1f;
    public float range = 10f;
    public Material material;

    protected GameObject player;
    protected float timeSinceAttack;
    protected Color color;
    protected Color baseColor;
    protected float colorStep;

    public void Start()
    {
        jumpPower = JUMP_POWER;
        isGrounded = true;
        facedForward = true;
        turning = false;
        turnDuration = TURN_DURATION;
        yOffset = 0.92f;
        player = GameObject.Find("Player");
        timeSinceAttack = Random.value * ATTACK_COOLDOWN;
        baseColor = material.color;
        color = baseColor;

        //Debug.Log("pos1: " + transform.position);
        //var pos = transform.position;
        //Move();
        //Debug.Log("pos2: " + transform.position);
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

    protected void UpdateColor()
    {
        if (isGrounded)
        {
            color.r += colorStep;
        }
        else
        {
            color = baseColor;
        }
        material.color = color;        
        //Debug.Log("Redness: " + material.color.r);
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

        colorStep = 1f / ATTACK_COOLDOWN * Time.deltaTime;
        UpdateColor();

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

    private void OnDestroy()
    {
        material.color = baseColor;
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

    [TestEnumerator]
    public IEnumerator TestColorChange()
    {
        if(timeSinceAttack > Time.deltaTime)
            yield return new WaitForSeconds(ATTACK_COOLDOWN - timeSinceAttack + Time.deltaTime);
        Assert.ApproxEqual(material.color.r, baseColor.r + colorStep);
        yield return new WaitForSeconds(5 * Time.deltaTime);
        Assert.ApproxEqual(material.color.r, baseColor.r + 5 * colorStep);
        yield return new WaitForSeconds(ATTACK_COOLDOWN - timeSinceAttack);
        Assert.AreEqual(material.color.r, baseColor.r);
    }
    #endregion
}
