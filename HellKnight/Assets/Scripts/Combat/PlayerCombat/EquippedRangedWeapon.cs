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
    public Transform shotPoint;
    public float force = 10;
    public float xArrowScale = (float)0.95578;
    public float yArrowScale = (float)0.01943;
    public float zArrowScale = (float)0.00069;

    private void Update()
    {
        Vector3 weaponPosition = transform.position;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.LookAt(worldPosition);
        transform.Rotate(0, -90, 0);

    }

    void InstantiateArrow()
    {
        Debug.Log("Pfeil spawnt");
        GameObject newArrow = Instantiate(projectilePrefab, shotPoint.position, shotPoint.rotation);
        newArrow.transform.localScale = new Vector3(xArrowScale, yArrowScale, zArrowScale);
        Rigidbody r = newArrow.GetComponent<Rigidbody>();
        r.isKinematic = false;
        r.velocity = transform.right * force;
    }

    protected float attackCooldown = 1;


    protected override void ExecuteAttack()
    {
        InstantiateArrow();
    }

    protected override void OnStartAttack()
    {
        this.DoDelayed(attackCooldown, () => IsInAttack = false);
    }

}