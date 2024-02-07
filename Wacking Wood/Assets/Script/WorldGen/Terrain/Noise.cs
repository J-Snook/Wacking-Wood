using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(Vector2Int mapSize,int seed, float scale, int octaves, float persistance, float lacuarity,Vector2 offset,float smoothness)
    {
        float[,] noiseMap = new float[mapSize.x, mapSize.y];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000,100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if(scale <= 0) { scale = 0.0001f; }
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        Vector2 halfSize = mapSize / 2;
        for(int y = 0; y < mapSize.y; y++)
        {
            for(int x = 0; x < mapSize.x; x++)
            {
                float amplitude = 1f;
                float frequency = 1f;
                float noiseHeight = 0f;
                for(int i = 0; i < octaves; i++)
                {
                    float sampleX = (x-halfSize.x) / scale * frequency + octaveOffsets[i].x * frequency;
                    float sampleY = (y-halfSize.y) / scale * frequency - octaveOffsets[i].y * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacuarity;
                }
                if(noiseHeight > maxNoiseHeight) { maxNoiseHeight = noiseHeight; }
                else if(noiseHeight < minNoiseHeight) { minNoiseHeight = noiseHeight; }
                noiseMap[x, y] = noiseHeight;
            }
        }
        for(int y = 0; y < mapSize.y; y++)
        {
            for(int x = 0; x < mapSize.x; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight*smoothness, maxNoiseHeight/ smoothness, noiseMap[x, y]);
            }
        }
        return noiseMap;
    }
}
