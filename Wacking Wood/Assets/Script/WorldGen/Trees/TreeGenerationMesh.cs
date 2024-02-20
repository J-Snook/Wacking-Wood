using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public enum SelectedTree
{
    Random,
    Tree1,
    Tree2,
    Tree3,
    Tree4
}


public class TreeGenerationMesh : MonoBehaviour
{
    public GameObject[] trees;
    public SelectedTree _treeType;
    public float density = 1;
    public int rejectionSamples = 30;
    public float sphereRadius = 1;
    private MeshRenderer _mesh;

    List<Vector2> points;
    private GameObject _treePrefab;

    public Vector3 TreeGen(Vector2Int structLoc,float structRadius)
    {
        if (_treeType== SelectedTree.Random)
        {
            _treePrefab = trees[Random.Range(0, trees.Length)];
        } else
        {
            _treePrefab = trees[(int)_treeType - 1];
        }

        _mesh = GetComponent<MeshRenderer>();
        Vector3 boundsSize = _mesh.bounds.size;
        Vector2 regionSize = new Vector2(boundsSize.x, boundsSize.z);
        points = PointGeneration.GeneratePoints(density, regionSize, rejectionSamples);
        Vector3 _structPos = new Vector3(structLoc.x, _mesh.bounds.max.y, 240f - structLoc.y);
        _structPos = _structPos + _mesh.bounds.min;
        if(points != null)
        {
            int Count = 1;
            foreach(Vector2 point in points)
            {
                Vector3 _spherePos = new Vector3(point.x, _mesh.bounds.max.y, point.y);
                _spherePos = _spherePos + _mesh.bounds.min;
                float dist = _mesh.bounds.size.y;
                if (Vector3.Distance(_structPos, _spherePos) > structRadius*1.25f)
                {
                    if(Physics.Raycast(_spherePos, Vector3.down, out RaycastHit hit))
                    {
                        _spherePos.y = hit.point.y;
                        GameObject tree = Instantiate(_treePrefab, _spherePos, Quaternion.identity);
                        tree.transform.parent = transform;
                        tree.name = "Tree " + Count;
                        Count++;
                    }
                }
            }
        }
        return _structPos;
    }
}