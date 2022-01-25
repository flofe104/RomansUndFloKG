using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    public GameObject enemy;

    public void OnDeath()
    {
        Destroy(enemy);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            HealDamage(10);
        }

        if (currentHealth < 1)
        {
            OnDeath();
        }
    }
}
