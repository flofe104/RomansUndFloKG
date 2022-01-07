using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon
{

    protected const int NUMBER_OF_ROOMS = 1;

    public Dungeon(int seed)
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
            rooms[i] = new Room(rand.Next());
        }
    }

    public Room[] rooms;

    public int seed;

    


}
