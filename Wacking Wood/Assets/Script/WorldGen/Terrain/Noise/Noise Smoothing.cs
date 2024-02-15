using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseSmoothing
{
    public static float[,] smoothHeightMap(float[,] heightMap, Vector2Int location, float hardRadius,int structureSmoothness)
    {
        int size = heightMap.GetLength(0);
        float targetHeight = heightMap[location.x, location.y];
        for(int y = 0; y < size; y++)
        {
            for(int x = 0; x < size; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                float dist = Vector2Int.Distance(new Vector2Int(x, y), location);
                if(dist <= hardRadius)
                {
                    heightMap[x, y] = targetHeight;
                }
                else if(dist <= hardRadius * 10)
                {
                    heightMap[x, y] = Mathf.Lerp(targetHeight, heightMap[x, y], (dist - hardRadius) / (hardRadius * structureSmoothness - hardRadius));
                }
            }
        }
        return heightMap;
    }
}
