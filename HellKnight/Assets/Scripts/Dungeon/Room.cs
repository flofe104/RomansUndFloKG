using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : DungeonPart
{

    public void Initialize(int seed, List<ISpawnableEnemy> possibleEnemies)
    {
        this.seed = seed;
        this.possibleEnemies = possibleEnemies;
        rand = new System.Random(seed);
    }


    protected override void DetermineDungeonPartSize()
    {
        dungeonPartSize = GetRoomSize();
    }


    protected virtual Vector2Int GetRoomSize()
    {
        Vector2Int result = new Vector2Int();
        result.x = rand.Next(MIN_WIDTH, MAX_WIDTH);
        result.y = rand.Next(MIN_WIDTH, MAX_WIDTH);
        return result;
    }

    private void BuildRoomLayout()
    {

    }

    protected List<RoomPlattform> plattforms;

    protected List<ISpawnableEnemy> possibleEnemies;

    protected HashSet<IEnemy> aliveEnemies;

    public void Generate()
    {
        GenerateInterior();
        if (!isCleared) 
        {
            GenerateEnemies();
            isCleared = aliveEnemies.Count == 0;
        }
    }

    protected virtual void GenerateInterior()
    {
        plattforms = PlattformGenerator.GeneratePlattformsInSpace(dungeonPartSize, rand);
        plattforms.ForEach(p => p.Create());
    }

    protected System.Random rand;


    protected const int MIN_ENEMIES = 3;
    protected const int MAX_ENEMIES = 9;

    protected const int MIN_WIDTH = 10;
    protected const int MAX_WIDTH = 20;

    protected bool isCleared;

    private int seed;

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


}
