using System.Collections;
using System.Collections.Generic;
using Testing;
using UnityEngine;

[TestMonoBehaviour]
public class BaseHealth : MonoBehaviour, IHealth
{

    protected List<IDeathListener> deathListeners = new List<IDeathListener>();

    public int maxHealth = 100;

    public int currentHealth;

    private void NotifyListenersOnDeath()
    {
        deathListeners.ForEach(deathListener => deathListener.OnDeath(this));
    }

    private void OnDeath()
    {
        NotifyListenersOnDeath();
        OnEntityDied();
    }

    protected virtual void OnEntityDied() { }

    protected void Awake()
    {
        currentHealth = maxHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    protected bool IsDead => currentHealth <= 0;

    public void ResetWithMaxHealth(int maxHealth)
    {
        SetMaxHealth(maxHealth);
        currentHealth = this.maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void AddDeathListener(IDeathListener listener)
    {
        deathListeners.Add(listener);
    }

    public void TakeDamage(int damage)
    {
        if (!IsDead)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                OnDeath();
                currentHealth = 0;
            }
        }
    }

    public void HealDamage(int damage)
    {
        if (!IsDead)
        {
            currentHealth += damage;
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    #region tests

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
        ResetWithMaxHealth(10);
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
    #endregion

}
