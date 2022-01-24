using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float attackCooldown = 2;
    public float hoverSpeed = 0.8f;
    public float hoverDistance = 0.8f;
    public AnimationCurve hoverPatternX;
    public AnimationCurve hoverPatternY;
    public GameObject projectilePrefab;

    private float timeSinceAttack = 0;
    private float t = 0;
    private GameObject player;
    private Vector3 center;

    private void Start()
    {
        player = GameObject.Find("Player");
        center = transform.position;
    }
    void Update()
    {
        Attack();
        Hover();
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

    private void Hover()
    {
        var pos = transform.position;
        t += 1 / hoverSpeed * Time.deltaTime;
        var x = hoverPatternX.Evaluate(t);
        var y = hoverPatternY.Evaluate(t);
        transform.position = center + hoverDistance * new Vector3(x, y);
    }
}