using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MoneySystem : MonoBehaviour
{
    private float _money = 100.00f;
    public float Money { get; private set; }
    private PlayerUI playerUIscript;

    public bool AdjustMoney(float change)
    {
        float newbal = _money + change;
        if (newbal > 0)
        {
            _money = newbal;
            playerUIscript.UpdateCashAmount(_money);
            return true;
        }
        return false;
    }

    private void Start()
    {
        playerUIscript = FindObjectOfType<PlayerUI>();
        if (playerUIscript == null )
        {
            Debug.Log("Player UI Script Not Found");
        }
    }
}
