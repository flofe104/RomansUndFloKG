using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Testing;

[TestMonoBehaviour]
public class RangeEnemySplitProjectileCombat : RangedEnemyBaseCombat
{
    public const float FIRE_ANGLE = 15f;
    public static int PROJECTILE_DAMAGE = 10;

    protected override void ExecuteAttack()
    {
        AimAtAngle(GetProjectile(), FIRE_ANGLE);
        AimAtAngle(GetProjectile(), -FIRE_ANGLE);
    }

    protected void AimAtAngle(Projectile p, float angle)
    {
        Vector2 dir = new Vector2(player.position.x, player.position.y) - new Vector2(transform.position.x, transform.position.y);
        dir.Normalize();
        dir = dir.Rotate(angle);
        p.TargetDirection = new Vector3(dir.x, dir.y, 0);
        p.ProjectileDamage = PROJECTILE_DAMAGE;
    }

    #region Tests

    [Test]
    public void TestAttack()
    {

    }

    #endregion
}
