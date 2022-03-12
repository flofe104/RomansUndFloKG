using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemySplitProjectileCombat : RangedEnemyBaseCombat
{

    public float aimAngleNextToPlayer;

    protected override void ExecuteAttack()
    {
        AimAtAngle(GetProjectile(), aimAngleNextToPlayer);
        AimAtAngle(GetProjectile(), -aimAngleNextToPlayer);
    }

    protected void AimAtAngle(Projectile p, float angle)
    {
        Vector2 dir = new Vector2(player.position.x, player.position.y) - new Vector2(transform.position.x, transform.position.y);
        dir.Normalize();
        dir = dir.Rotate(angle);
        p.TargetDirection = new Vector3(dir.x, dir.y, 0);
    }



}
