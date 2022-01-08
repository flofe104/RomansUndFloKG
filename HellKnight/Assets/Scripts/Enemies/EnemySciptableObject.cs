using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySciptableObject : ScriptableObject
{

    [SerializeField]
    protected int health;
    public int Health { get { return health; } }

    [SerializeField]
    protected GameObject prefab;
    public GameObject Prefab { get { return prefab; } }

    [SerializeField]
    protected int damage;
    public int Damage { get { return damage; } }

    public GameObject Instantiate(Transform parent = null)
    {
        GameObject gameObject = GameObject.Instantiate(prefab);
        BaseHealth health = gameObject.GetComponent<BaseHealth>();
        health.ResetWithMaxHealth(Health);
        return gameObject;
    }

}
