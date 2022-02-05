using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{

    protected Vector3 targetPosition;

    public Vector3 TargetPosition
    {
        set
        {
            targetPosition = value;
            targetDirection = (targetPosition - transform.position).normalized * projectileSpeed; 
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
        BaseHealth health = other.gameObject.GetComponent<BaseHealth>();
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
