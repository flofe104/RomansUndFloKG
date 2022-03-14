using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyMovement : BaseMovement
{
    public float hoverDistance = 0.8f;

    protected Vector3 center;
    protected float t = 0;

    private void Start()
    {
        speed = 0.8f;
        center = transform.position;
    }
    protected void Hover()
    {
        var pos = transform.position;
        t += 1 / speed * Time.deltaTime;
        transform.position = center + hoverDistance * new Vector3(Mathf.Cos(t), Mathf.Sin(t));
    }

    void Update()
    {
        Hover();
    }
}
