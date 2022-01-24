using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : DungeonPart, IDeathListener
{

    public void Initialize(int seed, List<ISpawnableEnemy> possibleEnemies)
    {
        this.seed = seed;
        this.possibleEnemies = possibleEnemies;
        rand = new System.Random(seed);
    }

    protected override void OnAddedRoomPartToMesh(float tiling, List<Vector3> vertices, List<int> triangles, List<Vector3> colliderVerts, List<int> colliderTris, List<Vector2> materialCoordinates)
    {
        Vector3 collisionOrigin = transform.position + new Vector3(0, 0, COLLISION_WIDTH);
        ///left wall collider
        AddSquareToMesh(collisionOrigin + new Vector3(0, entryHeight, 0), new Vector3(0, MAX_HEIGHT - transform.position.y - entryHeight, 0), new Vector3(0, 0, -2 * COLLISION_WIDTH), tiling, colliderVerts, colliderTris, null);
        ///right wall collider
        AddSquareToMesh(collisionOrigin + new Vector3(dungeonPartSize.x, entryHeight, 0), new Vector3(0, 0, -2 * COLLISION_WIDTH), new Vector3(0, MAX_HEIGHT - transform.position.y - entryHeight, 0), tiling, colliderVerts, colliderTris, null);
    }

    protected override void DetermineDungeonPartSize()
    {
        dungeonPartSize = GetRoomSize();
    }


    protected virtual Vector2Int GetRoomSize()
    {
        Vector2Int result = new Vector2Int();
        result.x = rand.Next(MIN_WIDTH, MAX_WIDTH);
        result.y = rand.Next(MIN_HEIGHT, MAX_HEIGHT);
        return result;
    }

    private void BuildRoomLayout()
    {

    }

    protected List<RoomPlattform> plattforms;

    protected List<ISpawnableEnemy> possibleEnemies;

    protected HashSet<IHealth> aliveEnemies;

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


    protected const int MIN_ENEMIES = 2;
    protected const int MAX_ENEMIES = 5;

    protected const int MIN_WIDTH = 10;
    protected const int MAX_WIDTH = 40;

    protected bool isCleared;

    private int seed;

    protected virtual void GenerateEnemies()
    {
        int enemyCount = rand.Next(MIN_ENEMIES,MAX_ENEMIES);
        aliveEnemies = new HashSet<IHealth>();

        for (int i = 0; i < enemyCount; i++)
        {
            float x = Mathf.Lerp(0, dungeonPartSize.x, (float)rand.NextDouble());
            float y = Mathf.Lerp(0, dungeonPartSize.y, (float)rand.NextDouble());
            Vector3 position = new Vector3(x, y, 0);

            int enemyIndex = rand.Next(possibleEnemies.Count);
            ISpawnableEnemy enemy = possibleEnemies[enemyIndex];
            GameObject g = enemy.Spawn(position);
            IHealth health = g.GetComponent<IHealth>();
            health.AddDeathListener(this);
            aliveEnemies.Add(health);
        }
    }

    public void OnDeath(IHealth died)
    {
        aliveEnemies.Remove(died);
    }
}
