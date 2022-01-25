using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnableEnemy
{

    GameObject Spawn(Vector3 position, Transform parent = null);

}
