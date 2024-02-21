using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndlessTerrain : MonoBehaviour
{
    const float playerMoveThresholdForChunkUpdate=25f;
    const float sqrMoveThresholdForChunkUpdate = playerMoveThresholdForChunkUpdate * playerMoveThresholdForChunkUpdate;

    public LODInfo[] detailLevels;
    public BuildingInfo[] buildingInfo;
    public static float maxViewDistance;
    public Transform player;
    public Material mapMat;
    public static Vector2 playerPosition;
    Vector2 playerPositionOld;
    static MapGenerator _mapGenerator;
    int chunkSize;
    int chunksVisableInViewDst;

    Dictionary<Vector2,TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2,TerrainChunk>();
    static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    private void Start()
    {
        _mapGenerator = FindObjectOfType<MapGenerator>();

        maxViewDistance = detailLevels[detailLevels.Length-1].visableDistanceThreshold;
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisableInViewDst = Mathf.RoundToInt(maxViewDistance / chunkSize);
        UpdateVisibleChunks();
    }

    private void Update()
    {
        playerPosition = new Vector2(player.position.x, player.position.z);

        if ((playerPositionOld-playerPosition).sqrMagnitude > sqrMoveThresholdForChunkUpdate)
        {
            playerPositionOld = playerPosition;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks()
    {
        for(int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].setVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(playerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(playerPosition.y / chunkSize);

        for(int yOffset = -chunksVisableInViewDst; yOffset <= chunksVisableInViewDst; yOffset++)
        {
            for(int xOffset = -chunksVisableInViewDst; xOffset <= chunksVisableInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                } else
                {
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord,chunkSize,detailLevels,transform, mapMat,buildingInfo));
                }

            }
        }
    }

    public class TerrainChunk
    {
        GameObject meshObj;
        Vector2 pos;
        Bounds bounds;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        MeshCollider meshCollider;

        LODInfo[] detailLevels;
        BuildingInfo[] buildingInfo;
        LODMesh[] lodMeshes;
        Vector2Int structPos;
        int buildingIndex;

        MapData mapData;
        bool hasMapData;
        int previousLODIndex = -1;

        public TerrainChunk(Vector2 coord,int size,LODInfo[] detailLevels,Transform parent,Material mat, BuildingInfo[] buildingInfo) 
        {
            this.detailLevels= detailLevels;
            this.buildingInfo = buildingInfo;
            pos = coord * size;
            bounds = new Bounds(pos, Vector3.one * size);
            Vector3 posV3 = new Vector3(pos.x, 0, pos.y);

            meshObj = new GameObject("TerrainChunk");
            meshRenderer = meshObj.AddComponent<MeshRenderer>();
            meshFilter = meshObj.AddComponent<MeshFilter>();
            meshCollider = meshObj.AddComponent<MeshCollider>();
            meshRenderer.sharedMaterial = mat;
            meshObj.transform.position = posV3;
            meshObj.transform.parent= parent;
            setVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for(int i = 0; i < detailLevels.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod,UpdateTerrainChunk);
            }

            _mapGenerator.RequestMapData(pos,OnMapDataRecvd);
        }

        void OnMapDataRecvd(MapData mapData)
        {
            this.mapData = mapData;
            hasMapData= true;

            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colorMap, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;

            UpdateTerrainChunk();
        }
 
        public void UpdateTerrainChunk()
        {
            if (hasMapData)
            {
                float playerDistFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPosition));
                bool visible = playerDistFromNearestEdge <= maxViewDistance;

                if(visible)
                {
                    int lodIndex = 0;
                    for(int i = 0; i < detailLevels.Length - 1; i++)
                    {
                        if(playerDistFromNearestEdge > detailLevels[i].visableDistanceThreshold)
                        {
                            lodIndex = i + 1;
                        }
                        else { break; }
                    }
                    if(lodIndex != previousLODIndex)
                    {
                        LODMesh lodMesh = lodMeshes[lodIndex];
                        if(lodMesh.hasMesh)
                        {
                            previousLODIndex = lodIndex;
                            meshFilter.mesh = lodMesh.mesh;
                            meshCollider.sharedMesh = lodMesh.mesh;
                            //Generate Trees and Buildings?
                        }
                        else if(!lodMesh.hasRequestedMesh)
                        {
                            float randomChance = Random.Range(0f, 100f);
                            buildingIndex=-1;
                            for(int i = 0; i < buildingInfo.Length; i++)
                            {
                                if(randomChance < buildingInfo[i].upperValue ) { buildingIndex = i; } else { break; }
                            }
                            if(buildingIndex != -1)
                            {
                                float structureRadius = buildingInfo[buildingIndex].radius;
                                structPos = new Vector2Int(Random.Range(Mathf.CeilToInt(structureRadius), Mathf.FloorToInt(240f - structureRadius)), Random.Range(Mathf.CeilToInt(structureRadius), Mathf.FloorToInt(240f - structureRadius)));
                                float[,] heightmap = NoiseSmoothing.smoothHeightMap(mapData.heightmap, structPos, structureRadius, 1);
                                mapData = new MapData(heightmap,mapData.colorMap);
                            }
                            lodMesh.RequestMesh(mapData);
                        }
                    }
                    terrainChunksVisibleLastUpdate.Add(this);
                }
                setVisible(visible);
            }
        }

        public void setVisible(bool visible)
        {
            meshObj.SetActive(visible);
        }

        public bool isVisible()
        {
            return meshObj.activeSelf;
        }
    }

    class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        Action updateCallback;

        public LODMesh(int lod, Action updateCallback)
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }

        void OnMeshDataRecvd(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh= true;

            updateCallback();
        }

        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh= true;
            _mapGenerator.RequestMeshData(mapData,lod, OnMeshDataRecvd);
        }
    }

    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public float visableDistanceThreshold;
    }

    [System.Serializable]
    public struct BuildingInfo
    {
        public string name;
        public GameObject prefab;
        public float upperValue;
        public float radius;
    }
}
