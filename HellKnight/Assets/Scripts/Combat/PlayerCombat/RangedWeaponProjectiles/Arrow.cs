using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {  
        correctFlight();
    }

    public void correctFlight()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit " + collision.gameObject.name);
        Destroy(gameObject);
        BaseHealth health = collision.gameObject.GetComponent<BaseHealth>();
        if (health != null && !(health is PlayerHealth))
        {
            OnEnemyHit(health);
        }
    }

    private void OnEnemyHit(BaseHealth health)
    {
        health.TakeDamage(25);
    }
}
