using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(Vector2Int mapSize, float scale)
    {
        float[,] noiseMap = new float[mapSize.x, mapSize.y];

        if(scale <= 0) { scale = 0.0001f; }
        for(int y = 0; y < mapSize.y; y++)
        {
            for(int x = 0; x < mapSize.x; x++)
            {
                float sampleX = x / scale;
                float sampleY = y / scale;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }
}
