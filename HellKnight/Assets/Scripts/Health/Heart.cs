using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit by" + other.gameObject.name);
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.HealDamage(33);
        }
        Destroy(gameObject);
    }

}
