using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;

public class PlattformTest
{

    protected const int TEST_ITERATIONS = 500;

    [Test]
    public void TestNoPlattformLowerThanPlayerHeight()
    {
        Assert.IsTrue(IsTrueForAllIterations((p) => p.BottomRight.y > PlattformGenerator.PLAYER_HEIGHT, TEST_ITERATIONS)); 
    }

    [Test]
    public void TestAllRoomsHavePlattforms()
    {
        Assert.IsTrue(IsTrueForListForAllIterations((ps) => ps.Count > 0, TEST_ITERATIONS));
    }

    [Test]
    public void TestNoPlattformsToCloseToCeiling()
    {
        Assert.IsTrue(IsTrueForAllIterations((p, v2) => p.TopLeft.y + PlattformGenerator.PLAYER_HEIGHT < v2.y, TEST_ITERATIONS));
    }

    [Test]
    public void TestNoPlattformOutOfBounds()
    {
        Assert.IsTrue(IsTrueForAllIterations((p, v2) => p.BottomRight.x <= v2.x && p.TopLeft.x >= 0, TEST_ITERATIONS));
    }

    [Test]
    public void TestNoAdjacendPlattformsAreNotTouching()
    {
        Assert.IsTrue(TestAdjacentPlattforms(
           (prev, next) =>
           {
               return !prev.GlobalBounds.Intersects(next.GlobalBounds);
           }));
    }

    [Test]
    public void TestUpperPlattformDoesNotBlocksJumpFromLower()
    {
        Assert.IsTrue(TestAdjacentPlattforms(
                (prev, next) =>
                {
                    float xCenterDiff = Mathf.Abs(next.GlobalBounds.center.x - prev.GlobalBounds.center.x);
                    float widthDiff = prev.ExtendsWidth - next.ExtendsWidth;
                    return widthDiff + xCenterDiff > 0;
                }));
    }

    [Test]
    public void TestPlattformsAreReachable()
    {
        Assert.IsTrue(TestAdjacentPlattforms((prev, next) =>
        {
            Vector2 distance = DistanceBetweenColliders(prev, next);
            return distance.y <= PlattformGenerator.MAX_JUMP_HEIGHT && distance.x <= PlattformGenerator.MAX_JUMP_DISTANCE;
        }));
    }

    public bool TestAdjacentPlattforms(Func<RoomPlattform, RoomPlattform, bool> p)
    {
        return IsTrueForListForAllIterations(
            (ps) =>
            {
                RoomPlattform coll = ps[0];
                RoomPlattform next;
                bool result = true;
                for (int i = 1; i < ps.Count && result; i++)
                {
                    next = ps[i];
                    result = p(coll,next);
                    coll = next;
                }
                return result;
            }, TEST_ITERATIONS);
    }

    private Vector2 DistanceBetweenColliders(RoomPlattform p1, RoomPlattform p2)
    {
        Vector2 distance = p1.Center - p2.Center;
        distance.x = Mathf.Abs(distance.x);
        distance.y = Mathf.Abs(distance.y);
        distance.x -= Mathf.Min(distance.x, p1.GetWidth());
        distance.x -= Mathf.Min(distance.x, p2.GetWidth());
        return distance;
    }

    protected bool IsTrueForAllIterations(Predicate<RoomPlattform> p, int iterations)
    {
        return IsTrueForListForAllIterations((ps) => ps.TrueForAll(p), iterations);
    }

    protected bool IsTrueForAllIterations(Func<RoomPlattform, Vector2Int, bool> p, int iterations)
    {
        return IsTrueForListForAllIterations((ps,v2) => ps.TrueForAll((x) => p(x,v2)), iterations);
    }

    protected bool IsTrueForListForAllIterations(Predicate<List<RoomPlattform>> p, int iterations)
    {
        return IsTrueForListForAllIterations((ps, _) => p(ps), iterations);
    }

    protected bool IsTrueForListForAllIterations(Func<List<RoomPlattform>, Vector2Int, bool> p, int iterations)
    {
        bool result = true;

        System.Random rand = new System.Random();
        for (int i = 0; i < iterations && result; i++)
        {
            Vector2Int space = new Vector2Int(rand.Next(10, 20), rand.Next(10, 20));
            result = p(PlattformGenerator.GeneratePlattformsInSpace(space, rand), space);
        }
        return result;
    }

}
