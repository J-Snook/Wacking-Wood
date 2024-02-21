using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const float maxViewDistance = 450;
    public Transform player;
    public Material mapMat;
    public static Vector2 playerPosition;
    static MapGenerator _mapGenerator;
    int chunkSize;
    int chunksVisableInViewDst;

    Dictionary<Vector2,TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2,TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    private void Start()
    {
        _mapGenerator = FindObjectOfType<MapGenerator>();
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisableInViewDst = Mathf.RoundToInt(maxViewDistance / chunkSize);
    }

    private void Update()
    {
        playerPosition = new Vector2(player.position.x, player.position.z);
        UpdateVisibleChunks();
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
                    if(terrainChunkDictionary[viewedChunkCoord].isVisible()) 
                    {
                        terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
                    }
                } else
                {
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord,chunkSize,transform, mapMat));
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

        public TerrainChunk(Vector2 coord,int size,Transform parent,Material mat) 
        {
            pos = coord * size;
            bounds = new Bounds(pos, Vector3.one * size);
            Vector3 posV3 = new Vector3(pos.x, 0, pos.y);

            meshObj = new GameObject("TerrainChunk");
            meshRenderer = meshObj.AddComponent<MeshRenderer>();
            meshFilter = meshObj.AddComponent<MeshFilter>();
            meshRenderer.sharedMaterial = mat;

            meshObj.transform.position = posV3;
            meshObj.transform.parent= parent;
            setVisible(false);

            _mapGenerator.RequestMapData(OnMapDataRecvd);
        }

        void OnMapDataRecvd(MapData mapData)
        {
            _mapGenerator.RequestMeshData(mapData, OnMeshDataRecvd);
        }

        void OnMeshDataRecvd(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMesh();
        }
 
        public void UpdateTerrainChunk()
        {
            float playerDistFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPosition));
            bool visible = playerDistFromNearestEdge <= maxViewDistance;
            setVisible(visible);
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
}
