using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEnemyCombat : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: "+other.gameObject.name);
        BaseHealth health = other.gameObject.GetComponent<BaseHealth>();
        if (health != null && other != gameObject.GetComponent<Collider>())
        {
            OnHealthHit(health);
        }
    }

    private void OnHealthHit(BaseHealth health)
    {
        health.TakeDamage(15);
    }
}