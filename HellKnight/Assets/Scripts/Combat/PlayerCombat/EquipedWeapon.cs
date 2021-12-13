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

    public ItemStats weaponStats;

    public bool IsInAttack { get; protected set; }

}
