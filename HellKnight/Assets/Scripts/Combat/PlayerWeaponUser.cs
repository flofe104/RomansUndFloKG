using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for any entity that wants to use a weapon
/// </summary>
/// <typeparam name="WeaponBehaviour">The behaviour to controll the weapon with</typeparam>
/// <typeparam name="WeaponType">the weapontype to control</typeparam>
public class PlayerWeaponUser : MonoBehaviour 
{

    /// <summary>
    /// the behaviour to controll the weapon
    /// </summary>
    protected IWeapon weaponBehaviour;

    /// <summary>
    /// instance of the currently equiped item
    /// </summary>
    protected GameObject equipedWeaponInstance;

    public bool HasWeaponEquipped => equipedWeaponInstance != null && weaponBehaviour != null;

    public const string ATTACK_BUTTON_NAME = "PrimaryAttack";

    private void Update()
    {
        if (Input.GetButton(ATTACK_BUTTON_NAME))
        {
            weaponBehaviour.Attack(DamageEnemiesFilter);
        }
    }

    protected bool DamageEnemiesFilter(IHealth health)
    {
        return !(health is PlayerHealth);
    }

    public void EquipWeapon(BaseInventoryWeapon weapon)
    {
        DestroyEquipedWeapon();
        weaponBehaviour = weapon.EquipWeapon(transform);
        equipedWeaponInstance = weaponBehaviour.gameObject;
    }

    public void DestroyEquipedWeapon()
    {
        if (equipedWeaponInstance != null)
        {
            Destroy(equipedWeaponInstance);
            weaponBehaviour = null;
            equipedWeaponInstance = null;
        }
    }
}
