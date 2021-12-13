using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Behaviour controller of a melee weapon
/// </summary>
public class EquipedMeleeWeapon : EquipedWeapon<EquipedMeleeWeapon, InventoryMeleeWeapon>
{

    public void Attack(Func<IHealth, bool> damageColliders)
    {
        this.healthDamageFilter = damageColliders;
        StartAttack();
    }

    protected IEnumerator attackAnimation;

    /// <summary>
    /// when the weapon encounters a collision with an entitiy which has IHealth attached to any of its scripts
    /// this function will determine if the entity will get damage on contact
    /// </summary>
    protected Func<IHealth, bool> healthDamageFilter;

    protected BoxCollider weaponCollider;
    protected BoxCollider WeaponCollider
    {
        get
        {
            if(weaponCollider == null)
            {
                weaponCollider = gameObject.AddComponent<BoxCollider>();
                weaponCollider.isTrigger = true;
                weaponCollider.enabled = false;
            }
            return weaponCollider;
        }
    }

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


        ///Check if the angle of the rotation of the weapon after the rotation time matches the expected rotation 
        ///with an allowed error (due to floating point precision) of 0.001 degree
        Assert.AreApproximatelyEqual(Quaternion.Angle(Quaternion.Euler(WeaponStats.EquipEulerAngle), transform.localRotation), WeaponStats.rotationAngle, 0.001f);

        weaponCollider.enabled = false;
        yield return new WaitForSeconds(weaponStats.AttackCooldownAfterAnimation);
        EndAttack();

        ///Check if the weapon ends its rotation where it started
        Assert.AreEqual(transform.localEulerAngles, WeaponStats.EquipEulerAngle);
    }

    protected void ApplyDamageCollider()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit[] attackHits = Physics.SphereCastAll(ray, weaponStats.range, 0.1f);

        foreach (RaycastHit hit in attackHits)
        {
            EvaluateHitCollider(hit.collider);
        }
    }

    protected void EvaluateHitCollider(Collider collider)
    {
        IHealth health = collider.GetComponent<IHealth>();
        if (health != null && healthDamageFilter(health))
        {
            health.Damage(weaponStats.Damage);
        }
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

    protected override void OnWeaponStatsAssigned(InventoryMeleeWeapon stats)
    {
        CreateWeaponCollider();
    }

    public void CreateWeaponCollider()
    {
        WeaponCollider.size = new Vector3(0.5f,weaponStats.range, 0.5f);
        WeaponCollider.center = new Vector3(0, weaponStats.range / 2, 0);
    }

    protected void OnAttackEnded()
    {
        IsInAttack = false;
    }

    protected void EndAttack()
    {
        attackAnimation = null;
        ResetWeaponRotation();
        OnAttackEnded();
    }

}
