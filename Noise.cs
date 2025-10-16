using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, Vector2 offset,int octaves, float persistence, float lacunarity)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY); // Set the offsets for the map(originally xOrg and yOrg)
        }

        if (scale <= 0f) scale = 0.00001f;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (float y = 0f; y < mapHeight; y++)
        {
            for (float x = 0f; x < mapWidth; x++)
            {
                float amplitude = 1f;
                float frequency = 1f;
                float noiseHeight = 0f;

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = octaveOffsets[i].x + (x - mapWidth/2f) /scale * frequency;
                    float yCoord = octaveOffsets[i].y + (y - mapHeight/2f) /scale * frequency;
                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2f - 1f;  //Calculate perlin noise using sampled coordinates

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight) //if the current noiseHeight sample is Lower than the minimum, set the minimum to the current height
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[(int)x, (int)y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]); //Normalize height values between 0-1, finding the lerp value between the min and max height
            }
        }

        return noiseMap;
    }
}
