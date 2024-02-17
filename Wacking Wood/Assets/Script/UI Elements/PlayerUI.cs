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


    private MoneySystem moneySystem;

    public void Start()
    {
        InvokeRepeating("UpdateTime", 0f, 1f); // Update every second

        // find money system
        moneySystem = FindObjectOfType<MoneySystem>();

        if (moneySystem == null)
        {
            Debug.LogError("MoneySystem not found in the scene. ");
            return;
        }

        // money change event
        moneySystem.OnMoneyChanged += UpdateCashAmount;
    }

    private void OnDestroy()
    {
        if (moneySystem != null)
        {
            moneySystem.OnMoneyChanged -= UpdateCashAmount;
        }
       
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

    // method that will deduct amount from our purchase
    public void DeductCashAmount(double amount)
    {
        if (moneySystem != null)
        {
            moneySystem.TakeMoney((int)amount); 
        }
        else
        {
            Debug.LogError("Money system reference is null");
        }
        
    }

}
