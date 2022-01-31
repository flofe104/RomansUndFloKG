using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnemyCombat : MonoBehaviour
{
    public float attackCooldown = 2;
    public GameObject projectilePrefab;

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

    public void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (timeSinceAttack >= attackCooldown)
        {
            var playerPosition = player.transform.position;
            var startPosition = Vector3.MoveTowards(transform.position, playerPosition, transform.localScale.x);
            var projectile = Instantiate(projectilePrefab, startPosition, Quaternion.identity);
            projectile.GetComponent<Projectile>().TargetPosition = playerPosition;
            timeSinceAttack = 0;
        }
    }

}