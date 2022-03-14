using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyCombat : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit: "+other.gameObject.name);
        PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
        if (health != null && other)
        {
            OnHealthHit(health);
        }
    }

    private void OnHealthHit(BaseHealth health)
    {
        health.TakeDamage(15);
    }
}