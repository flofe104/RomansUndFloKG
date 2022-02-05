using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : BaseMovement
{
    public float hoverDistance = 0.8f;
    public AnimationCurve hoverPatternX;
    public AnimationCurve hoverPatternY;

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
        var x = hoverPatternX.Evaluate(t);
        var y = hoverPatternY.Evaluate(t);
        transform.position = center + hoverDistance * new Vector3(x, y);
    }

    void Update()
    {
        Hover();
    }
}
