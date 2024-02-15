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

    const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0f, 1f)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public Vector2Int structureLocation;
    public float structureRadius;
    public int structureSmoothness;

    public bool autoUpdate;

    public TerrainTypes[] regions;

    public TreeGenerationMesh treeGen;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize,seed, noiseScale, octaves,persistance,lacunarity,offset);

        noiseMap = NoiseSmoothing.smoothHeightMap(noiseMap, structureLocation, structureRadius, structureSmoothness);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for(int y = 0; y < mapChunkSize; y++)
        {
            for(int x = 0; x < mapChunkSize; x++)
            {
                colorMap[y * mapChunkSize + x] = regions[0].color;
            }
        }
        MapDisplay display = gameObject.GetComponent<MapDisplay>();
        if (drawMode==DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colorMap, mapChunkSize));
        }
        else if (drawMode==DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier,meshHeightCurve,levelOfDetail), TextureGenerator.TextureFromColourMap(colorMap, mapChunkSize));
        }
        treeGen.TreeGen(structureLocation,structureRadius);
    }

    private void OnValidate()
    {
        if(lacunarity < 1) { lacunarity = 1; }
        if(octaves < 0) { octaves = 0; }
        if(meshHeightMultiplier < 1) { meshHeightMultiplier = 1; }
    }

    private void Start()
    {
        GenerateMap();
    }
}

[System.Serializable]
public struct TerrainTypes
{
    public string name;
    public float height;
    public Color color;
}