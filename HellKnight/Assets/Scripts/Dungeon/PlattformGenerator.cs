using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattformGenerator
{
    protected const float maxJumpHeight = 5f;
    protected const float maxJumpDistance = 4f;

    private const int MIN_WIDTH = 1;
    private const int MAX_WIDTH = 5;
    private const int MIN_X_OFFSET = 1;
    private const int MIN_Y_OFFSET = 2;

    public static List<RoomPlattform> GeneratePlattformsInSpace(Vector2Int size, System.Random rand)
    {
        List<RoomPlattform> result = new List<RoomPlattform>();

        float layerBaseHeight = 0;
        float layerBaseWidth = MIN_X_OFFSET;

        int maxIterations = 100;
        int i = 0;
        while (layerBaseHeight <= size.y || layerBaseWidth <= size.x || i >= maxIterations)
        {
            float width = (float)rand.Next(MIN_WIDTH, MAX_WIDTH);
            float height = 0.3f;

            float yPos = Mathf.Lerp(layerBaseHeight + MIN_Y_OFFSET, layerBaseHeight + maxJumpHeight, (float)rand.NextDouble());
            float xPos = Mathf.Lerp(layerBaseWidth + MIN_X_OFFSET, layerBaseWidth + maxJumpDistance, (float)rand.NextDouble());

            if(layerBaseHeight <= size.y)
                layerBaseHeight = yPos + height;
            if (layerBaseWidth <= size.x)
                layerBaseWidth = xPos + width;
            i++;
            result.Add(new RoomPlattform(new Vector2(xPos, yPos), width, height));
        }

        return result;
    }
}
