using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipedWeapon<WeaponBehaviour, ItemStats> : MonoBehaviour 
    where ItemStats : InventoryWeapon<WeaponBehaviour, ItemStats> where WeaponBehaviour : EquipedWeapon<WeaponBehaviour, ItemStats>
{

    public ItemStats weaponStats;

    public bool IsInAttack { get; protected set; }

    protected abstract void Attack();

    protected void StartAttack()
    {
        IsInAttack = true;
        this.DoDelayed(weaponStats.attackDuration, EndAttack);
    }

    protected void EndAttack()
    {
        IsInAttack = false;
    }

}
