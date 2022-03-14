using System.Collections;
using System.Collections.Generic;
using Testing;
using UnityEngine;

public abstract class RangedEnemyBaseCombat : MonoBehaviour
{
    public const float ATTACK_COOLDOWN = 2;
    public GameObject projectilePrefab;

    protected float timeSinceAttack = 0;
    protected Transform player;


    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        Attack();
    }

    protected Projectile GetProjectile()
    {
        Vector3 playerDirection = (player.position - transform.position).normalized;
        Vector3 startPosition = transform.position + Vector3.Scale(transform.localScale, playerDirection);

        GameObject projectileObject = Instantiate(projectilePrefab, startPosition, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        return projectile;
    }

    protected void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (timeSinceAttack >= ATTACK_COOLDOWN)
        {
            ExecuteAttack();
            timeSinceAttack = 0;
        }
    }

    protected abstract void ExecuteAttack();


}