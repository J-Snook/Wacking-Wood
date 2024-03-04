using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPointGen : MonoBehaviour
{
    private List<Vector2> points = new List<Vector2>();
    public Vector2 area;
    public float radius;
    public int numBeforeRejection;
    public float sphereRadius;

    private void OnValidate()
    {
        points = PointGeneration.GeneratePoints(Random.Range(0, 1000000) * Random.Range(0, 1000000), radius, area, numBeforeRejection);
    }

    private void OnDrawGizmos()
    {
        Vector3 area3d = new Vector3(area.x, area.y, 0);
        Gizmos.DrawWireCube(area3d/2, area3d);
        Gizmos.color = Color.red;
        if(points.Count >0)
        {
            for(int i = 0; i < points.Count; i++)
            {
                Vector3 pos = new Vector3(points[i].x, points[i].y, 0);
                Gizmos.DrawSphere(pos, sphereRadius);
            }
        }
    }
}
