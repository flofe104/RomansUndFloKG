using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Testing;

[TestMonoBehaviour(CallStartBeforeTesting = true)]
public class MeleeEnemyCombat : MonoBehaviour
{
    public const float ATTACK_COOLDOWN = 2f;
    public const int ATTACK_DAMAGE = 10;
    protected float timeSinceAttack;
    protected Color color;
    protected Color baseColor;
    protected float colorStep;
    public Material material;
    protected MeleeEnemyMovement movementScript;

    public static string prefabForTestName = "TestMeleeEnemyPrefab";


    private void Start()
    {
        timeSinceAttack = Random.value * ATTACK_COOLDOWN;

        movementScript = GetComponent<MeleeEnemyMovement>();
        baseColor = material.color;
        color = baseColor;
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit: "+ other.gameObject.name);
        PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
        if (health != null)
        {
            OnHealthHit(health);
        }
    }

    private void OnHealthHit(BaseHealth health)
    {
        health.TakeDamage(ATTACK_DAMAGE);
    }

    protected void UpdateColor()
    {
        if (movementScript.GetIsGrounded())
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

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        if (movementScript.GetIsGrounded() && timeSinceAttack >= ATTACK_COOLDOWN)
        {
            movementScript.JumpAtAngle();
            timeSinceAttack = 0;
        }

        colorStep = 1f / ATTACK_COOLDOWN * Time.deltaTime;
        UpdateColor();
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
        yield return null;
        float after = timeSinceAttack;
        Assert.ApproxEqual(after, before);
    }

    [TestEnumerator]
    public IEnumerator TestColorChange()
    {
        if (timeSinceAttack > Time.deltaTime)
            yield return new WaitForSeconds(ATTACK_COOLDOWN - timeSinceAttack + Time.deltaTime);
        
        Assert.ApproxEqual(material.color.r, baseColor.r + colorStep);
        float r = baseColor.r;
        for (int i = 0; i < 5; i++)
        {
            yield return null;
            r+=colorStep;
            Assert.ApproxEqual(material.color.r, r);

        }
        yield return new WaitForSeconds(ATTACK_COOLDOWN - timeSinceAttack);
        Assert.AreEqual(material.color.r, baseColor.r);
    }
#endregion
}