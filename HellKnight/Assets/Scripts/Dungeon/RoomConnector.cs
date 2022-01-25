using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnector : DungeonPart
{

    protected const int CONNECTOR_HEIGHT = /*PLAYER_HEIGHT + PLAYER_JUMP_HEIGHT*/ 3;
    protected const int CONNECTOR_WIDTH = /*PLAYER_HEIGHT + PLAYER_JUMP_HEIGHT*/ 10;

    protected override Vector2Int DetermineDungeonPartSize()
    {
        return new Vector2Int(CONNECTOR_WIDTH, CONNECTOR_HEIGHT);
    }

}
