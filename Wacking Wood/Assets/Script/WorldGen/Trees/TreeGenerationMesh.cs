using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeGenerationMesh : MonoBehaviour
{
    private List<Vector3> treePoints;
    private List<GameObject> treeObjects = new List<GameObject>();
    private ObjectPooler _pooler;
    private string treeTag;
    private int seed;
    private System.Random prng;

    private void Awake()
    {
        _pooler = ObjectPooler.Instance;
        
    }

    public void treeInit(Vector2 coord)
    {
        seed = DataPersistanceManager.instance.gameData.seed;
        int x = (int)coord.x;
        int y = (int)coord.y;
        x = (x >= 0) ? 2 * x : -2 * (x - 1);
        y = (y >= 0) ? 2 * y : -2 * (y - 1);
        int a = (x >= y) ? (x * x) + x + y : x + (y * y); // Generates a Roughly unique number based on x and y except for when x||y ==0
        seed = seed * Mathf.RoundToInt(a);
        prng = new System.Random(seed);
    }

    public void TreeGen(MeshRenderer meshRenderer, BuildingGeneration buildingScript,float density=7f,int rejectionSamples=2)
    {
        if (treeObjects.Count >0) { return; }
        if (treePoints == null )
        {
            treePoints = new List<Vector3>();
            treeTag = "tree" + prng.Next(1, 4).ToString();
            Bounds _meshBounds = meshRenderer.bounds;
            Vector2 regionSize = new Vector2(_meshBounds.size.x, _meshBounds.size.z);
            List<Vector2> points = PointGeneration.GeneratePoints(seed,density, regionSize, rejectionSamples);
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
        int Count = 1;
        foreach(Vector3 point in treePoints)
        {
            GameObject tree = _pooler.SpawnFromPool(treeTag, point, Quaternion.identity);
            tree.transform.parent = transform;
            tree.transform.GetComponentInChildren<TreeHit>().initPoint = point;
            tree.name = treeTag+ ":" + Count;
            treeObjects.Add(tree);
            Count++;
        }
    }

    public void RemovePoint(Vector3 point)
    {
        if(treePoints.Contains(point))
        {
            treePoints.Remove(point);
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
}