using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Testing;

public class PlayerHealth : BaseHealth
{
    public const float IMMUNE_TIME = 0.5f;

    public GameObject player;

    protected override void OnEntityDied()
    {
        Destroy(player);
        //load the first scene of the game
        SceneManager.LoadScene(2);
    }

    void Update()
    {
        timeSinceDamage += Time.deltaTime;
        if(timeSinceDamage < IMMUNE_TIME)
        {
            isImmune = true;
        }
        else
        {
            isImmune = false;
        }

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
           TakeDamage(10);
        //}
        */
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealDamage(100);
        }

    }

    [Test]
    public void TestImmunity()
    {
        var preHealth = currentHealth;
        TakeDamage(1);
        TakeDamage(1);
        var postHealth = currentHealth;
        Assert.IsTrue(postHealth == preHealth - 1);
    }
}