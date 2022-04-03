using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventoryWeapon : InstantiableInventoryItem
{

    public abstract IWeapon EquipWeapon(Transform parent);

}
