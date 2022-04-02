using System.Collections;
using UnityEngine;
using Testing;

[RequireComponent(typeof(CharacterController))]
[TestMonoBehaviour]
public class PlayerMovement : BaseMovement
{
    protected const float DASH_DISTANCE = 3f;
    protected const float DASH_DURATION = 0.15f;
    protected const float DASH_COOLDOWN = 1f;
    protected const float SPEED = 10f;
    protected const float JUMP_POWER = 30f;
    public const float TURN_DURATION = 0.5f;

    protected bool dashing = false;
    protected float dashTime = 0;
    protected float dashTimestamp = 0;
    protected Vector3 dashDirection = Vector3.right;


    public void Start()
    {
        speed = SPEED;
        jumpPower = JUMP_POWER;
        turnDuration = TURN_DURATION;
        isGrounded = true;
        yOffset = Controller.height / 2 + Controller.skinWidth;
    }

    protected void ApplyJumpForce()
    {
        velocity.y = jumpPower;
        isGrounded = false;
    }

    protected void Dash()
    {
        if (dashing && dashTime < DASH_DURATION)
        {
            //Debug.Log("Direction: " + direction);
            dashTime += Time.deltaTime;
            Vector3 direction;
            if (facedForward)
                direction = Vector3.right;
            else
                direction = Vector3.left;
            Controller.Move(direction * (DASH_DISTANCE / DASH_DURATION) * Time.deltaTime);
        }
        else
        {
            dashing = false;
            dashTime = 0;
        }
    }

    protected void Move(float input)
    {
        velocity.x = input * speed;
        //Debug.Log("Moving with velocity " + velocity);
        Controller.Move(velocity * Time.deltaTime);
    }

    protected virtual float GetHorizontalInput()
    {
        if (Input.GetKey(KeyCode.A))
            return -1f;
        else if (Input.GetKey(KeyCode.D))
            return 1f;
        else return 0f;
    }

    protected virtual bool GetVerticalInput()
    {
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);
    }

    protected virtual bool GetDashInput()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    public void Update()
    {
        ApplyGravity();

        float horizontalInput = GetHorizontalInput();

        bool jumpTriggered = GetVerticalInput();
        if (jumpTriggered && isGrounded)
            ApplyJumpForce();

        bool dashTriggered = GetDashInput();
        if (dashTriggered && !dashing && dashTimestamp + DASH_COOLDOWN < Time.time)
        {
            dashing = true;
            dashTimestamp = Time.time;
            if (horizontalInput < 0)
                dashDirection = Vector3.left;
            else if (horizontalInput > 0)
                dashDirection = Vector3.right;
        }
        if(dashing)
            Dash();

        Move(horizontalInput);

        if (!facedForward && horizontalInput > 0)
            TurnRight();
        else if (facedForward && horizontalInput < 0)
            TurnLeft();
        if (turning)
            UpdateTurn();

        CheckGround();
    }


    #region Tests

    [Test]
    public void TestHorizontalMovement()
    {
        
        var posBefore = transform.position;
        Move(1.0f);
        var posAfter = transform.position;
        var distance = posAfter.x - posBefore.x;
        Assert.ApproxEqual(distance, SPEED * Time.deltaTime);
    }

    [Test]
    public void TestJumpPower()
    {
        ApplyJumpForce();
        Assert.AreEqual(jumpPower, velocity.y);
    }

    [TestEnumerator]
    public IEnumerator TestDash()
    {
        var preDistance = transform.position;
        var preTime = Time.fixedTime;
        dashing = true;
        yield return new WaitForSeconds(DASH_DURATION);

        var postDistance = transform.position;
        var postTime = Time.fixedTime;

        Assert.ApproxEqual(postDistance.magnitude - DASH_DISTANCE, preDistance.magnitude, 0.5f);
        Assert.ApproxEqual(postTime - DASH_DURATION, preTime);
    }

    [TestEnumerator]
    public IEnumerator TestTurn()
    {        
        var startRotation = transform.rotation;
        TurnLeft();
        yield return new WaitForSeconds(TURN_DURATION);
        var endRotation = transform.rotation;

        var expectedRotation = startRotation * Quaternion.Euler(0, 180, 0);
        Assert.LessOrEqual(Quaternion.Angle(endRotation, expectedRotation), 5f);
        Assert.IsTrue(!turning);


        startRotation = transform.rotation;
        TurnRight();
        yield return new WaitForSeconds(TURN_DURATION);
        endRotation = transform.rotation;

        expectedRotation = startRotation * Quaternion.Inverse(Quaternion.Euler(0, 180, 0));
        Assert.LessOrEqual(Quaternion.Angle(endRotation, expectedRotation), 1f);
    }

    [Test]
    public void TestGravity()
    {
        isGrounded = false;
        float velY = velocity.y;
        ApplyGravity();
        Assert.Lesser(velocity.y, velY);
    }

    #endregion
}
