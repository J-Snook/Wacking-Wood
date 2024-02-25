using System.Collections.Generic;
using UnityEngine;

public class TreeGenerationMesh : MonoBehaviour
{
    private List<Vector3> treePoints;
    private ObjectPooler _pooler;
    private string treeTag;

    public void TreeGen(MeshRenderer meshRenderer, BuildingGeneration buildingScript,float density=7f,int rejectionSamples=2)
    {
        if (treePoints == null )
        {
            treePoints = new List<Vector3>();
            _pooler = ObjectPooler.Instance;
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
                //points.Remove(point);
            }
        }
        int Count = 1;
        foreach(Vector3 point in treePoints)
        {
            GameObject tree = _pooler.SpawnFromPool(treeTag, point, Quaternion.identity);
            tree.transform.parent = transform;
            tree.name = treeTag+ ":" + Count;
            Count++;
        }
    }
}