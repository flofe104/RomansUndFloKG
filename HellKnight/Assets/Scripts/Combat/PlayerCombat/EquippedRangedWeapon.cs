using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Behaviour controller of a ranged weapon
/// </summary>
public class EquippedRangedWeapon : EquippedWeapon<EquippedRangedWeapon, InventoryRangedWeapon>
{

    private void Update()
    {
    Vector3 weaponPosition = transform.position;
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector3 direction = mousePosition - weaponPosition;

    }

   
    public GameObject projectilePrefab;


    public void Attack(Func<IHealth, bool> healthDamageFilter)
    {
        this.healthDamageFilter = healthDamageFilter;
        StartAttack();
    }

    protected IEnumerator attackAnimation;

    /// <summary>
    /// when the projectile of a ranged weapon encounters a collision with an entitiy which has IHealth attached to any of its scripts
    /// this function will determine if the entity will get damage on contact
    /// </summary>
    protected Func<IHealth, bool> healthDamageFilter;

    
    protected void AnimateRangedWeaponAttack()
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
            float attackProgress = timeInAttack / weapon.AttackAnimationDuration;
            float rotateProgress = Mathf.Lerp(0, weapon.rotationAngle, attackProgress);
            ResetWeaponRotation();
            transform.Rotate(0, 0, rotateProgress, Space.Self);
        } while (timeInAttack < weapon.AttackAnimationDuration);


        ///Check if the angle of the rotation of the weapon after the rotation time matches the expected rotation 
        ///with an allowed error (due to floating point precision) of 0.001 degree
        Assert.AreApproximatelyEqual(Quaternion.Angle(Quaternion.Euler(Weapon.EquipEulerAngle), transform.localRotation), Weapon.rotationAngle, 0.001f);
        yield return new WaitForSeconds(weapon.AttackCooldownAfterAnimation);
        EndAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit " + other.gameObject.name);
        Destroy(gameObject);
        EnemyHealth health = other.gameObject.GetComponent<EnemyHealth>();
        if (health != null && healthDamageFilter(health))
        {
            OnEnemyHit(health);
        }
    }

    private void OnEnemyHit(EnemyHealth health)
    {
        health.TakeDamage(weapon.Damage);
    }

    protected void StartAttack()
    {
        if (!CanStartAttack)
            return;

        IsInAttack = true;
        AnimateRangedWeaponAttack();
    }

    protected void ResetWeaponRotation()
    {
        transform.localEulerAngles = weapon.EquipEulerAngle;
    }

    protected void OnAttackEnded()
    {
        IsInAttack = false;

        ResetWeaponRotation();

        ///Check if the weapon rotation matches its start rotation after the attack ended
        Assert.AreEqual(transform.localEulerAngles, Weapon.EquipEulerAngle);
    }

    protected void EndAttack()
    {
        attackAnimation = null;
        OnAttackEnded();
    }

}
