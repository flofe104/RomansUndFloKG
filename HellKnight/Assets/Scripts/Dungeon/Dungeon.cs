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

    public const int NUMBER_OF_ROOMS = 5;
    protected const int NUMBER_OF_ROOM_CONNECTORS = NUMBER_OF_ROOMS - 1;
    public GameObject heartPrefab;
    public GameObject dungeonDoorPrefab;
    public Material platformMaterial;

    private void Start()
    {
        InitializeDungeon(UnityEngine.Random.Range(0,9999999));
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
        Room.SetPlatformMaterial(platformMaterial);
        Room.SetHeartPrefab(heartPrefab);

        rooms = new Room[NUMBER_OF_ROOMS];
        connectors = new RoomConnector[NUMBER_OF_ROOM_CONNECTORS];
        Vector2 offset = new Vector2();
        BuildWall(ref offset);
        for (int i = 0; i < NUMBER_OF_ROOMS; i++)
        {
            AddRoom(i, ref offset);
            if (i + 1 < NUMBER_OF_ROOMS)
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
        c.Initialize(dungeonDoorPrefab);
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
        r.Initialize(rand.Next(), enemies, GetAt(connectors, index - 1), index);
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
    protected void TestDoorInitialStates()
    {
        Assert.IsTrue(!rooms[0].ExitDoor.IsOpen);

        for (int i = 1; i < NUMBER_OF_ROOM_CONNECTORS-1; i++)
        {
            Assert.IsTrue(rooms[i].EntryDoor.IsOpen);
            Assert.IsTrue(!rooms[i].ExitDoor.IsOpen);
        }
        Assert.IsTrue(rooms[NUMBER_OF_ROOM_CONNECTORS - 1].EntryDoor.IsOpen);

    }

    [TestEnumerator]
    protected IEnumerator TestExitOpensWhenRoomIsCleared()
    {
        yield return new WaitForFixedUpdate();
        while (rooms[0].HasAliveEnemies)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Assert.IsTrue(rooms[0].ExitDoor.IsOpen);
    }

  

    [TestEnumerator]
    protected IEnumerator TestEntryClosesOnRoomEnter()
    {
        yield return new WaitForFixedUpdate();
        while (!rooms[1].EnemiesSpawned)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Assert.IsTrue(!rooms[1].EntryDoor.IsOpen);
    }

    [Test]
    protected void TestRoomConnectorsNotNull()
    {
        Assert.IsTrue(rooms[0].PreviousConnector == null && rooms[0].NextConnector != null);
        for (int i = 1; i < NUMBER_OF_ROOM_CONNECTORS; i++)
        {
            Assert.IsTrue(rooms[i].NextConnector != null && rooms[i].PreviousConnector != null);
        }
        Assert.IsTrue(rooms[NUMBER_OF_ROOM_CONNECTORS].PreviousConnector != null && rooms[NUMBER_OF_ROOM_CONNECTORS].NextConnector == null);
    }

    [Test]
    protected void TestAdjacentRoomsShareConnector()
    {       
        for (int i = 0; i < NUMBER_OF_ROOM_CONNECTORS; i++)
        {
            Assert.IsTrue(rooms[i].NextConnector == rooms[i + 1].PreviousConnector);
        }
    }

    [Test]
    public void TestRoomSizes()
    {
        IsTrueForForAllIterations(RoomHasMinSize, 500);
    }

    protected bool RoomHasMinSize(Room r)
    {
        return r.Width >= 20 && r.Height >= 20;
    }

    [Test]
    protected void TestPlayerStartPosition()
    {
        Vector3 playerStartPos = GameObject.Find("Player").transform.position;
        float distanceFromFirstRoomAnchor = Vector3.Distance(playerStartPos, rooms[0].DungenPartGlobalAnchorPosition);
        Assert.ApproxEqual(distanceFromFirstRoomAnchor, 0, 3);
    }

    [Test]
    protected void DontSpawnEnemiesTooEarly()
    {
        for (int i = 1; i < NUMBER_OF_ROOMS; i++)
        {
            Assert.IsTrue(!rooms[i].HasAliveEnemies);
        }
    }

    [TestEnumerator]
    protected IEnumerator FirstRoomHasEnemies()
    {
        yield return new WaitForFixedUpdate();

        Assert.IsTrue(rooms[0].HasAliveEnemies);
    }

    [Test]
    public void TestNumberOfRooms()
    {
        Assert.AreEqual(rooms.Length, NUMBER_OF_ROOMS);
    }

    [Test]
    public void TestConnectorHeight()
    {
        for (int i = 0; i < NUMBER_OF_ROOM_CONNECTORS; i++)
        {
            Assert.Equals(connectors[i].Height, 2.5f);
            Assert.Equals(connectors[i].Width, 20);
        }
    }

    [Test]
    public void TestRandomness()
    {
        Vector3[][] vertices = new Vector3[5][];
        Vector3[][] colliderVerts = new Vector3[5][];
        for (int i = 0; i < 5; ++i)
        {
            Dungeon d = TestPipeline.CreateNewInstanceOf<Dungeon>();
            d.dungeonDoorPrefab = dungeonDoorPrefab;
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
        d.dungeonDoorPrefab = dungeonDoorPrefab;
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

    protected bool IsTrueForForAllIterations(Func<Room, bool> p, int iterations)
    {
        bool result = true;

        System.Random rand = new System.Random();
        for (int i = 0; i < iterations && result; i++)
        {
            Room r = TestPipeline.CreateNewInstanceOf<Room>();
            r.Initialize(new System.Random().Next(), enemies, null,0);
            result = p(r);
        }
        return result;
    }

    #endregion

}
