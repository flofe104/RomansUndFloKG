using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMeleeWeapon : InventoryWeapon<EquipedMeleeWeapon, InventoryMeleeWeapon>
{

    [Range(0, 2)]
    public float range;

    [Range(45, 180)]
    public float rotationAngle;

}
