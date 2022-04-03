using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class for all inventory weapons
/// </summary>
/// <typeparam name="WeaponBehaviour">Generic type of the behaviour that will controll weapons of this type</typeparam>
/// <typeparam name="WeaponStats">Generic type of the Itemstats the weaponbehaviour will use. Choose always the type that inherits from this class</typeparam>
public abstract class InventoryWeapon<WeaponBehaviour, WeaponStats> : BaseInventoryWeapon, IInstantiatableWeapon<WeaponBehaviour,WeaponStats>
    where WeaponBehaviour : EquippedWeapon<WeaponBehaviour,WeaponStats> 
    where WeaponStats : InventoryWeapon<WeaponBehaviour,WeaponStats>
{
    [Tooltip("Damage the weapon deals on hit")]
    [Range(1, 100)]
    [SerializeField]
    protected int damage;
    public int Damage => damage;

    [Tooltip("Time in seconds for the attack animation")]
    [Range(0, 2)]
    [SerializeField]
    protected float attackAnimationDuration;
    public float AttackAnimationDuration => attackAnimationDuration;

    [Tooltip("Time in seconds after animation finished, before next attack can begin")]
    [Range(0, 2)]
    [SerializeField]
    protected float attackCooldownAfterAnimation;
    public float AttackCooldownAfterAnimation => attackCooldownAfterAnimation;

    public float TotalAttackTime => attackAnimationDuration + attackCooldownAfterAnimation;

    public WeaponBehaviour CreateInstanceAndGetBehaviour(Transform parent)
    {
        GameObject instance = CreateInstance(parent);
        return instance.GetComponent<WeaponBehaviour>();
    }

    protected override void OnInstantiate(GameObject instance)
    {
        AddWeaponBehaviour(instance);
    }

    protected WeaponBehaviour AddWeaponBehaviour(GameObject instance)
    {
        WeaponBehaviour weapon = instance.AddComponent<WeaponBehaviour>();
        weapon.Weapon = (WeaponStats)this;
        return weapon;
    }

    public override IWeapon EquipWeapon(Transform parent)
    {
        return CreateInstanceAndGetBehaviour(parent);
    }

}
