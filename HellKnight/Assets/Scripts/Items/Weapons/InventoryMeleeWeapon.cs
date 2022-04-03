using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMeleeWeapon : InventoryWeapon<EquippedSwingWeapon, InventoryMeleeWeapon>
{

    [Range(0, 4)]
    public float range;

    [Range(35, 360)]
    public float rotationAngle;

}
