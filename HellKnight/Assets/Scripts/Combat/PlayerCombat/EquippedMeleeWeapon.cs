using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Behaviour controller of a melee weapon
/// </summary>
public abstract class EquippedMeleeWeapon<WeaponBehaviour, WeaponStats> : EquippedWeapon<WeaponBehaviour, WeaponStats>
    where WeaponBehaviour : EquippedMeleeWeapon<WeaponBehaviour, WeaponStats>
    where WeaponStats : InventoryWeapon<WeaponBehaviour, WeaponStats>
{

    public override void Attack(Func<IHealth, bool> healthDamageFilter)
    {
        this.healthDamageFilter = healthDamageFilter;
        StartAttack();
    }

    protected IEnumerator attackAnimation;

    /// <summary>
    /// when the weapon encounters a collision with an entitiy which has IHealth attached to any of its scripts
    /// this function will determine if the entity will get damage on contact
    /// </summary>
    protected Func<IHealth, bool> healthDamageFilter;

    protected BoxCollider weaponCollider;

    protected Rigidbody body;

    protected BoxCollider WeaponCollider
    {
        get
        {
            if(weaponCollider == null)
            {
                weaponCollider = gameObject.AddComponent<BoxCollider>();
                weaponCollider.isTrigger = true;
                weaponCollider.enabled = false;
                CreateRigidBodyIfNotExising();
            }
            return weaponCollider;
        }
    }

    protected void CreateRigidBodyIfNotExising()
    {
        if (body == null)
        {
            body = gameObject.AddComponent<Rigidbody>();
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            body.useGravity = false;
        }
    }

    protected void AnimateMeleeWeaponAttack()
    {
        attackAnimation = WeaponAttackAnimation();
        StartCoroutine(attackAnimation);
    }


    protected IEnumerator WeaponAttackAnimation()
    {
        float timeInAttack = 0;
        weaponCollider.enabled = true;
        do
        {
            yield return null;
            timeInAttack += Time.deltaTime;
            float attackProgress = timeInAttack / weapon.AttackAnimationDuration;
            ResetWeaponTransform();
            AnimateWeaponInAttackProgress(attackProgress);
        } while (timeInAttack < weapon.AttackAnimationDuration);


        weaponCollider.enabled = false;
        yield return new WaitForSeconds(weapon.AttackCooldownAfterAnimation);
        EndAttack();
    }

    protected abstract void AnimateWeaponInAttackProgress(float progress);

    private void OnTriggerEnter(Collider other)
    {
        EvaluateHitCollider(other);
    }

    protected void EvaluateHitCollider(Collider collider)
    {
        IHealth health = collider.GetComponent<IHealth>();
        if (health != null && healthDamageFilter(health))
        {
            health.TakeDamage(weapon.Damage);
        }
    }

    protected void StartAttack()
    {
        if (!CanStartAttack)
            return;

        IsInAttack = true;
        OnStartAttack();
        AnimateMeleeWeaponAttack();
    }

    protected virtual void OnStartAttack() { }
    protected virtual void AfterAttackEnded() { }

    protected void ResetWeaponTransform()
    {
        transform.localEulerAngles = weapon.EquipEulerAngle;
        transform.localPosition = weapon.EquipPosition;
        transform.localScale = weapon.EquipScale;
    }

    protected void OnAttackEnded()
    {
        IsInAttack = false;
        weaponCollider.enabled = false;

        ResetWeaponTransform();

        AfterAttackEnded();

        ///Check if the weapon rotation matches its start rotation after the attack ended
        Assert.AreEqual(transform.localEulerAngles, Weapon.EquipEulerAngle);
        Assert.AreEqual(transform.localPosition, Weapon.EquipPosition);
        Assert.AreEqual(transform.localScale, Weapon.EquipScale);
    }

    protected void EndAttack()
    {
        attackAnimation = null;
        OnAttackEnded();
    }

}
