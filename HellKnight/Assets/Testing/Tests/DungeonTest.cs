using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DungeonTest : Dungeon
{
    
    [Test]
    public void TestNumberOfRooms()
    {
        int seed = 0;
        InitializeDungeon(seed);

        Assert.AreEqual(rooms.Length, NUMBER_OF_ROOMS);
    }

    [Test]
    public void TestRandomness()
    {
        MeshFilter[] filters = new MeshFilter[5];
        MeshCollider[] meshColliders = new MeshCollider[5];
        for (int i = 0; i < 5; ++i)
        {
            InitializeDungeon(i);
            filters[i] = filter;
            meshColliders[i] = meshCollider;
        }

        for (int i = 0; i < 5; ++i)
        {
            for(int j = 0; j < 5; ++j)
            {
                Assert.AreNotEqual(filters[i], filters[j]);
                Assert.AreNotEqual(meshColliders[i], meshColliders[j]);
            }
        }
    }

    [Test]
    public void TestNormals()
    {
        var cameraPosition = Camera.main.transform.forward;

        for(int i=0; i<triangles.Count; i+=3)
        {
            var v0 = vertices[triangles[i]];
            var v1 = vertices[triangles[i+1]];
            var v2 = vertices[triangles[i+2]];
            var normal = (Vector3.Cross(v1 - v0, v2 - v0)).normalized;

            Assert.LessOrEqual(Vector3.Angle(normal, cameraPosition), 180);
        }
    }
}
