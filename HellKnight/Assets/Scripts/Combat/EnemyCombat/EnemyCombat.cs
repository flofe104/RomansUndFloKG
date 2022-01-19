using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float attackCooldown = 2;
    public GameObject projectilePrefab;
    public GameObject player;

    private Movement enemyMovement;
    private float timeSinceAttack = 0;

    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (timeSinceAttack >= attackCooldown)
        {
            var projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
            projectile.GetComponent<BaseProjectile>().targetPosition = player.transform.position;
            timeSinceAttack = 0;
        }
    }

}