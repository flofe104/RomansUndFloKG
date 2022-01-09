using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dungeon : MonoBehaviour
{

    protected const int NUMBER_OF_ROOMS = 1;

    private void Start()
    {
        InitializeDungeon(seed);   
    }

    public void InitializeDungeon(int seed)
    {
        this.seed = seed;
        Generate();
    }

    private void Generate()
    {
        System.Random rand = new System.Random(seed);
        rooms = new Room[NUMBER_OF_ROOMS];
        for (int i = 0; i < NUMBER_OF_ROOMS; i++)
        {
            rooms[i] = new Room(rand.Next(), possibleEnemies.Select(e => e as ISpawnableEnemy).ToList());
            rooms[i].Generate();
        }
    }

    
    public Room[] rooms;

    public List<EnemySciptableObject> possibleEnemies;

    public int seed;

    


}
