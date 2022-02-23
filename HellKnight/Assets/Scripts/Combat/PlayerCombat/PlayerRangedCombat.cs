using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedCombat : RangedWeaponUser
{

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

}
