using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EquippedSwingWeapon : EquippedMeleeWeapon<EquippedSwingWeapon, InventoryMeleeWeapon>
{


    protected override void OnWeaponStatsAssigned(InventoryMeleeWeapon stats)
    {
        ApplyWeaponStatsToCollider();
    }

    public void ApplyWeaponStatsToCollider()
    {
        WeaponCollider.size = new Vector3(0.5f, weapon.range, 0.5f);
        WeaponCollider.center = new Vector3(0, weapon.range / 2, 0);
    }

    protected override void AnimateWeaponInAttackProgress(float attackProgress)
    {
        float rotateProgress = Mathf.Lerp(0, weapon.rotationAngle, attackProgress);
        transform.Rotate(0, 0, rotateProgress, Space.Self);
    }

}
