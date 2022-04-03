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

    protected virtual void OnWeaponStatsAssigned(WeaponStats stats) { }

    public abstract void Attack(Func<IHealth, bool> damageFilter);

    public bool IsInAttack { get; protected set; }

    public virtual bool CanStartAttack => !IsInAttack;

}
