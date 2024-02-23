using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseSmoothing
{
    public static float[,] smoothHeightMap(float[,] heightMap, Vector2Int location, float hardRadius,float structureSmoothness)
    {
        int size = heightMap.GetLength(0)-1;
        float targetHeight = heightMap[location.x, location.y];
        int smoothingRange = Mathf.CeilToInt(hardRadius * structureSmoothness);
        for(int y = location.y - smoothingRange; y < location.y + smoothingRange; y++)
        {
            if(y < 0 || y > size) continue;
            for(int x = location.x - smoothingRange; x < location.x + smoothingRange; x++)
            {
                if(x < 0 || x > size) continue;
                Vector2Int pos = new Vector2Int(x, y);
                float dist = Vector2Int.Distance(pos, location);
                if(dist <= hardRadius)
                {
                    heightMap[x, y] = targetHeight;
                }
                else if(dist <= hardRadius * structureSmoothness)
                {
                    heightMap[x, y]=Mathf.Lerp(targetHeight, heightMap[x, y], (dist - hardRadius) / (hardRadius * structureSmoothness - hardRadius));
                }
            }
        }
        return heightMap;
    }
}
