using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeGenerationMesh : MonoBehaviour, IDataPersistance
{
    private List<Vector3> removedPoints;
    private List<Vector3> treePoints;
    private List<GameObject> treeObjects = new List<GameObject>();
    private ObjectPooler _pooler;
    private DataPersistanceManager _saveManager;
    private string treeTag;
    private Vector2 coord;
    private int seed;
    private System.Random prng;

    private void Awake()
    {
        _pooler = ObjectPooler.Instance;
        _saveManager = DataPersistanceManager.instance;
    }

    public void treeInit(Vector2 coord)
    {
        this.coord = coord;
        seed = _saveManager.gameData.seed;
        int x = (int)coord.x;
        int y = (int)coord.y;
        x = (x >= 0) ? 2 * x : -2 * (x - 1);
        y = (y >= 0) ? 2 * y : -2 * (y - 1);
        int a = (x >= y) ? (x * x) + x + y : x + (y * y); // Generates a Roughly unique number based on x and y except for when x||y ==0
        seed = seed * Mathf.RoundToInt(a);
        prng = new System.Random(seed);
        if(_saveManager.gameData.treeCutdown.TryGetValue(coord, out RemovedTrees removedPoints))
        {
            this.removedPoints = removedPoints.points;
        }
        else
        {
            this.removedPoints = new List<Vector3>();
        }
    }

    public void TreeGen(MeshRenderer meshRenderer, BuildingGeneration buildingScript)
    {
        if(treeObjects.Count > 0) { return; }
        if(treePoints == null)
        {
            treePoints = new List<Vector3>();
            treeTag = "tree" + prng.Next(1, 4).ToString();
            float density = ((float)prng.NextDouble() * 4f) + 6f;
            int rejectionSamples = prng.Next(2, 4);
            Bounds _meshBounds = meshRenderer.bounds;
            Vector2 regionSize = new Vector2(_meshBounds.size.x, _meshBounds.size.z);
            List<Vector2> points = PointGeneration.GeneratePoints(seed, density, regionSize, rejectionSamples);
            Vector2 _structPos = new Vector2(buildingScript.buildingLocalPos.x, 240f - buildingScript.buildingLocalPos.y);
            float sqrbuildRadius = buildingScript.building.radius * buildingScript.building.radius;
            foreach(Vector2 point in points)
            {
                float sqrDist = (_structPos - point).sqrMagnitude;
                if(sqrDist > sqrbuildRadius)
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
        treePoints = treePoints.Except(removedPoints).ToList();
        int Count = 1;
        foreach(Vector3 point in treePoints)
        {
            GameObject tree = _pooler.SpawnFromPool(treeTag, point, Quaternion.identity);
            tree.transform.parent = transform;
            tree.transform.GetComponentInChildren<TreeHit>().initPoint = point;
            tree.name = treeTag + ":" + Count;
            treeObjects.Add(tree);
            Count++;
        }
    }

    public void RemovePoint(Vector3 point)
    {
        if(treePoints.Contains(point))
        {
            treePoints.Remove(point);
            removedPoints.Add(point);
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

    }

    public void SaveData(ref GameData data)
    {
        if(removedPoints.Count > 0)
        {
            if(_saveManager.gameData.treeCutdown.ContainsKey(coord))
            {
                if(_saveManager.gameData.treeCutdown[coord].points.Count != removedPoints.Count)
                {
                    _saveManager.gameData.treeCutdown.Remove(coord);
                    _saveManager.gameData.treeCutdown.Add(coord, new RemovedTrees(removedPoints));
                }
            } else
            {
                _saveManager.gameData.treeCutdown.Add(coord, new RemovedTrees(removedPoints));
            }
        }
    }
}