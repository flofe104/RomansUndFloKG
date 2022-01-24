using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    
    public void TakeDamage(int damage);

    void HealDamage(int damage);

    void AddDeathListener(IDeathListener listener);

}
