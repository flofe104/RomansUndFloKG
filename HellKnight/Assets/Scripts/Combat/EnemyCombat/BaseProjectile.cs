using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public Vector3 targetPosition = Vector3.zero;
    public float projectileSpeed = 10;

    private Vector3 targetDirection = Vector3.zero;
    private float aliveTime = 0;

    private const float MAX_ALIVE_TIME = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit " + other.gameObject.name);
        Destroy(gameObject);
        if (other.gameObject.name == "Player")
        {
            OnPlayerHit();
        }
    }

    private void OnPlayerHit()
    {
        throw new System.NotImplementedException();
    }

    private void Move()
    {
        // set movement direction once
        if(targetDirection == Vector3.zero && targetPosition != Vector3.zero)
        {
            targetDirection = (targetPosition - transform.position).normalized * projectileSpeed;
        }

        if(targetDirection != Vector3.zero)
        {
            transform.position += targetDirection * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        aliveTime += Time.deltaTime;
        if (aliveTime > MAX_ALIVE_TIME)
            Destroy(gameObject);

        Move();
    }
}
