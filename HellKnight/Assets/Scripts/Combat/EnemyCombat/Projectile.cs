using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{

    public Vector3 TargetPosition
    {
        set
        {
            targetDirection = (value - transform.position).normalized * projectileSpeed; 
        }
    }

    public Vector3 TargetDirection
    {
        set
        {
            targetDirection = value.normalized * projectileSpeed;
        }
    }

    public float projectileSpeed = 10;

    protected Vector3 targetDirection = Vector3.zero;
    protected float aliveTime = 0;

    protected const float MAX_ALIVE_TIME = 5.0f;

    protected void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit " + other.gameObject.name);
        Destroy(gameObject);
        PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
        if (health != null)
        {
            OnHealthHit(health);
        }
    }

    public void OnHealthHit(BaseHealth health)
    {
        health.TakeDamage(15);
    }

    public void Move()
    {
        transform.position += targetDirection * Time.deltaTime;
    }

    void FixedUpdate()
    {
        aliveTime += Time.deltaTime;
        if (aliveTime > MAX_ALIVE_TIME)
            Destroy(gameObject);

        Move();
    }
}
