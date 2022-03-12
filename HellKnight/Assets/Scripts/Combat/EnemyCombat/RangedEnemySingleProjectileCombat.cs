using System.Collections;
using System.Collections.Generic;
using Testing;
using UnityEngine;

[TestMonoBehaviour]
public class RangedEnemySingleProjectileCombat : RangedEnemyBaseCombat
{

    protected override void ExecuteAttack()
    {
        Projectile p = GetProjectile();
        p.TargetPosition = player.position;
    }


    #region tests


    [Test]
    public void TestLowerCooldown()
    {
        var numObjectsBefore = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>().Length;
        Attack(); // creates two objects/projectiles
        Attack(); // creates none 
        var numObjectsAfter = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>().Length;
        Assert.AreEqual(numObjectsBefore + 2, numObjectsAfter);
    }

    [TestEnumerator]
    public IEnumerator TestUpperCooldown()
    {
        var numObjectsBefore = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>().Length;
        Attack(); // creates two objects/projectiles
        yield return new WaitForSeconds(4);
        Attack(); // creates none 
        var numObjectsAfter = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>().Length;
        Assert.AreEqual(numObjectsBefore + 4, numObjectsAfter);
    }

    [TestEnumerator]
    public IEnumerator TestHover()
    {
        var posBefore = transform.position;

        yield return new WaitForSeconds(1);

        var posAfer = transform.position;
        Assert.AreNotEqual(posBefore, posAfer);

        var distance = Vector3.Distance(posBefore, posAfer);
        Assert.GreaterOrEqual(distance, 0.5f);
        Assert.LessOrEqual(distance, 10);
    }

    #endregion


}

