using UnityEngine;

public class BuildingGeneration: MonoBehaviour
{
    public bool hasBuilding=false;
    public GameObject buildingGO;
    public BuildingInfo building;
    public Vector2Int buildingLocalPos;
    private System.Random prng;

    public void buildingInit(Vector2 coord)
    {
        int seed = DataPersistanceManager.instance.gameData.seed;
        int x = (int)coord.x;
        int y = (int)coord.y;
        x = (x >= 0) ? 2 * x : -2 * (x - 1);
        y = (y >= 0) ? 2 * y : -2 * (y - 1);
        int a = (x >= y) ? (x * x) + x + y : x + (y * y);
        seed = seed * Mathf.RoundToInt(a);
        prng = new System.Random(seed);
    }

    public void selectedBuildingIndex(BuildingInfo[] buildingInfo)
    {
        float randomWeight = (float)prng.NextDouble()*100f;
        int index = -1;
        for(int i = 0; i < buildingInfo.Length; i++)
        {
            if(randomWeight <= buildingInfo[i].weight) { index = i; break; }
        }
        if (index >= 0)
        {
            hasBuilding= true;
            building= buildingInfo[index];
        }
    }

    public void GenerateStructurePosition()
    {
        if (hasBuilding)
        {
            float structureDiameter = building.radius * 4f;
            int lowerBound = Mathf.CeilToInt(structureDiameter);
            int upperBound = Mathf.FloorToInt(240f - structureDiameter);
            buildingLocalPos = new Vector2Int(prng.Next(lowerBound, upperBound), prng.Next(lowerBound, upperBound));
        }
    }

    public void BuildStructure(MeshCollider meshCollider)
    {
        if (hasBuilding)
        {
            Vector3 structPos = new Vector3(buildingLocalPos.x,meshCollider.bounds.max.y,240f-buildingLocalPos.y) + meshCollider.bounds.min;
            if(Physics.Raycast(structPos,Vector3.down,out RaycastHit hit)) {
                structPos = new Vector3(structPos.x, hit.point.y, structPos.z);
                buildingGO = Instantiate(building.prefab, structPos,Quaternion.identity);
                buildingGO.transform.parent = meshCollider.transform;
                buildingGO.name = building.name;
            }
        }
    }
}

[System.Serializable]
public struct BuildingInfo
{
    public string name;
    public GameObject prefab;
    public float weight;
    public float radius;
}
