using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingGeneration: MonoBehaviour,IDataPersistance
{
    public bool hasBuilding=false;
    private int buildingIndex;
    public GameObject buildingGO;
    public BuildingInfo building;
    public Vector2Int buildingLocalPos;
    private Vector2 coord;
    private bool hasStoredInfo=false;

    public void buildingInit(Vector2 coord)
    {
        this.coord = coord;
    }

    public void selectedBuildingIndex(BuildingInfo[] buildingInfo)
    {
        if(DataPersistanceManager.instance.gameData.buildingStoredInfo.TryGetValue(coord, out BuildingInfomation storedInfo))
        {
            buildingIndex = storedInfo.index;
            building = buildingInfo[buildingIndex];
            buildingLocalPos = storedInfo.localPos;
            hasBuilding = true;
            hasStoredInfo = true;
            return;
        }
        float randomWeight = Random.Range(0f, 100f);
        int index = -1;
        for(int i = 0; i < buildingInfo.Length; i++)
        {
            if(randomWeight <= buildingInfo[i].weight) { index = i; break; }
        }
        if (index >= 0)
        {
            hasBuilding= true;
            buildingIndex = index;
            building= buildingInfo[index];
        }
    }

    public void GenerateStructurePosition()
    {
        if(hasStoredInfo) { return; }
        if (hasBuilding)
        {
            float structureDiameter = building.radius * 4f;
            int lowerBound = Mathf.CeilToInt(structureDiameter);
            int upperBound = Mathf.FloorToInt(240f - structureDiameter);
            buildingLocalPos = new Vector2Int(Random.Range(lowerBound, upperBound), Random.Range(lowerBound, upperBound));
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

    public void LoadData(GameData data)
    {
        //Ignore me loads a different way
    }

    public void SaveData(ref GameData data)
    {
        if(!hasBuilding) { return; }
        if(data.buildingStoredInfo.ContainsKey(coord))
        {
            data.buildingStoredInfo.Remove(coord);
        }
        data.buildingStoredInfo.Add(coord,new BuildingInfomation(buildingIndex,buildingLocalPos));
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
