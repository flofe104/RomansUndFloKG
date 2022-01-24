using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour, IHealth
{

    protected List<IDeathListener> deathListeners = new List<IDeathListener>();

    public int maxHealth = 100;

    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            HealDamage(10);
        }
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
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    public void HealDamage(int damage)
    {
        currentHealth += damage;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

}
