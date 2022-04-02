using System.Collections;
using System.Collections.Generic;
using Testing;
using UnityEngine;

[TestMonoBehaviour]
public class RangedEnemyMovement : MonoBehaviour
{
    public const float HOVER_RADIUS = 1f;
    public const float HOVER_SPEED = 2f;

    protected Vector3 center;
    protected float t = 0;

    private void Start()
    {
        center = transform.position;
    }
    protected void Hover()
    {
        var pos = transform.position;
        t += Time.deltaTime * HOVER_SPEED;
        transform.position = center + HOVER_RADIUS * new Vector3(Mathf.Cos(t), Mathf.Sin(t));
    }

    void Update()
    {
        Hover();
    }


    #region Tests

    [TestEnumerator]
    public IEnumerator TestHover()
    {
        float circumference = 2 * Mathf.PI * HOVER_RADIUS;

        var distance = Vector3.Distance(transform.position, center);
        Assert.ApproxEqual(distance, HOVER_RADIUS);


        var posBefore = transform.position;
        yield return new WaitForSeconds(circumference / HOVER_SPEED / 2);

        distance = Vector3.Distance(transform.position, center);
        Assert.ApproxEqual(distance, HOVER_RADIUS);


        yield return new WaitForSeconds(circumference / HOVER_SPEED / 2);
        var posAfter = transform.position;

        distance = Vector3.Distance(posBefore, posAfter);
        Assert.ApproxEqual(distance, 0);

        distance = Vector3.Distance(transform.position, center);
        Assert.ApproxEqual(distance, HOVER_RADIUS);
    }

    #endregion
}
