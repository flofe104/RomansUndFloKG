using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damageFilter"></param>
    /// <returns>returns true if attack could be started</returns>
    bool Attack(Func<IHealth, bool> damageFilter);

    GameObject gameObject { get; }

}
