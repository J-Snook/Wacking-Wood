using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[System.Serializable]
public class GameData
{
    public int minutes;
    public int hours;
    public int days;
    public float cashAmount;
    public float stamina;
    public float fuel;
    public int seed;

    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public SerializableDictionary<Vector2, BuildingInfomation> buildingStoredInfo;
    public SerializableDictionary<Vector2, treeInfomation> treeInfomation;

    public GameData()
    {
        System.Random r = new System.Random();
        playerPosition = Vector3.up*15f;
        playerRotation = Vector3.zero;

        minutes = 50;
        hours = 5;
        days = 0;
        cashAmount = 0f;
        stamina=100f;
        fuel=100f;
        seed = r.Next(1000000);
        buildingStoredInfo = new SerializableDictionary<Vector2, BuildingInfomation>();
        treeInfomation = new SerializableDictionary<Vector2, treeInfomation>();
    }
}

[System.Serializable]
public class treeInfomation
{
    public Vector2 coord;
    public string tag;
    public List<Vector3> positions;

    public treeInfomation(string tag, List<Vector3> positions, Vector2 coord)
    {
        this.tag = tag;
        this.positions = positions;
        this.coord = coord;
    }
}

[System.Serializable]
public class BuildingInfomation
{
    public int index;
    public Vector2Int localPos;

    public BuildingInfomation(int index, Vector2Int localPos)
    {
        this.index = index;
        this.localPos = localPos;
    }
}
