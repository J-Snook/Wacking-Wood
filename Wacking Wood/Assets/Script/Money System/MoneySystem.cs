using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MoneySystem : MonoBehaviour
{
    private float money = 100.00f;

    public event Action<double> OnMoneyChanged; 

    public void AddMoney(int amount)
    {
        //this will increase our $-value as we sell something
        money += amount;
        

    }

    public void TakeMoney(int amount)
    {
        if (money >= amount)
        {
            // this will just decrese our $-value as we buy something
            money -= amount;

        }
        else
        {
            Debug.LogWarning("Not enough money to take " + amount);
        }
    }

    public float GetMoney()
    {
        return money;
    }




}
