using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DungeonPart : MonoBehaviour
{

    protected const float MAX_HEIGHT = 50;
    protected const float MIN_HEIGHT = -50;
    protected const float COLLISION_WIDTH = 1;

    protected Vector2Int dungeonPartSize;

    protected float entryHeightOffset = 0;
    protected float entryHeight = 2.5f;


    protected abstract void DetermineDungeonPartSize();

    public void AddRoomLayoutToMeshData(float tiling, List<Vector3> vertices, List<int> triangles, List<Vector3> colliderVerts, List<int> colliderTris, List<Vector2> materialCoordinates)
    {
        DetermineDungeonPartSize();
        float startHeight = transform.position.y + dungeonPartSize.y;
        float distanceToLowestHeight = transform.position.y - MIN_HEIGHT;

        ///build rendered mesh
        AddSquareToMesh(transform.position + new Vector3(0, dungeonPartSize.y, 0), new Vector3(dungeonPartSize.x,0,0),new Vector3(0, MAX_HEIGHT - startHeight,0), tiling, vertices, triangles, materialCoordinates);
        AddSquareToMesh(transform.position - new Vector3(0, distanceToLowestHeight, 0), new Vector3(dungeonPartSize.x,0,0), new Vector3(0,distanceToLowestHeight,0), tiling, vertices, triangles, materialCoordinates);

        ///build collision mesh
        Vector3 collisionOrigin = transform.position + new Vector3(0, 0, COLLISION_WIDTH);
        ///floor collider
        AddSquareToMesh(collisionOrigin, new Vector3(0, 0, - 2 * COLLISION_WIDTH), new Vector3(dungeonPartSize.x, 0, 0), tiling, colliderVerts, colliderTris, null);
        ///ceiling collider
        AddSquareToMesh(collisionOrigin + new Vector3(0, dungeonPartSize.y,0), new Vector3(dungeonPartSize.x, 0, 0), new Vector3(0, 0, -2 * COLLISION_WIDTH), tiling, colliderVerts, colliderTris, null);

        OnAddedRoomPartToMesh(tiling,vertices,triangles,colliderVerts,colliderTris,materialCoordinates);
    }

    protected virtual void OnAddedRoomPartToMesh(float tiling, List<Vector3> vertices, List<int> triangles, List<Vector3> colliderVerts, List<int> colliderTris, List<Vector2> materialCoordinates) { }

    protected void AddSquareToMesh(Vector3 origin, Vector3 firstOffset, Vector3 sndOffset, float tiling, List<Vector3> vertices, List<int> triangles, List<Vector2> materialCoordinates)
    {
        AddTriangleToMesh(origin, origin + firstOffset, origin + firstOffset + sndOffset, tiling, vertices, triangles, materialCoordinates);
        AddTriangleToMesh(origin + firstOffset + sndOffset, origin + sndOffset, origin, tiling, vertices, triangles, materialCoordinates);
    }

    protected void AddTriangleToMesh(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, float tiling, List<Vector3> vertices, List<int> triangles, List<Vector2> materialCoordinates)
    {
        int index = vertices.Count;
        vertices.Add(vertexA);
        vertices.Add(vertexB);
        vertices.Add(vertexC);
        triangles.Add(index);
        triangles.Add(index + 2);
        triangles.Add(index + 1);
        if (materialCoordinates != null)
        {
            materialCoordinates.Add(new Vector2(vertexA.x, vertexA.y) * tiling);
            materialCoordinates.Add(new Vector2(vertexB.x, vertexB.y) * tiling);
            materialCoordinates.Add(new Vector2(vertexC.x, vertexC.y) * tiling);
        }
    }


}
