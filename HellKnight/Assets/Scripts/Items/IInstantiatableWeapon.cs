using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for all equipable weapons
/// </summary>
/// <typeparam name="WeaponBehaviour">The behaviour to controll the weapon with</typeparam>
/// <typeparam name="WeaponStats">the weapontype to control</typeparam>
public interface IInstantiatableWeapon<WeaponBehaviour, WeaponStats> : IInstantiatableItem
    where WeaponBehaviour : EquippedWeapon<WeaponBehaviour, WeaponStats>
    where WeaponStats : InventoryWeapon<WeaponBehaviour, WeaponStats>
{

    WeaponBehaviour CreateInstanceAndGetBehaviour(Transform parent);

}
