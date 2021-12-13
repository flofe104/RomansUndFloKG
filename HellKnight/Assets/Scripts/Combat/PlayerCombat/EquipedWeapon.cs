using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour class to controll any inventory weapon
/// </summary>
/// <typeparam name="WeaponBehaviour">The type that controlls the inventory weapon. Choose always the type that inherits from this class</typeparam>
/// <typeparam name="ItemStats"></typeparam>
public abstract class EquipedWeapon<WeaponBehaviour, ItemStats> : MonoBehaviour 
    where ItemStats : InventoryWeapon<WeaponBehaviour, ItemStats> 
    where WeaponBehaviour : EquipedWeapon<WeaponBehaviour, ItemStats>
{

    protected ItemStats weaponStats;

    public ItemStats WeaponStats
    {
        get
        {
            return weaponStats; 
        }
        set
        {
            weaponStats = value;
            OnWeaponStatsAssigned(weaponStats);
        }
    }

    protected virtual void OnWeaponStatsAssigned(ItemStats stats) { }

    public bool IsInAttack { get; protected set; }

    public virtual bool CanStartAttack => !IsInAttack;

}
