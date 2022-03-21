using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoors : MonoBehaviour
{

    public const float DOOR_LIFT_DISTANCE = RoomConnector.CONNECTOR_HEIGHT;

    protected bool isOpen;
    public bool IsOpen => isOpen;

    public Vector3 closedPosition;

    public void Open()
    {
        isOpen = true;
        UpdatePosition();
    }

    public void Close()
    {
        isOpen = false;
        UpdatePosition();
    }

    protected void UpdatePosition()
    {
        transform.localPosition = closedPosition + new Vector3(0, DOOR_LIFT_DISTANCE * Convert.ToInt32(isOpen), 0);
    }

}
