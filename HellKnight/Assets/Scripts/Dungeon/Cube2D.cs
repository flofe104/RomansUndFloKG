using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2D
{

    public Cube2D(Vector2 bottomLeftAnchor, float width, float height)
    {
        this.bottomLeftAnchor = bottomLeftAnchor;
        this.width = width; 
        this.height = height;
        Width = new Vector2(width, 0);
        Height = new Vector2(0, height);
    }

    protected Vector2 bottomLeftAnchor;

    protected float width;

    public Bounds GlobalBounds
    {
        get
        {
            return new Bounds(Center3D, new Vector3(width, height, 1)); 
        }
    }

    public float GetWidth() => width;

    public float ExtendsWidth => width / 2;

    protected float height;

    public float GetHeight() => height;

    protected Vector2 Width;

    protected Vector2 Height;

    public Vector2 BottomRight => bottomLeftAnchor + Width;

    public Vector2 TopLeft => bottomLeftAnchor + Height;

    public Vector2 TopRight => BottomRight + Height;

    public Vector2 Center => bottomLeftAnchor + new Vector2(width / 2, height / 2);

    public Vector3 Center3D => new Vector3(Center.x, Center.y, 0);

    protected Material mat;

    protected GameObject box;

    protected Transform parent;

    public GameObject Create(Transform parent = null)
    {
        if (box == null) 
        { 
            this.parent = parent;
            Display();
        }
        return box;
    }

    protected void CreateBox()
    {
        box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.SetParent(parent);
        AdjustCube();
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

    protected void AdjustCube()
    {
        box.transform.localScale = new Vector3(width,height,1);
        box.transform.localPosition = Center;
        box.GetOrAddComponent<BoxCollider>();
    }


    protected virtual void Display()
    {
        CreateBox();
    }

}
