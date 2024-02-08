using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColourMap,
        Mesh
    }
    public DrawMode drawMode;

    public Vector2Int mapSize;
    public float noiseScale;

    public int octaves;
    [Range(0f, 1f)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;

    public float smoothness=1;

    public bool autoUpdate;

    public TerrainTypes[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize,seed, noiseScale, octaves,persistance,lacunarity,offset,smoothness);

        Color[] colorMap = new Color[mapSize.x * mapSize.y];
        for(int y = 0; y < mapSize.y; y++)
        {
            for(int x = 0; x < mapSize.x; x++)
            {
                float currentHeight = noiseMap[x, y];
                for(int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapSize.x + x] = regions[i].color;
                        break;
                    }

                }
            }
        }
        MapDisplay display = gameObject.GetComponent<MapDisplay>();
        if (drawMode==DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colorMap, mapSize));
        }
        else if (drawMode==DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier), TextureGenerator.TextureFromColourMap(colorMap, mapSize));
        }
    }

    private void OnValidate()
    {
        if(mapSize.x < 1) { mapSize.x = 1; }
        if(mapSize.y < 1) { mapSize.y = 1; }
        if(lacunarity < 1) { lacunarity = 1; }
        if(octaves < 0) { octaves = 0; }
        if(meshHeightMultiplier < 1) { meshHeightMultiplier = 1; }
    }
}

[System.Serializable]
public struct TerrainTypes
{
    public string name;
    public float height;
    public Color color;
}
