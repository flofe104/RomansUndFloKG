using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattformGenerator
{

    public static AnimationCurve plattformWidthEvaluator;

    public static AnimationCurve plattformHeightEvaluator;

    protected const float maxJumpHeight = 2f;
    protected const float maxJumpDistance = 2;
    protected const float playerHeight = 1f;

    public static List<RoomPlattform> GeneratePlattformsInSpace(Vector2Int size, System.Random rand)
    {
        List<RoomPlattform> result = new List<RoomPlattform>();
        
        float layerHeight = maxJumpHeight;
        float maxHeight = size.y - layerHeight - playerHeight;

        int maxLayer = Mathf.FloorToInt(maxHeight / layerHeight);

        for (int layer = 1; layer < 2; layer++)
        {
            float width = plattformWidthEvaluator.Evaluate((float)rand.NextDouble());
            float height = 0.3f;
            float heightOffset = Mathf.Lerp(height + playerHeight, layerHeight, plattformHeightEvaluator.Evaluate((float)rand.NextDouble()));
            float widthOffset = Mathf.Lerp(0, size.y - width, plattformHeightEvaluator.Evaluate((float)rand.NextDouble()));
            result.Add(new RoomPlattform(new Vector2(widthOffset, heightOffset - height), width, height));
        }

        return result;
    }


}
