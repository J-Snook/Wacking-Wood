using UnityEngine;

public static class Noise
{
    public enum NormalizeMode {
        Local,
        Global
    }

    public static float[,] GenerateNoiseMap(int mapSize,int seed, float scale, int octaves, float persistance, float lacuarity,Vector2 offset,NormalizeMode normalizeMode)
    {
        float[,] noiseMap = new float[mapSize, mapSize];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1f;
        float frequency = 1f;


        for(int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000,100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        if(scale <= 0) { scale = 0.0001f; }
        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        int halfSize = mapSize / 2;
        for(int y = 0; y < mapSize; y++)
        {
            for(int x = 0; x < mapSize; x++)
            {
                amplitude = 1f;
                frequency = 1f;
                float noiseHeight = 0f;
                for(int i = 0; i < octaves; i++)
                {
                    float sampleX = (x-halfSize + octaveOffsets[i].x) / scale * frequency;
                    float sampleY = (y-halfSize + octaveOffsets[i].y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacuarity;
                }
                if(noiseHeight > maxLocalNoiseHeight) { maxLocalNoiseHeight = noiseHeight; }
                else if(noiseHeight < minLocalNoiseHeight) { minLocalNoiseHeight = noiseHeight; }
                noiseMap[x, y] = noiseHeight;
            }
        }
        for(int y = 0; y < mapSize; y++)
        {
            for(int x = 0; x < mapSize; x++)
            {
                if (normalizeMode == NormalizeMode.Local)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                }
                else
                {
                    float normalizedHeight = ((noiseMap[x, y] + 1f) / (2f * maxPossibleHeight/1.25f));
                    noiseMap[x,y] = Mathf.Clamp(normalizedHeight,0f,int.MaxValue);
                }
            }
        }
        return noiseMap;
    }
}
