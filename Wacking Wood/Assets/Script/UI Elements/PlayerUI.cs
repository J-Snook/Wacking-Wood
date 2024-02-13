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

    public bool SetInteractionPrompt(string text)
    {
        prompt.SetActive(text!=String.Empty);
        promptText.text = text;
        return text!=String.Empty;
    }


}
