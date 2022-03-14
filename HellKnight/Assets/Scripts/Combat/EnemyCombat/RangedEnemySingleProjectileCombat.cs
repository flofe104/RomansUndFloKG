using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemySingleProjectileCombat : RangedEnemyBaseCombat
{

    protected override void ExecuteAttack()
    {
        Projectile p = GetProjectile();
        p.TargetPosition = player.position;
    }

}

