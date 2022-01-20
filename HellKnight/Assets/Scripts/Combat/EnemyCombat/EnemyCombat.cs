using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float attackCooldown = 2;
    public GameObject projectilePrefab;

    private Movement enemyMovement;
    private float timeSinceAttack = 0;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
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
            projectile.GetComponent<BaseProjectile>().TargetPosition = player.transform.position;
            timeSinceAttack = 0;
        }
    }

}