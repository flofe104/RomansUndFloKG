using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

    public Room(int seed)
    {
        this.seed = seed;
        rand = new System.Random(seed);
    }

    protected HashSet<IEnemy> aliveEnemies;

    private void Generate()
    {
        GenerateInterior(rand.Next());
        if (!isCleared) 
        {
            GenerateEnemies(rand.Next());
            isCleared = aliveEnemies.Count == 0;
        }
    }

    protected virtual void GenerateInterior(int seed)
    {

    }

    protected System.Random rand;

    protected bool isCleared;

    protected virtual void GenerateEnemies(int seed)
    {

    }

    private int seed;


}
