using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Vector2Int mapSize;
    public float noiseScale;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, noiseScale);
        MapDisplay display = gameObject.GetComponent<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }
}
