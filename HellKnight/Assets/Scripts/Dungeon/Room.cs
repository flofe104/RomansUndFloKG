using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

    public Room(int seed, List<EnemySciptableObject> possibleEnemies)
    {
        this.seed = seed;
        this.possibleEnemies = possibleEnemies;
        rand = new System.Random(seed);
    }

    protected List<EnemySciptableObject> possibleEnemies;

    protected HashSet<IEnemy> aliveEnemies;

    private void Generate()
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
        ///TODO: Spawn random amount of plattforms and connect overlapping ones and do max independant set. 
    }

    protected System.Random rand;

    protected int minEnemies = 3;
    protected int maxEnemies = 9;

    protected bool isCleared;

    protected virtual void GenerateEnemies()
    {
        int enemyCount = rand.Next(minEnemies,maxEnemies);

        for (int i = 0; i < enemyCount; i++)
        {
            int enemyIndex = rand.Next(possibleEnemies.Count);
            EnemySciptableObject enemy = possibleEnemies[enemyIndex];
            GameObject g = enemy.Instantiate();
        }
    }


    

    private int seed;


}
