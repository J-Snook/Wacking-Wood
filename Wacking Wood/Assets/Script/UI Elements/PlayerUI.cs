using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider fuelBar;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private TextMeshProUGUI timeOfDay;
    [SerializeField] private TextMeshProUGUI cashAmount;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private GameObject prompt;

    public void UpdateTime(int hour, int minute)
    {
        timeOfDay.text = $"{hour.ToString("D2")}:{minute.ToString("D2")}";
    }
    
    
    public void UpdateFuelValue(float fuelValue)
    {
        fuelBar.value = fuelValue;
    }
    
    
    public void UpdateStaminaValue(float staminaValue)
    {
        staminaBar.value = staminaValue;
    }

    
    public void UpdateCashAmount(float cashValue)
    {
       
        
        cashAmount.text = $"{cashValue:0.00}";
    }

    public bool SetInteractionPrompt(string text)
    {
        prompt.SetActive(text!=String.Empty);
        promptText.text = text;
        return text!=String.Empty;
    }

}
