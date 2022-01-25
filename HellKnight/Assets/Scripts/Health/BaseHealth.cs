using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour, IHealth
{

    protected List<IDeathListener> deathListeners = new List<IDeathListener>();

    public int maxHealth = 100;

    public int currentHealth;

    private void NotifyListenersOnDeath()
    {
        deathListeners.ForEach(deathListener => deathListener.OnDeath(this));
    }

    protected virtual void OnDeath()
    {
        NotifyListenersOnDeath();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }


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

}
