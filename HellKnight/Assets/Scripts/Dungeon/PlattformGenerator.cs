using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattformGenerator
{

    public const float PLAYER_HEIGHT = 2f;

    public const float MAX_JUMP_HEIGHT = 5f;
    public const float MAX_JUMP_DISTANCE = 4f;

    private const float MIN_WIDTH = 1;
    private const float MAX_WIDTH = 5;
    private const float HEIGHT = 0.3f;
    private const float MIN_X_OFFSET = 1;
    private const float MIN_Y_OFFSET = 2;

    public static List<RoomPlattform> GeneratePlattformsInSpace(Vector2 size, System.Random rand)
    {
        List<RoomPlattform> result = new List<RoomPlattform>();

        float layerBaseHeight = 0;
        float layerBaseWidth = 0;

        float yBuffer = PLAYER_HEIGHT + MAX_JUMP_HEIGHT;

        int maxIterations = 50;
        int i = 0;

        //Debug.Log("Roomsize: " + size.x + " , " + size.y);

        bool finished = false;
        while (!finished && i <= maxIterations)
        {
            float yOffset = Mathf.Lerp(MIN_Y_OFFSET, MAX_JUMP_HEIGHT - HEIGHT, (float)rand.NextDouble());
            float yPos = layerBaseHeight + yOffset;
            if(yPos + HEIGHT + yBuffer > size.y)
            {
                finished = true;
                if (layerBaseHeight + MAX_JUMP_HEIGHT >= size.y)
                    break;
                else
                    yPos = size.y - yBuffer - HEIGHT;
            }
            layerBaseHeight = yPos + HEIGHT;

            float xOffset = Mathf.Lerp(MIN_X_OFFSET, MAX_JUMP_DISTANCE, (float)rand.NextDouble());
            float xPos = layerBaseWidth + xOffset;
            float width = Mathf.Lerp(MIN_WIDTH, MAX_WIDTH, (float)rand.NextDouble());

            if (xPos + width > size.x)
            {
                finished = true;
            }
            else
            {
                layerBaseWidth = xPos + width;
            }

            //Debug.Log("PS pos: " + xPos + " , " + yPos);


            var platform = new RoomPlattform(new Vector2(xPos, yPos), width, HEIGHT);
            result.Add(platform);

            i++;
        }

        return result;
    }
}
