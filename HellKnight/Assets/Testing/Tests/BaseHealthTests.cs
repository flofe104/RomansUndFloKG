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
        HealDamage(heal);

        Assert.AreEqual(currentHealth, previousHealth + heal);
    }

    [Test]
    public void TestDamage()
    {
        int previousHealth = currentHealth;
        int damage = currentHealth - 1;

        TakeDamage(damage);

        Assert.AreEqual(currentHealth, previousHealth - damage);
        Assert.AreEqual(IsDead, false);
    }

    [Test]
    public void TestFatalDamage()
    {
        int damage = maxHealth;

        TakeDamage(damage);

        Assert.AreEqual(currentHealth, 0);
        Assert.AreEqual(IsDead, true);
    }


    [Test]
    public void TestNoRevive()
    {
        int damage = maxHealth;

        TakeDamage(damage);
        HealDamage(1);
        Assert.AreEqual(IsDead, true);
    }
}
