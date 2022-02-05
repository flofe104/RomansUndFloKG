using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RangedEnemyTest : RangedEnemySingleProjectileCombat
{


    [Test]
    public void TestLowerCooldown()
    {
        var numObjectsBefore = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>().Length;
        Attack(); // creates two objects/projectiles
        Attack(); // creates none 
        var numObjectsAfter = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>().Length;
        Assert.AreEqual(numObjectsBefore + 2, numObjectsAfter);
    }
    [Test]
    public void TestUpperCooldown()
    {
        var numObjectsBefore = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>().Length;
        Attack(); // creates two objects/projectiles
        StartCoroutine(Waiter(4f));
        Attack(); // creates none 
        var numObjectsAfter = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>().Length;
        Assert.AreEqual(numObjectsBefore + 4, numObjectsAfter);
    }
    IEnumerator Waiter(float time)
    {
        yield return new WaitForSeconds(time);
    }
    [Test]
    public void TestHover()
    {
        var posBefore = transform.position;
        Waiter(1f);
        var posAfer = transform.position;
        Assert.AreNotEqual(posBefore, posAfer);

        var distance = Vector3.Distance(posBefore, posAfer);
        Assert.GreaterOrEqual(distance, 0.5);
        Assert.LessOrEqual(distance, 10);
    }
}
