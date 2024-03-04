using System.Collections.Generic;
using UnityEngine;

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
    public Quaternion playerRotation;
    public Quaternion cameraRotation;
    public SerializableDictionary<Vector2, RemovedTrees> treeCutdown;

    public GameData()
    {
        System.Random r = new System.Random();
        playerPosition = Vector3.up*15f;
        playerRotation = Quaternion.identity;
        cameraRotation = Quaternion.identity;

        minutes = 50;
        hours = 5;
        days = 0;
        cashAmount = 0f;
        stamina=100f;
        fuel=0f;
        seed = r.Next(999999999)* r.Next(999999999);
        treeCutdown = new SerializableDictionary<Vector2, RemovedTrees>();
    }
}

[System.Serializable]
public class RemovedTrees
{
    public List<Vector3> points;

    public RemovedTrees(List<Vector3> treeCutdownPoints)
    {
        this.points = treeCutdownPoints;
    }
}
