using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedSpearWeapon : EquippedMeleeWeapon<EquippedSpearWeapon, InventorySpearWeapon>
{


    protected override void OnWeaponStatsAssigned(InventorySpearWeapon stats)
    {
        ApplyWeaponStatsToCollider();
    }

    public void ApplyWeaponStatsToCollider()
    {
        WeaponCollider.size = new Vector3(0.5f, 0.5f, weapon.Length);
        WeaponCollider.center = new Vector3(0, 0, weapon.Length / 2);
    }


    private void Update()
    {
        UpdateSpearOrientation();
    }

    protected void UpdateSpearOrientation()
    {
        Vector3 weaponPosition = transform.position;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.LookAt(worldPosition);
    }

    protected Vector3 attackStartEuler;

    protected override void OnStartAttack()
    {
        attackStartEuler = transform.localEulerAngles;
        enabled = false;
    }

    protected override void AfterAttackEnded()
    {
        UpdateSpearOrientation();
        enabled = true;
    }

    protected override void AnimateWeaponInAttackProgress(float progress)
    {
        transform.localEulerAngles = attackStartEuler;
        transform.position += transform.forward * weapon.ExtendDistance * progress;
    }
}
