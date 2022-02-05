using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class ProjectileTest : Projectile
{
    [Test]
    public void TestSpeed()
    {
        var positionBefore = transform.position;
        Waiter(1f);
        var positionAfter = transform.position;
        var distanceTravelled = Vector3.Distance(positionBefore, positionAfter);
        Assert.LessOrEqual(distanceTravelled, 50);
        Assert.GreaterOrEqual(distanceTravelled, 10);
    }
    [Test]
    public void TestDamage()
    {
        var health = FindObjectOfType<PlayerHealth>();
        OnHealthHit(health);
        Assert.LessOrEqual(health.currentHealth, health.maxHealth - 1);
        Assert.GreaterOrEqual(health.currentHealth, health.maxHealth - 20);
    }

    IEnumerator Waiter(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
