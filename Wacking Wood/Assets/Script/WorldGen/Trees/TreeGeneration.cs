using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGeneration : MonoBehaviour
{

    public float density = 1;
    public int rejectionSamples = 30;
    public float sphereRadius = 1;
    private Terrain _terrain;

    List<Vector2> points;

    void OnValidate()
    {
        _terrain = GetComponent<Terrain>();
        Vector3 boundsSize = _terrain.terrainData.bounds.size;
        Vector2 regionSize = new Vector2(boundsSize.x, boundsSize.z);
        points = PointGeneration.GeneratePoints(density, regionSize, rejectionSamples);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_terrain.terrainData.bounds.center, sphereRadius);
        Gizmos.color = Color.red;
        if(points != null)
        {
            foreach(Vector2 point in points)
            {
                Vector3 _spherePos = new Vector3(point.x, 0, point.y);
                _spherePos = _spherePos + _terrain.terrainData.bounds.min;
                _spherePos.y = _terrain.SampleHeight(_spherePos) + sphereRadius;
                Gizmos.DrawSphere(_spherePos, sphereRadius);
            }
        }
    }
}