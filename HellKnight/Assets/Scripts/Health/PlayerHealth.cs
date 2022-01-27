using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : BaseHealth
{

    public GameObject player;

    public void OnDeath()
    {
       Destroy(player);
       //load the first scene of the game
       SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            HealDamage(10);
        }

        if(currentHealth < 1)
        {
            OnDeath();
        }
    }
}