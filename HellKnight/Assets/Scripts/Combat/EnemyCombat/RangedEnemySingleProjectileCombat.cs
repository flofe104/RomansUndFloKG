using System.Collections;
using System.Collections.Generic;
using Testing;
using UnityEngine;

[TestMonoBehaviour]
public class RangedEnemySingleProjectileCombat : RangedEnemyBaseCombat
{
    public static int PROJECTILE_DAMAGE = 5;

    protected override void ExecuteAttack()
    {
        p = GetProjectile();
        p.TargetPosition = player.position;
        p.ProjectileDamage = PROJECTILE_DAMAGE;
    }

    public static string prefabForTestName = "TestRangeEnemyPrefab";


    #region tests
    [TestEnumerator]
    public IEnumerator TestSpeed()
    {
        if (p == null) //wait for projectile to spawn
            yield return new WaitForSeconds(1f);

        var positionBefore = p.transform.position;
        yield return new WaitForSeconds(1f);
        var positionAfter = p.transform.position;

        var distanceTravelled = Vector3.Distance(positionBefore, positionAfter);
        Assert.ApproxEqual(distanceTravelled, Projectile.PROJECTILE_SPEED, 0.5f);
    }

    [TestEnumerator]
    public IEnumerator TestCooldown()
    {
        float before = timeSinceAttack;
        yield return new WaitForSeconds(ATTACK_COOLDOWN);
        float after = timeSinceAttack;
        Assert.ApproxEqual(after % ATTACK_COOLDOWN, before);
    }

    #endregion


}

