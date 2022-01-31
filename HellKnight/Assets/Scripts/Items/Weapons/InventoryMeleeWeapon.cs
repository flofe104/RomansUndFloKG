using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMeleeWeapon : InventoryWeapon<EquippedMeleeWeapon, InventoryMeleeWeapon>
{

    [Range(0, 2)]
    public float range;

    [Range(35, 360)]
    public float rotationAngle;

}
