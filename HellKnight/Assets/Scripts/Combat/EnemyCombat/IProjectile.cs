using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    public abstract void OnHealthHit(BaseHealth health);
    public abstract void Move();
}
