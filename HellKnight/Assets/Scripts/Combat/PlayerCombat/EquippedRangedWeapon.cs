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
    public GameObject projectilePrefab;
    public float force;
    public Transform shotPoint;

    private void Update()
    {
    Vector3 weaponPosition = transform.position;
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector3 direction = mousePosition - weaponPosition;
    transform.right = direction;

    }

    void Shoot()
    {
        GameObject newArrow = Instantiate(projectilePrefab, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<Rigidbody>().velocity = transform.right * force;
    }
   
    public void Attack(Func<IHealth, bool> healthDamageFilter)
    {
        this.healthDamageFilter = healthDamageFilter;
        Shoot();
    }

    protected IEnumerator attackAnimation;

    /// <summary>
    /// when the projectile of a ranged weapon encounters a collision with an entitiy which has IHealth attached to any of its scripts
    /// this function will determine if the entity will get damage on contact
    /// </summary>
    protected Func<IHealth, bool> healthDamageFilter;

    
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

}
