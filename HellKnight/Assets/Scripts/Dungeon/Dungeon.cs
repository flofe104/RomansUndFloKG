using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Testing;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer),typeof(MeshCollider), typeof(MeshFilter))]
[TestMonoBehaviour(CallStartBeforeTesting = true)]
public class Dungeon : MonoBehaviour
{

    protected const int NUMBER_OF_ROOMS = 5;
    protected const int NUMBER_OF_ROOM_CONNECTORS = NUMBER_OF_ROOMS - 1;


    private void Start()
    {
        InitializeDungeon(seed);
    }

    public void InitializeDungeon(int seed)
    {
        if (rand != null)
            return;

        if (DisplayDungeonAction == null)
        {
            DisplayDungeonAction = DisplayDungeon;
        }
        this.seed = seed;
        rand = new System.Random(seed);
        enemies = possibleEnemies.Select(e => e as ISpawnableEnemy).ToList();

        Generate();
    }

  

    private void Generate()
    {
        rooms = new Room[NUMBER_OF_ROOMS];
        connectors = new RoomConnector[NUMBER_OF_ROOM_CONNECTORS];
        Vector2 offset = new Vector2();
        BuildWall(ref offset);
        for (int i = 0; i < NUMBER_OF_ROOMS; i++)
        {
            AddRoom(i, ref offset);
            if(i + 1 < NUMBER_OF_ROOMS)
            {
                AddConnector(i,ref offset);
            }
        }

        BuildWall(ref offset);
        DisplayDungeonAction();
    }

    private void AddConnector(int index, ref Vector2 offset)
    {
        CreateNewDungeonObject<RoomConnector>(ref offset, (c)=> HandleConnector(c, index));
    }

    private void BuildWall(ref Vector2 offset)
    {
        CreateNewDungeonObject<DungeonWall>(ref offset);
    }

    private void AddRoom(int index, ref Vector2 offset)
    {
        CreateNewDungeonObject<Room>(ref offset, (r) => HandleNewRoom(r, index));
    }

    protected void HandleConnector(RoomConnector c, int index)
    {
        connectors[index] = c;
        if (TryGetAt(rooms, index, out Room r))
            r.SetNextConnector(c);
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

    protected T GetAt<T>(T[] array, int index) where T : DungeonPart
    {
        if (index < 0 || index >= array.Length)
            return null;
        return array[index];
    }

    protected bool TryGetAt<T>(T[] array, int index, out T result) where T : DungeonPart
    {
        if (index < 0 || index >= array.Length)
            result = null;
        else
            result = array[index];
        return result != null;
    }

    protected void HandleNewRoom(Room r, int index)
    {
        rooms[index] = r;
        r.Initialize(rand.Next(), enemies, GetAt(connectors, index - 1));
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

    protected void DisplayDungeon()
    {
        Mesh m = new Mesh();
        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        m.uv = uvCoords.ToArray();
        filter.mesh = m;

        BuildCollider();
    }

    protected Action DisplayDungeonAction;


    protected void BuildCollider()
    {
        Mesh m = new Mesh();
        m.vertices = colliderVertices.ToArray();
        m.triangles = colliderTriangles.ToArray();
        meshCollider.sharedMesh = m;
    }

    protected System.Random rand;

    public Room[] rooms;
    public RoomConnector[] connectors;



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


    #region Tests

    [Test]
    public void TestNumberOfRooms()
    {
        Assert.AreEqual(rooms.Length, NUMBER_OF_ROOMS);
    }

    [Test]
    public void TestRandomness()
    {
        Vector3[][] vertices = new Vector3[5][];
        Vector3[][] colliderVerts = new Vector3[5][];
        for (int i = 0; i < 5; ++i)
        {
            Dungeon d = TestPipeline.CreateNewInstanceOf<Dungeon>();
            d.possibleEnemies = possibleEnemies;
            d.DisplayDungeonAction = delegate
            {
                vertices[i] = d.vertices.ToArray();
                colliderVerts[i] = d.colliderVertices.ToArray();
            };
            d.InitializeDungeon(i);
        }

        for (int i = 0; i < 5; ++i)
        {
            for (int j = i + 1; j < 5; ++j)
            {
                Assert.AreNotEqual(vertices[i], vertices[j]);
                Assert.AreNotEqual(colliderVerts[i], colliderVerts[j]);
            }
        }
    }


    //[TestEnumerator]
    //public IEnumerator TestTest()
    //{
    //    int i = 10;
    //    while(i-- > 0)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        Assert.AreNotEqual(-1, i);
    //    }
    //}

    [Test]
    public void TestNormals()
    {
        Dungeon d = TestPipeline.CreateNewInstanceOf<Dungeon>();
        d.possibleEnemies = possibleEnemies;
        List<int> triangles = null;
        List<Vector3> vertices = null;
        d.DisplayDungeonAction = delegate
        {
            triangles = d.triangles;
            vertices = d.vertices;
        };
        d.InitializeDungeon(0);
        for (int i = 0; i < triangles.Count; i += 3)
        {
            var v0 = vertices[triangles[i]];
            var v1 = vertices[triangles[i + 1]];
            var v2 = vertices[triangles[i + 2]];
            var normal = (Vector3.Cross(v1 - v0, v2 - v0)).normalized;

            Assert.AreEqual(Vector3.Angle(normal, -Vector3.forward), 0);
        }
    }


    #endregion

}
