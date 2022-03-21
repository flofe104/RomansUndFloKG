using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : DungeonPart, IDeathListener
{

    public void Initialize(int seed, List<ISpawnableEnemy> possibleEnemies, RoomConnector previous)
    {
        this.seed = seed;
        this.possibleEnemies = possibleEnemies;
        this.previousConnector = previous;
        rand = new System.Random(seed);
        CreateEnemySpawnColliderForRoom();
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

    protected RoomConnector previousConnector;

    protected RoomConnector nextConnector;

    protected List<RoomPlattform> plattforms;

    protected List<ISpawnableEnemy> possibleEnemies;

    public bool EnemiesSpawned => aliveEnemies != null;

    protected HashSet<IHealth> aliveEnemies;

    public bool HasAliveEnemies => aliveEnemies.Count > 0;

    public bool IsRoomCleared => !HasAliveEnemies;

    protected BoxCollider c;

    public void SetNextConnector(RoomConnector connector)
    {
        nextConnector = connector;
    }

    protected void CreateEnemySpawnColliderForRoom()
    {
        if (!isCleared)
        {
            c = gameObject.AddComponent<BoxCollider>();
            c.isTrigger = true;
            c.size = DungeonPartSizeWithDefaultWidth3D;
            c.center = DungeonPartCenterPosition;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            GenerateEnemies();
            EntryDoor.Close();
            Destroy(c);
        }
    }

    protected override void OnAddedRoomPartToMesh(float tiling, List<Vector3> vertices, List<int> triangles, List<Vector3> colliderVerts, List<int> colliderTris, List<Vector2> materialCoordinates)
    {
        Vector3 collisionOrigin = transform.position + new Vector3(0, 0, COLLISION_WIDTH);
        ///left wall collider
        AddSquareToMesh(collisionOrigin + new Vector3(0, entryHeight, 0), new Vector3(0, MAX_DUNGEON_HEIGHT - transform.position.y - entryHeight, 0), new Vector3(0, 0, -2 * COLLISION_WIDTH), tiling, colliderVerts, colliderTris, null);
        ///right wall collider
        Vector3 rightWallBottom = collisionOrigin + new Vector3(dungeonPartSize.x, 0, 0);
        AddSquareToMesh(rightWallBottom, new Vector3(0, 0, -2 * COLLISION_WIDTH), new Vector3(0, ExitHeight, 0), tiling, colliderVerts, colliderTris, null);
    }

    protected override void DetermineDungeonPartSize(out Vector2 dungeonPartSize)
    {
        if (base.dungeonPartSize != default)
        {
            dungeonPartSize = base.dungeonPartSize;
        }
        else
        {
            Vector2 result = new Vector2();
            result.x = rand.Next(MIN_WIDTH, MAX_WIDTH);
            result.y = rand.Next(MIN_HEIGHT, MAX_HEIGHT);
            GenerateInterior(result);
            result = AdjustRoomSizeToPlattforms(result);
            dungeonPartSize = result;
        }
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
        if (aliveEnemies != null)
            return;

        int enemyCountMean = Mathf.FloorToInt(DungeonPartSize.x / 10f);
        int enemyCountMax = Mathf.FloorToInt(enemyCountMean / 2);
        int enemyCountMin = Mathf.FloorToInt(enemyCountMean * 2);
        int enemyCount = rand.Next(enemyCountMean - ENEMY_SPREAD, enemyCountMean + ENEMY_SPREAD);
        if(enemyCount > enemyCountMax) enemyCount = enemyCountMax;
        if(enemyCount < enemyCountMin) enemyCount = enemyCountMin;
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
        if (IsRoomCleared)
            OnRoomCleared();
    }

    protected void OnRoomCleared()
    {
        OpenDoors();
    }

    public RoomConnector PreviousConnector => previousConnector;
    public RoomConnector NextConnector => nextConnector;

    public void OpenDoors()
    {
        ExitDoor?.Open();
    }


    public void CloseDoors()
    {
        EntryDoor?.Close();
        ExitDoor?.Close();
    }

    public RoomDoors EntryDoor => previousConnector?.exitDoor;
    public RoomDoors ExitDoor => nextConnector?.entryDoor;


}
