using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySpearWeapon : InventoryWeapon<EquippedSpearWeapon, InventorySpearWeapon>
{

    [SerializeField]
    protected float extendDistance;

    public float ExtendDistance => extendDistance;

    [SerializeField]
    protected float length;

    public float Length => length;


}
