using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeGenerationMesh : MonoBehaviour,IDataPersistance
{
    private List<Vector3> treePoints;
    private List<GameObject> treeObjects = new List<GameObject>();
    private ObjectPooler _pooler;
    private string treeTag;
    private Vector2 coord;

    private void Awake()
    {
        _pooler = ObjectPooler.Instance;
        DataPersistanceManager _dataManagement = DataPersistanceManager.instance;
        
    }

    public void treeInit(Vector2 coord)
    {
        this.coord = coord;
        if (DataPersistanceManager.instance.gameData.treeStoreTags.TryGetValue(coord,out string tag))
        {
            treeTag = tag;
        }
        if(DataPersistanceManager.instance.gameData.treeStorePoints.TryGetValue(coord, out List<Vector3> points))
        {
            treePoints = points;
        }
    }

    public void TreeGen(MeshRenderer meshRenderer, BuildingGeneration buildingScript,float density=7f,int rejectionSamples=2)
    {
        if (treeObjects.Count >0) { return; }
        if (treePoints == null )
        {
            treePoints = new List<Vector3>();
            treeTag = "tree" + Random.Range(1, 4).ToString();
            Bounds _meshBounds = meshRenderer.bounds;
            Vector2 regionSize = new Vector2(_meshBounds.size.x, _meshBounds.size.z);
            List<Vector2> points = PointGeneration.GeneratePoints(density, regionSize, rejectionSamples);
            Vector2 _structPos = new Vector2(buildingScript.buildingLocalPos.x, 240f - buildingScript.buildingLocalPos.y);
            foreach(Vector2 point in points)
            {
                float sqrDist = (_structPos - point).sqrMagnitude;
                if(sqrDist > buildingScript.building.radius * 1.25f)
                {
                    Vector3 _truePos = new Vector3(point.x, _meshBounds.max.y, point.y) + _meshBounds.min;
                    float rayDist = _meshBounds.size.y;
                    if(Physics.Raycast(_truePos, Vector3.down, out RaycastHit hit))
                    {
                        _truePos.y = hit.point.y;
                        treePoints.Add(_truePos);
                        continue;
                    }
                }
            }
        }
        int Count = 1;
        foreach(Vector3 point in treePoints)
        {
            GameObject tree = _pooler.SpawnFromPool(treeTag, point, Quaternion.identity);
            tree.transform.parent = transform;
            tree.name = treeTag+ ":" + Count;
            treeObjects.Add(tree);
            Count++;
        }
    }


    public void ResetTrees()
    {
        for(int i = 0; i < treeObjects.Count; i++)
        {
            treeObjects[i].SetActive(false);
            treeObjects[i].transform.parent = _pooler.transform.Find(treeTag);
        }
        treeObjects.Clear();
    }

    public void LoadData(GameData data)
    {
        Debug.Log("This shouldnt load here");
    }

    public void SaveData(ref GameData data)
    {
        if (data.treeStorePoints.ContainsKey(coord))
        {
            data.treeStorePoints.Remove(coord);
        }
        if(data.treeStoreTags.ContainsKey(coord))
        {
            data.treeStoreTags.Remove(coord);
        }
        data.treeStoreTags.Add(coord, treeTag);
        data.treeStorePoints.Add(coord, treePoints);
    }
}