using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject buildingPrefab;
    
    public enum DrawMode
    {
        NoiseMap,
        ColourMap,
        Mesh
    }
    public DrawMode drawMode;

    public const int mapChunkSize = 241;
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

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData();
        MapDisplay display = gameObject.GetComponent<MapDisplay>();
        if(drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightmap));
        }
        else if(drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapData.colorMap, mapChunkSize));
        }
        else if(drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightmap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(mapData.colorMap, mapChunkSize));
        }
    }

    public void RequestMapData(Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(callback);
        };

        new Thread(threadStart).Start();
    }

    void MapDataThread(Action<MapData> callback)
    {
        MapData mapData = GenerateMapData();
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData,Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData,callback);
        };

        new Thread(threadStart).Start();
    }

    void MeshDataThread(MapData mapData,Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightmap,meshHeightMultiplier,meshHeightCurve,levelOfDetail);
        lock (meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback,meshData));
        }
    }

    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
        {
            for(int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        if(meshDataThreadInfoQueue.Count > 0)
        {
            for(int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    private MapData GenerateMapData()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize,seed, noiseScale, octaves,persistance,lacunarity,offset);

        //noiseMap = NoiseSmoothing.smoothHeightMap(noiseMap, structureLocation, structureRadius, structureSmoothness);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for(int y = 0; y < mapChunkSize; y++)
        {
            for(int x = 0; x < mapChunkSize; x++)
            {
                colorMap[y * mapChunkSize + x] = regions[0].color;
            }
        }

        /*Vector3 _structPos = treeGen.TreeGen(structureLocation, structureRadius);
        if (Physics.Raycast(_structPos, Vector3.down,out RaycastHit hit))
        {
            Instantiate(buildingPrefab,new Vector3(_structPos.x,hit.point.y+buildingPrefab.transform.localScale.y/2,_structPos.z),Quaternion.identity);
        }*/

        return new MapData(noiseMap,colorMap);
    }

    private void OnValidate()
    {
        if(lacunarity < 1) { lacunarity = 1; }
        if(octaves < 0) { octaves = 0; }
        if(meshHeightMultiplier < 1) { meshHeightMultiplier = 1; }
    }

    private void Start()
    {
        if (structureLocation.x == 0 && structureLocation.y == 0 )
        {
            structureLocation = new Vector2Int(UnityEngine.Random.Range(Mathf.CeilToInt(structureRadius), Mathf.FloorToInt(240f - structureRadius)), UnityEngine.Random.Range(Mathf.CeilToInt(structureRadius), Mathf.FloorToInt(240f - structureRadius)));
        }
        if (offset.x == 0 && offset.y == 0 )
        {
            offset = new Vector2(UnityEngine.Random.Range(-100000f,100000f), UnityEngine.Random.Range(-100000f, 100000f));
        }
        GenerateMapData();
    }


    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback,T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}

[System.Serializable]
public struct TerrainTypes
{
    public string name;
    public float height;
    public Color color;
}

public struct MapData
{
    public readonly float[,] heightmap;
    public readonly Color[] colorMap;

    public MapData(float[,] heightmap, Color[] colorMap)
    {
        this.heightmap = heightmap;
        this.colorMap = colorMap;
    }
}