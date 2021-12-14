using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for any entity that wants to use a weapon
/// </summary>
/// <typeparam name="WeaponBehaviour">The behaviour to controll the weapon with</typeparam>
/// <typeparam name="WeaponType">the weapontype to control</typeparam>
public abstract class WeaponUser<WeaponBehaviour, WeaponType> : MonoBehaviour 
    where WeaponBehaviour : EquippedWeapon<WeaponBehaviour, WeaponType> 
    where WeaponType : InventoryWeapon<WeaponBehaviour, WeaponType>
{

    [Tooltip("Weapon to equip when the object is spawned")]
    [SerializeField]
    protected InventoryWeapon<WeaponBehaviour, WeaponType> startWeapon;

    /// <summary>
    /// the behaviour to controll the weapon
    /// </summary>
    protected WeaponBehaviour weaponBehaviour;

    /// <summary>
    /// instance of the currently equiped item
    /// </summary>
    protected GameObject equipedWeaponInstance;

    private void Start()
    {
        if (startWeapon != null)
        {
            EquipWeapon(startWeapon);
        }
    }

    protected void EquipWeapon(InventoryWeapon<WeaponBehaviour, WeaponType> weapon)
    {
        DestroyEquipedWeapon();
        weaponBehaviour = weapon.CreateInstanceAndGetBehaviour(transform);
        equipedWeaponInstance = weaponBehaviour.gameObject;
    }

    protected void DestroyEquipedWeapon()
    {
        if (equipedWeaponInstance != null)
        {
            Destroy(equipedWeaponInstance);
            weaponBehaviour = null;
            equipedWeaponInstance = null;
        }
    }
}
