using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class InventoryWeapon<WeaponBehaviour, ItemStats> : InstantiableInventoryItem 
    where WeaponBehaviour : EquipedWeapon<WeaponBehaviour,ItemStats> 
    where ItemStats : InventoryWeapon<WeaponBehaviour,ItemStats>
{
    [Tooltip("Damage the weapon deals on hit")]
    [Range(1, 100)]
    public int damage;

    [Tooltip("Time in seconds before the next attack can be started")]
    [Range(0, 2)]
    public float attackDuration;

    protected override void OnInstantiate(GameObject instance)
    {
        WeaponBehaviour weapon = instance.AddComponent<WeaponBehaviour>();
        weapon.weaponStats = (ItemStats)this;
    }

}
