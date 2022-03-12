using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer),typeof(MeshCollider), typeof(MeshFilter))]
public class Dungeon : MonoBehaviour
{

    protected const int NUMBER_OF_ROOMS = 5;

    private void Start()
    {
        InitializeDungeon(seed);   
    }

   

    public void InitializeDungeon(int seed)
    {
        this.seed = seed;
        rand = new System.Random(seed);
        enemies = possibleEnemies.Select(e => e as ISpawnableEnemy).ToList();

        Generate();
    }

  

    private void Generate()
    {
        rooms = new Room[NUMBER_OF_ROOMS];
        Vector2 offset = new Vector2();
        BuildWall(ref offset);
        for (int i = 0; i < NUMBER_OF_ROOMS; i++)
        {
            AddRoom(i, ref offset);
            if(i + 1 < NUMBER_OF_ROOMS)
            {
                AddConnector(ref offset);
            }
        }
        BuildWall(ref offset);
        DisplayDungeon();
    }

    private void AddConnector(ref Vector2 offset)
    {
        CreateNewDungeonObject<RoomConnector>(ref offset);
    }

    private void BuildWall(ref Vector2 offset)
    {
        CreateNewDungeonObject<DungeonWall>(ref offset);
    }

    private void AddRoom(int index, ref Vector2 offset)
    {
        CreateNewDungeonObject<Room>(ref offset, (r) => HandleNewRoom(r, index));
    }

    protected T CreateNewDungeonObject<T>(ref Vector2 offset, Action<T> Initialize = null) where T : DungeonPart
    {
        GameObject dungeonObject = new GameObject();
        dungeonObject.name = typeof(T).Name;
        dungeonObject.transform.parent = transform;
        dungeonObject.transform.localPosition = offset;
        T part = dungeonObject.AddComponent<T>();
        Initialize?.Invoke(part);
        AddDungeonPartToMesh(part, ref offset);
        return part;
    }

    protected void HandleNewRoom(Room r, int index)
    {
        rooms[index] = r;
        r.Initialize(rand.Next(), enemies);
    }

    protected void AddDungeonPartToMesh(DungeonPart part, ref Vector2 offset)
    {
        part.AddDungeonPartToMeshLayout(tiling, vertices, triangles, colliderVertices, colliderTriangles, uvCoords);
        MoveOffset(ref offset, part);
    }

    protected void MoveOffset(ref Vector2 offset, DungeonPart dungeonPart)
    {
        offset += new Vector2(dungeonPart.Width, dungeonPart.ExitHeight);
    }

    protected virtual void DisplayDungeon()
    {
        Mesh m = new Mesh();
        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        m.uv = uvCoords.ToArray();
        filter.mesh = m;

        BuildCollider();
    }

    protected void BuildCollider()
    {
        Mesh m = new Mesh();
        m.vertices = colliderVertices.ToArray();
        m.triangles = colliderTriangles.ToArray();
        meshCollider.sharedMesh = m;
    }

    protected System.Random rand;

    public Room[] rooms;

    public List<EnemySciptableObject> possibleEnemies;
    public List<ISpawnableEnemy> enemies;


    protected List<Vector3> vertices = new List<Vector3>();
    protected List<int> triangles = new List<int>();
    protected List<Vector2> uvCoords = new List<Vector2>();

    protected List<Vector3> colliderVertices = new List<Vector3>();
    protected List<int> colliderTriangles = new List<int>();

    [SerializeField]
    protected MeshFilter filter;

    [SerializeField]
    protected MeshRenderer meshRenderer;

    [SerializeField]
    protected MeshCollider meshCollider;

    public float tiling = 1;

    public AnimationCurve plattformWidth;

    public AnimationCurve plattformHeight;

    public int seed;


}
