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

