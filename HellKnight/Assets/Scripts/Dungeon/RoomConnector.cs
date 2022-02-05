using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnector : DungeonPart
{

    public const float CONNECTOR_HEIGHT = /*PLAYER_HEIGHT + PLAYER_JUMP_HEIGHT*/ PlattformGenerator.PLAYER_HEIGHT * 1.5f;
    protected const float CONNECTOR_WIDTH = /*PLAYER_HEIGHT + PLAYER_JUMP_HEIGHT*/ 10;

    protected override Vector2 DetermineDungeonPartSize()
    {
        return new Vector2(CONNECTOR_WIDTH, CONNECTOR_HEIGHT);
    }

}
