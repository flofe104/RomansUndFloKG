using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : BaseHealth
{

    protected override void OnEntityDied()
    {
        Destroy(gameObject);
    }

}
