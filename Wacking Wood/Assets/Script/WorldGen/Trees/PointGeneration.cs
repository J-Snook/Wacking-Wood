using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointGeneration
{
    public static List<Vector2> GeneratePoints(Vector2 areaSize, float radius, int spawnAttempts = 30)
    {
        float gridSquareSize = radius / Mathf.Sqrt(radius);
        int[,] grid = new int[Mathf.CeilToInt(areaSize.x / gridSquareSize), Mathf.CeilToInt(areaSize.y / gridSquareSize)];
        List<Vector2> acceptedPoints = new List<Vector2>();
        List<Vector2> checkPoints = new List<Vector2>();
        checkPoints.Add(areaSize / 2);
        while(checkPoints.Count > 0)
        {
            int randomCheckPointIndex = Random.Range(0, checkPoints.Count);
            bool validPoint = false;
            for(int i = 0; i < spawnAttempts; i++)
            {
                float angle = Random.value * 2 * Mathf.PI;
                Vector2 direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 checkPosition = checkPoints[randomCheckPointIndex] + direction * Random.Range(radius, radius * 2);
                if(IsValidPoint(checkPosition, areaSize, gridSquareSize, radius, acceptedPoints, grid))
                {
                    acceptedPoints.Add(checkPosition);
                    checkPoints.Add(checkPosition);
                    grid[(int)(checkPosition.x / gridSquareSize), (int)(checkPosition.y / gridSquareSize)] = acceptedPoints.Count;
                    validPoint = true;
                    break;
                }
            }
            if(!validPoint)
            {
                checkPoints.RemoveAt(randomCheckPointIndex);
            }
        }
        return acceptedPoints;
    }

    private static bool IsValidPoint(Vector2 checkPoint, Vector2 areaSize, float gridSquareSize, float radius, List<Vector2> acceptedPoints, int[,] grid)
    {
        if(checkPoint.x >= 0 && checkPoint.x < areaSize.x && checkPoint.y >= 0 && checkPoint.y < areaSize.y)
        {
            int GridSquareX = (int)(checkPoint.x / gridSquareSize);
            int GridSquareY = (int)(checkPoint.y / gridSquareSize);
            int searchStartX = Mathf.Max(0, GridSquareX - 2);
            int searchEndX = Mathf.Min(GridSquareX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, GridSquareY - 2);
            int searchEndY = Mathf.Min(GridSquareY + 2, grid.GetLength(1) - 1);
            for(int x = searchStartX; x < searchEndX; x++)
            {
                for(int y = searchStartY; y < searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if(pointIndex != -1)
                    {
                        float sqrDst = (checkPoint - acceptedPoints[pointIndex]).sqrMagnitude;
                        if(sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }
}
