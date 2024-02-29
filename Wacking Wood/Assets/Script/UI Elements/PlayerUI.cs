using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour, IDataPersistance
{
    [SerializeField] private Slider fuelBar;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private TextMeshProUGUI timeOfDay;
    [SerializeField] private TextMeshProUGUI cashAmount;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private GameObject prompt;

    private GameData gameData;

    private void Start()
    {
        gameData = new GameData();
    }


    public void UpdateTime(int hour, int minute)
    {
        timeOfDay.text = $"{hour.ToString("D2")}:{minute.ToString("D2")}";

        gameData.timeOfDay = hour * 60 + minute;
    }
    
   

    public void LoadData(GameData data)
    {
        gameData = data;
        UpdateTime(gameData.timeOfDay / 60, gameData.timeOfDay % 60); // Convert total minutes back to hour and minute
        UpdateCashAmount(gameData.cashAmount);
    }

    public void SaveData(ref GameData data)
    {
        data = gameData;
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
        gameData.cashAmount = cashValue;
        cashAmount.text = cashValue.ToString("C");
        

        //string formattedCash = cashValue.ToString("C");
        //cashAmount.text = $"{formattedCash}";
    }

    public bool SetInteractionPrompt(string text)
    {
        prompt.SetActive(text!=String.Empty);
        promptText.text = text;
        return text!=String.Empty;
    }

}
