using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnector : DungeonPart
{

    public const float CONNECTOR_HEIGHT = /*PLAYER_HEIGHT + PLAYER_JUMP_HEIGHT*/ PlattformGenerator.PLAYER_HEIGHT * 1.5f;
    protected const float CONNECTOR_WIDTH = /*PLAYER_HEIGHT + PLAYER_JUMP_HEIGHT*/ 10;
    protected const float DOOR_WIDTH = 1;
    protected const float DOOR_Z_LENGTH = 0.2f;
    protected override void DetermineDungeonPartSize(out Vector2 dungeonPartSize)
    {
        dungeonPartSize = new Vector2(CONNECTOR_WIDTH, CONNECTOR_HEIGHT);
    }

    public void Initialize(GameObject doorPrefab)
    {
        entryDoor = CreateDoor(doorPrefab,0);
        entryDoor.Close();
        exitDoor = CreateDoor(doorPrefab, CONNECTOR_WIDTH - DOOR_WIDTH);
        exitDoor.Open();
    }

    public RoomDoors CreateDoor(GameObject prefab, float xOffset)
    {
        GameObject door = Instantiate(prefab, transform);
        Transform t = door.transform;
        t.localScale = new Vector3(DOOR_WIDTH, CONNECTOR_HEIGHT, DOOR_Z_LENGTH);
        t.localPosition = t.localScale / 2 + new Vector3(xOffset, 0, -DOOR_Z_LENGTH/2);
        RoomDoors doorBehaviour = door.GetOrAddComponent<RoomDoors>();
        doorBehaviour.closedPosition = t.localPosition;
        return doorBehaviour;
    }

    public RoomDoors entryDoor;
    public RoomDoors exitDoor;

    public bool IsEntryOpen => entryDoor.IsOpen;
    public bool IsExitOpen => exitDoor.IsOpen;

}
