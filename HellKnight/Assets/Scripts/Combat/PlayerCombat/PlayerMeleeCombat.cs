using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeCombat : MeleeWeaponCombat
{

    public const string ATTACK_BUTTON_NAME = "PrimaryAttack";

    private void Update()
    {
        if (Input.GetButton(ATTACK_BUTTON_NAME))
        {
            weaponBehaviour.Attack();
        }
    }

}
