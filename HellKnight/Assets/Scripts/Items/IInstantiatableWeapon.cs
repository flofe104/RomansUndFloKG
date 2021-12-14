using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for all equipable weapons
/// </summary>
/// <typeparam name="WeaponBehaviour">The behaviour to controll the weapon with</typeparam>
/// <typeparam name="WeaponType">the weapontype to control</typeparam>
public interface IInstantiatableWeapon<WeaponBehaviour, WeaponType> : IInstantiatableItem
    where WeaponBehaviour : EquippedWeapon<WeaponBehaviour, WeaponType>
    where WeaponType : InventoryWeapon<WeaponBehaviour, WeaponType>
{

    WeaponBehaviour CreateInstanceAndGetBehaviour(Transform parent);

}
