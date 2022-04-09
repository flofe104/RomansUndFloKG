using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DungeonPart : MonoBehaviour
{

    protected const int MAX_DUNGEON_HEIGHT = 400;
    protected const int MIN_DUNGEON_HEIGHT = -400;
    protected const float COLLISION_WIDTH = 1;

    protected Vector2 dungeonPartSize;

    protected Vector3 DungeonPartSize3D => new Vector3(DungeonPartSize.x, DungeonPartSize.y, 0);

    protected Vector3 DungeonPartSizeWithDefaultWidth3D => new Vector3(DungeonPartSize.x - 2, DungeonPartSize.y, 1);

    protected Vector3 DungeonPartCenterPosition => DungeonPartSize3D / 2;

    protected float entryHeightOffset = 0;

    public float Width => dungeonPartSize.x;

    public virtual float ExitHeight => 0;

    public float Height => dungeonPartSize.y;

    protected abstract void DetermineDungeonPartSize(out Vector2 dungeonPartSize);

    protected Vector2 DungeonPartSize
    {
        get
        {
            if (dungeonPartSize == default)
            {
                DetermineDungeonPartSize(out dungeonPartSize);
            }
            return dungeonPartSize;
        }
    }

    public void AddDungeonPartToMeshLayout(float tiling, List<Vector3> vertices, List<int> triangles, List<Vector3> colliderVerts, List<int> colliderTris, List<Vector2> materialCoordinates)
    {
        float startHeight = transform.position.y + DungeonPartSize.y;
        float distanceToLowestHeight = transform.position.y - MIN_DUNGEON_HEIGHT;

        ///build rendered mesh
        AddSquareToMesh(transform.position + new Vector3(0, dungeonPartSize.y, 0), new Vector3(dungeonPartSize.x,0,0),new Vector3(0, MAX_DUNGEON_HEIGHT - startHeight,0), tiling, vertices, triangles, materialCoordinates);
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
