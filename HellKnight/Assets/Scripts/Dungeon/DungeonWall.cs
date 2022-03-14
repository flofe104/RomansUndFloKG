using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonWall : DungeonPart
{

    protected const float WALL_WIDTH = 20;

    protected override Vector2 DetermineDungeonPartSize()
    {
        return new Vector2(WALL_WIDTH, 0);
    }

    protected override void OnAddedRoomPartToMesh(float tiling, List<Vector3> vertices, List<int> triangles, List<Vector3> colliderVerts, List<int> colliderTris, List<Vector2> materialCoordinates)
    {
        Vector3 collisionOrigin = transform.position + new Vector3(0, 0, COLLISION_WIDTH);
        ///left wall collider
        AddSquareToMesh(collisionOrigin + new Vector3(0, 0, 0), new Vector3(0, 0, -2 * COLLISION_WIDTH), new Vector3(0, MAX_DUNGEON_HEIGHT - transform.position.y, 0), tiling, colliderVerts, colliderTris, null);
        ///right wall collider
        AddSquareToMesh(collisionOrigin + new Vector3(dungeonPartSize.x, 0, 0), new Vector3(0, MAX_DUNGEON_HEIGHT - transform.position.y, 0), new Vector3(0, 0, -2 * COLLISION_WIDTH), tiling, colliderVerts, colliderTris, null);
    }


}
