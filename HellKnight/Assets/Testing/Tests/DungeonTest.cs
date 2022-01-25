using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DungeonTest : Dungeon
{

    protected void InitializeEnemyList()
    {
        UnityEngine.Object o = Resources.Load("TestEnemy");
        possibleEnemies = new List<EnemySciptableObject>() { o as EnemySciptableObject };
    }

    [Test]
    public void TestNumberOfRooms()
    {
        InitializeEnemyList();
        int seed = 0;
        InitializeDungeon(seed);

        Assert.AreEqual(rooms.Length, NUMBER_OF_ROOMS);
    }

    [Test]
    public void TestRandomness()
    {
        InitializeEnemyList();
        Vector3[][] vertices = new Vector3[5][];
        Vector3[][] colliderVerts = new Vector3[5][];
        for (int i = 0; i < 5; ++i)
        {
            DisplayDungeonAction = delegate 
            { 
                vertices[i] = base.vertices.ToArray(); 
                colliderVerts[i] = colliderVertices.ToArray(); 
            };
            InitializeDungeon(i);
        }

        for (int i = 0; i < 5; ++i)
        {
            for(int j = i + 1; j < 5; ++j)
            {
                Assert.AreNotEqual(vertices[i], vertices[j]);
                Assert.AreNotEqual(colliderVerts[i], colliderVerts[j]);
            }
        }
    }

    protected override void DisplayDungeon()
    {
        DisplayDungeonAction?.Invoke();
    }

    protected Action DisplayDungeonAction;

    [Test]
    public void TestNormals()
    {
        InitializeEnemyList();
        List<int> triangles = null;
        List<Vector3> vertices = null;
        DisplayDungeonAction = delegate 
        { 
            triangles = base.triangles; 
            vertices = base.vertices; 
        };
        InitializeDungeon(0);
        for (int i=0; i<triangles.Count; i+=3)
        {
            var v0 = vertices[triangles[i]];
            var v1 = vertices[triangles[i+1]];
            var v2 = vertices[triangles[i+2]];
            var normal = (Vector3.Cross(v1 - v0, v2 - v0)).normalized;

            Assert.AreEqual(Vector3.Angle(normal, -Vector3.forward), 0);
        }
    }
}
