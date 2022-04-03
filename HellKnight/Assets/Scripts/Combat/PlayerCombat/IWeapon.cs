using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Attack(Func<IHealth, bool> damageFilter);

    GameObject gameObject { get; }

}
