using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class for all inventory weapons
/// </summary>
/// <typeparam name="WeaponBehaviour">Generic type of the behaviour that will controll weapons of this type</typeparam>
/// <typeparam name="ItemStats">Generic type of the Itemstats the weaponbehaviour will use. Choose always the type that inherits from this class</typeparam>
public abstract class InventoryWeapon<WeaponBehaviour, ItemStats> : InstantiableInventoryItem, IInstantiatableWeapon<WeaponBehaviour,ItemStats>
    where WeaponBehaviour : EquipedWeapon<WeaponBehaviour,ItemStats> 
    where ItemStats : InventoryWeapon<WeaponBehaviour,ItemStats>
{
    [Tooltip("Damage the weapon deals on hit")]
    [Range(1, 100)]
    public int damage;

    [Tooltip("Time in seconds before the next attack can be started")]
    [Range(0, 2)]
    public float attackDuration;

    public WeaponBehaviour CreateInstanceAndGetBehaviour(Transform parent)
    {
        GameObject instance = CreateInstance(parent);
        return instance.GetComponent<WeaponBehaviour>();
    }

    protected override void OnInstantiate(GameObject instance)
    {
        WeaponBehaviour weapon = instance.AddComponent<WeaponBehaviour>();
        weapon.weaponStats = (ItemStats)this;
    }

}
