using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider fuelBar;
    public Slider staminaBar;
    public TextMeshProUGUI timeOfDay;
    public TextMeshProUGUI cashAmount;
    
    
    public void Start()
    {
        InvokeRepeating("UpdateTime", 0f, 1f); // Update every second
    }


    public void UpdateTime()
    {
        DateTime now = DateTime.Now;
        string timeString = now.ToString("HH:mm"); 
        timeOfDay.text = timeString;
    }
    
    
    public void UpdateFuelValue(float fuelValue)
    {
        fuelBar.value = fuelValue;
    }
    
    
    public void UpdateStaminaValue(float staminaValue)
    {
        staminaBar.value = staminaValue;
    }

    
    public void UpdateCashAmount(double cashValue)
    {
        string formattedCash = cashValue.ToString("C");
        
        cashAmount.text = $"{formattedCash}";
    }

}
