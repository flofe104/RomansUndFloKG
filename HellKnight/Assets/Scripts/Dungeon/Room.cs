using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

    public Room(int seed, List<ISpawnableEnemy> possibleEnemies)
    {
        this.seed = seed;
        this.possibleEnemies = possibleEnemies;
        rand = new System.Random(seed);
    }

    protected virtual Vector2Int GetRoomSize()
    {
        Vector2Int result = new Vector2Int();
        result.x = rand.Next(MIN_WIDTH, MAX_WIDTH);
        result.y = rand.Next(MIN_WIDTH, MAX_WIDTH);
        return result;
    }

    private void GenerateRoomLayout()
    {
        roomSize = GetRoomSize();
        BuildRoomLayout();
    }

    private void BuildRoomLayout()
    {

    }

    protected List<ISpawnableEnemy> possibleEnemies;

    protected HashSet<IEnemy> aliveEnemies;

    public void Generate()
    {
        GenerateRoomLayout();
        GenerateInterior();
        if (!isCleared) 
        {
            GenerateEnemies();
            isCleared = aliveEnemies.Count == 0;
        }
    }

    protected virtual void GenerateInterior()
    {
        ///TODO: Spawn random amount of plattforms and connect overlapping ones and do max independant set. 
    }

    protected System.Random rand;

    protected Vector2Int roomSize;

    protected const int MIN_ENEMIES = 3;
    protected const int MAX_ENEMIES = 9;

    protected const int MIN_WIDTH = 10;
    protected const int MAX_WIDTH = 20;

    protected const float entranceHeight = 2;
    protected const float entranceWidth = 0.5f;
    protected const float entranceWayLength = 1.5f;

    protected bool isCleared;

    protected virtual void GenerateEnemies()
    {
        int enemyCount = rand.Next(MIN_ENEMIES,MAX_ENEMIES);
        aliveEnemies = new HashSet<IEnemy>();

        for (int i = 0; i < enemyCount; i++)
        {
            int enemyIndex = rand.Next(possibleEnemies.Count);
            ISpawnableEnemy enemy = possibleEnemies[enemyIndex];
            GameObject g = enemy.Spawn();
        }
    }


    private int seed;


}
