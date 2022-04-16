using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour class to controll any inventory weapon
/// </summary>
/// <typeparam name="WeaponBehaviour">The type that controlls the inventory weapon. Choose always the type that inherits from this class</typeparam>
/// <typeparam name="WeaponStats"></typeparam>
public abstract class EquippedWeapon<WeaponBehaviour, WeaponStats> : MonoBehaviour, IWeapon
    where WeaponStats : InventoryWeapon<WeaponBehaviour, WeaponStats> 
    where WeaponBehaviour : EquippedWeapon<WeaponBehaviour, WeaponStats>
{

    protected WeaponStats weapon;

    public WeaponStats Weapon
    {
        get
        {
            return weapon; 
        }
        set
        {
            weapon = value;
            OnWeaponStatsAssigned(weapon);
        }
    }


    /// <summary>
    /// when the weapon encounters a collision with an entitiy which has IHealth attached to any of its scripts
    /// this function will determine if the entity will get damage on contact
    /// </summary>
    protected Func<IHealth, bool> healthDamageFilter;

    protected virtual void OnWeaponStatsAssigned(WeaponStats stats) { }

    public bool Attack(Func<IHealth, bool> damageFilter)
    {
        if (!CanStartAttack)
            return false;

        healthDamageFilter = damageFilter;
        IsInAttack = true;

        OnStartAttack();
        ExecuteAttack();
        return true;
    }

    protected virtual void OnStartAttack() { }

    protected abstract void ExecuteAttack();

    public bool IsInAttack { get; protected set; }

    public virtual bool CanStartAttack => !IsInAttack;

}
