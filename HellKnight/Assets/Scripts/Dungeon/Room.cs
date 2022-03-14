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

    protected System.Random rand;


    protected const int ENEMY_SPREAD = 2;
    protected const int MIN_WIDTH = 30;
    protected const int MAX_WIDTH = 60;

    protected const int MIN_HEIGHT = 20;
    protected const int MAX_HEIGHT = 40;

    protected bool isCleared;

    private int seed;


    public override float ExitHeight => exitHeight;

    protected float exitHeight;


    protected float entryHeight = 2.5f;


    protected List<RoomPlattform> plattforms;

    protected List<ISpawnableEnemy> possibleEnemies;

    protected HashSet<IHealth> aliveEnemies;


    protected override void OnAddedRoomPartToMesh(float tiling, List<Vector3> vertices, List<int> triangles, List<Vector3> colliderVerts, List<int> colliderTris, List<Vector2> materialCoordinates)
    {
        Vector3 collisionOrigin = transform.position + new Vector3(0, 0, COLLISION_WIDTH);
        ///left wall collider
        AddSquareToMesh(collisionOrigin + new Vector3(0, entryHeight, 0), new Vector3(0, MAX_DUNGEON_HEIGHT - transform.position.y - entryHeight, 0), new Vector3(0, 0, -2 * COLLISION_WIDTH), tiling, colliderVerts, colliderTris, null);
        ///right wall collider
        Vector3 rightWallBottom = collisionOrigin + new Vector3(dungeonPartSize.x, 0, 0);
        AddSquareToMesh(rightWallBottom, new Vector3(0, 0, -2 * COLLISION_WIDTH), new Vector3(0, ExitHeight, 0), tiling, colliderVerts, colliderTris, null);
    }

    protected override Vector2 DetermineDungeonPartSize()
    {
       return GetRoomSize();
    }


    protected virtual Vector2 GetRoomSize()
    {
        Vector2 result = new Vector2();
        result.x = rand.Next(MIN_WIDTH, MAX_WIDTH);
        result.y = rand.Next(MIN_HEIGHT, MAX_HEIGHT);
        GenerateInterior(result);
        result = AdjustRoomSizeToPlattforms(result);
        return result;
    }

    protected void SetExitHeight()
    {
        exitHeight = plattforms[plattforms.Count - 1].TopLeft.y;
    }

    protected Vector2 AdjustRoomSizeToPlattforms(Vector2 roomSize)
    {
        RoomPlattform last = plattforms[plattforms.Count - 1];
        roomSize.x = last.BottomRight.x;
        roomSize.y = last.TopLeft.y + RoomConnector.CONNECTOR_HEIGHT;
        return roomSize;
    }

    public void Generate()
    {
        if (!isCleared) 
        {
            GenerateEnemies();
            isCleared = aliveEnemies.Count == 0;
        }
    }

    protected virtual void GenerateInterior()
    {
        GenerateInterior(dungeonPartSize);
    }

    protected virtual void GenerateInterior(Vector2 forSpace)
    {
        plattforms = PlattformGenerator.GeneratePlattformsInSpace(forSpace, rand);
        SetExitHeight();
        plattforms.ForEach(p => p.Create(transform));
    }

    protected virtual void GenerateEnemies()
    {
        var roomSize = GetRoomSize();
        int enemyCountMean = Mathf.FloorToInt(roomSize.x / 10f);
        int enemyCount = rand.Next(enemyCountMean - ENEMY_SPREAD, enemyCountMean + ENEMY_SPREAD);
        aliveEnemies = new HashSet<IHealth>();

        for (int i = 0; i < enemyCount; i++)
        {
            float x = Mathf.Lerp(0, dungeonPartSize.x, (float)rand.NextDouble());
            float y = Mathf.Lerp(0, dungeonPartSize.y, (float)rand.NextDouble());
            Vector3 position = new Vector3(x, y, 0);

            int enemyIndex = rand.Next(possibleEnemies.Count);
            ISpawnableEnemy enemy = possibleEnemies[enemyIndex];
            GameObject g = enemy.Spawn(position, transform);
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
