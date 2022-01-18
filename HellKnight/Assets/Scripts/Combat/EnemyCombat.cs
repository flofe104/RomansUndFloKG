using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        if (timeSinceAttack >= attackCooldown)
        {
            StartCoroutine(FireProjectile(player.transform.position));
            timeSinceAttack = 0;
        }        
    }

    IEnumerator FireProjectile(Vector3 target)
    {
        var projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);

        // Check if the position of the cube and sphere are approximately equal.
        while (Vector3.Distance(projectile.transform.position, target) > 0.05f)
        {
            // Move our position a step closer to the target.
            float step = projectileSpeed * Time.deltaTime; // calculate distance to move
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, target, step);
            yield return null;
        }
        Destroy(projectile);
    }

    private void Attack()
    {

    }

    public float attackCooldown = 2;
    public float projectileSpeed = 10;
    public GameObject player;
    public GameObject projectilePrefab;

    private Movement enemyMovement;
    private Vector3 playerPosition;
    private float timeSinceAttack = 0;
}
