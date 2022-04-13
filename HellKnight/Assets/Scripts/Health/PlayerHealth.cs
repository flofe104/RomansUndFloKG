using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Testing;

[TestMonoBehaviour]
public class PlayerHealth : BaseHealth
{
    public const float IMMUNE_TIME = 0.5f;

    public GameObject player;

    protected bool isImmune;

    protected override bool IsImmune => isImmune;

    protected override void OnEntityDied()
    {
        Destroy(player);
        //load the first scene of the game
        SceneManager.LoadScene(2);
    }

    protected override void OnResetHealth()
    {
        StopAllCoroutines();
        isImmune = false;
    }

    protected override void OnTakeNonLethalDamage()
    {
        isImmune = true;
        this.DoDelayed(IMMUNE_TIME, () => isImmune = false);
    }

    [Test]
    public void TestImmunity()
    {
        OnResetHealth();
        var preHealth = currentHealth;
        TakeDamage(1);
        TakeDamage(1);
        var postHealth = currentHealth;
        Assert.IsTrue(postHealth == preHealth - 1);
    }
}