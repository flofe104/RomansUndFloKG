using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DungeonPart : MonoBehaviour
{

    protected const float MAX_HEIGHT = 50;
    protected const float MIN_HEIGHT = -50;
    protected const float COLLISION_WIDTH = 1;

    protected Vector2Int dungeonPartSize;

    protected abstract void DetermineDungeonPartSize();

    public void AddRoomLayoutToMeshData(float tiling, List<Vector3> vertices, List<int> triangles, List<Vector3> meshVerts, List<int> meshTris, List<Vector2> materialCoordinates)
    {
        DetermineDungeonPartSize();
        float startHeight = transform.position.y + dungeonPartSize.y;
        float distanceToLowestHeight = transform.position.y - MIN_HEIGHT;
        ///build rendered mesh
        AddSquareToMesh(transform.position + new Vector3(0, dungeonPartSize.y, 0), dungeonPartSize.x, MAX_HEIGHT - startHeight, tiling, vertices, triangles, materialCoordinates);
        AddSquareToMesh(transform.position - new Vector3(0, distanceToLowestHeight, 0), dungeonPartSize.x, distanceToLowestHeight, tiling, vertices, triangles, materialCoordinates);

        ///build collision mesh
        //AddSquareToMesh(transform.position + new Vector3(0, 0, COLLISION_WIDTH), dungeonPartSize.x, MAX_HEIGHT - startHeight, tiling, vertices, triangles, materialCoordinates);
        //AddSquareToMesh(transform.position - new Vector3(0, distanceToLowestHeight, 0), dungeonPartSize.x, distanceToLowestHeight, tiling, vertices, triangles, materialCoordinates);

    }

    protected void AddSquareToMesh(Vector3 origin, float width, float height, float tiling, List<Vector3> vertices, List<int> triangles, List<Vector2> materialCoordinates)
    {
        AddSquareToMesh(new Vector2(origin.x, origin.y), width, height, tiling, vertices, triangles, materialCoordinates);
    }

    protected void AddSquareToMesh(Vector2 origin, float width, float height, float tiling, List<Vector3> vertices, List<int> triangles, List<Vector2> materialCoordinates)
    {
        AddTriangleToMesh(origin, new Vector2(width, 0), new Vector2(width, height), tiling, vertices, triangles, materialCoordinates);
        AddTriangleToMesh(origin + new Vector2(width, height), new Vector2(-width, 0), new Vector2(-width, -height), tiling, vertices, triangles, materialCoordinates);
    }

    protected void AddTriangleToMesh(Vector2 origin, Vector2 firstOffset, Vector2 sndOffset, float tiling, List<Vector3> vertices, List<int> triangles, List<Vector2> materialCoordinates)
    {
        int index = vertices.Count;
        Vector3 vertexA = new Vector3(origin.x, origin.y, 0);
        Vector3 vertexB = new Vector3(origin.x + firstOffset.x, origin.y + firstOffset.y, 0);
        Vector3 vertexC = new Vector3(origin.x + sndOffset.x, origin.y + sndOffset.y, 0);
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
