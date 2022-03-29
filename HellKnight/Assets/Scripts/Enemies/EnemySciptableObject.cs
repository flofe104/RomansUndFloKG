using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySciptableObject : ScriptableObject, ISpawnableEnemy
{

    [SerializeField]
    protected int health;
    public int Health { get { return health; } }

    [SerializeField]
    protected GameObject prefab;
    public GameObject Prefab { get { return prefab; } }

    [SerializeField]
    protected bool spawnOnGround = false;
    
    public bool SpawnOnGround => spawnOnGround;


    [SerializeField]
    protected int damage;
    public int Damage { get { return damage; } }

    public int ChallengeRating => health / 10 + 2 * damage;
        
    public GameObject Spawn(Vector3 position, Transform parent)
    {
        if(spawnOnGround)
        {
            position.y = 1.14f;
        }
        GameObject gameObject = Instantiate(prefab, position, Prefab.transform.rotation);
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = position;
        BaseHealth health = gameObject.GetComponent<BaseHealth>();
        health.ResetWithMaxHealth(Health);
        return gameObject;
    }
}
