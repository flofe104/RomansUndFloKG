using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour, IHealth
{

    protected List<IDeathListener> deathListeners = new List<IDeathListener>();

    protected int maxHealth;

    protected int currentHealth;

    public void ResetWithMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = this.maxHealth;
    }

    public void AddDeathListener(IDeathListener listener)
    {
        deathListeners.Add(listener);
    }

    public void Damage(int damage)
    {
        throw new System.NotImplementedException();
    }

}
