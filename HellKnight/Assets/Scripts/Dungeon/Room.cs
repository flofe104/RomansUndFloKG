using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

    public Room(int seed)
    {
        this.seed = seed;
    }

    HashSet<IEnemy> aliveEnemies;
Platforms : List<Platform>


    protected virtual void Generate()
    {

    }

    private int seed;


}
