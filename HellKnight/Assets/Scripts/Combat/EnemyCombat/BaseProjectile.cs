using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
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
