using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedMeleeWeapon : EquipedWeapon<EquipedMeleeWeapon, InventoryMeleeWeapon>
{

    public void Attack()
    {
        StartAttack();
    }

    protected IEnumerator attackAnimation;

    protected void AnimateMeleeWeaponAttack()
    {
        attackAnimation = RotateWeapon();
        StartCoroutine(attackAnimation);
    }

    protected IEnumerator RotateWeapon()
    {
        float timeInAttack = 0;
        do
        {
            yield return null;
            timeInAttack += Time.deltaTime;
            float attackProgress = timeInAttack / weaponStats.AttackAnimationDuration;
            float rotateProgress = Mathf.Lerp(0, weaponStats.rotationAngle, attackProgress);
            ResetWeaponRotation();
            transform.Rotate(0, 0, rotateProgress, Space.Self);
        } while (timeInAttack < weaponStats.AttackAnimationDuration);
        yield return new WaitForSeconds(weaponStats.AttackCooldownAfterAnimation);
        EndAttack();
    }

    protected void StartAttack()
    {
        if (!CanStartAttack)
            return;

        IsInAttack = true;
        AnimateMeleeWeaponAttack();
    }

    protected void ResetWeaponRotation()
    {
        transform.localEulerAngles = weaponStats.EquipEulerAngle;
    }

    protected void OnAttackEnded()
    {

    }

    protected void EndAttack()
    {
        IsInAttack = false;
        attackAnimation = null;
        ResetWeaponRotation();
        OnAttackEnded();
    }

}
