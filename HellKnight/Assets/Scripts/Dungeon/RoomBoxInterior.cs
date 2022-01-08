using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBoxInterior
{

    protected Bounds bounds;

    protected Material mat;

    protected GameObject box; 

    protected void CreateBox()
    {
        box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        AdjustBox();
    }

    protected GameObject Box
    {
        get
        {
            if(box == null)
            {
                CreateBox();
            }
            return box;
        }
    }

    protected void AdjustBox()
    {
        box.transform.localScale = bounds.extents * 2;
        box.transform.localPosition = bounds.center;
        AdjustBoxCollider(box.GetOrAddComponent<BoxCollider>());
    }

    protected void AdjustBoxCollider(BoxCollider collider)
    {
        collider.center = bounds.center;
        collider.size = bounds.extents * 2;
    }

    protected virtual void Display()
    {
        CreateBox();
    }

}
