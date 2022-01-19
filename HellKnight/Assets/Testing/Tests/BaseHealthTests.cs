using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BaseHealthTests : BaseHealth
{
    [Test]
    public void TestHeal()
    {
        currentHealth = 1;
        var previousHealth = currentHealth;
        int heal = maxHealth - currentHealth - 1;
        Heal(heal);

        Assert.AreEqual(currentHealth, previousHealth + heal);
    }

    [Test]
    public void TestDamage()
    {
        int previousHealth = currentHealth;
        int damage = currentHealth - 1;

        Damage(damage);

        Assert.AreEqual(currentHealth, previousHealth - damage);
        Assert.AreEqual(isDead, false);
    }

    [Test]
    public void TestFatalDamage()
    {
        int damage = maxHealth;

        Damage(damage);

        Assert.AreEqual(currentHealth, 0);
        Assert.AreEqual(isDead, true);
    }
}
