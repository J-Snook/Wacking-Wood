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
    public Dictionary<Vector2, int> buildingTypes;
    public Dictionary<Vector2, string> treeStoreTags;
    public Dictionary<Vector2, Vector2Int> buildingPos;
    public Dictionary<Vector2, List<Vector3>> treeStorePoints;

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
        buildingPos = new Dictionary<Vector2, Vector2Int>();
        buildingTypes = new Dictionary<Vector2, int>();
        treeStoreTags = new Dictionary<Vector2, string>();
        treeStorePoints = new Dictionary<Vector2, List<Vector3>>();
    }

}
