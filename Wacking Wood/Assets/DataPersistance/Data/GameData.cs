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
    public int timeOfDay; // Represented in total minutes
    public float cashAmount;

    public Vector3 playerPosition;
    public Vector3 playerRotation;

   

    public GameData()
    {
        playerPosition = Vector3.zero;
        playerRotation = Vector3.zero;

        timeOfDay = 0;
        cashAmount = 0f;
    }

}
