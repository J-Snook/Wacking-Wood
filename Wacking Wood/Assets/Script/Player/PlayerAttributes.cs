using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour, IDataPersistance
{
    #region Variables
    
        private float currentStamina = 100f;
        private float maxStamina = 100f;
        private float minStamina = 0f;
        private float refillTime = 3f;
    
        private float currentFuel = 100f;
        private float maxFuel = 100f;
        private float minFuel = 0f;

        private float currentCash = 0f;
        private float minCash = 0;

        public PlayerUI uiElements;

    #endregion

    #region Properties

    public float Stamina
    {
        get { return currentStamina; }
        set
        {
            currentStamina = value;
            refillTime = 3f;
        }
    }

    public float RefillTime
    {
        get => refillTime;
        set => refillTime = value;
    }
        
    public float Fuel
    {
        get => currentFuel;
        set => currentFuel = value;
    }
        
    public float Cash { get { return currentCash; } private set { currentCash = value; } }

    #endregion
    

    // Update is called once per frame
    void Update()
    {
        UpdateStamina();
        UpdateFuel();
    }

    private void UpdateStamina()
    {
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        
        if (currentStamina < minStamina)
        {
            currentStamina = minStamina;
        }
        
        refillTime -= Time.deltaTime;

        if (refillTime <= 0)
        {
            currentStamina += 15f*Time.deltaTime;
        }
        
        uiElements.UpdateStaminaValue(currentStamina/maxStamina);
    }
    
    private void UpdateFuel()
    {
        if (currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
        
        if (currentFuel < minFuel)
        {
            currentFuel = minFuel;
        }
        
        uiElements.UpdateFuelValue(currentFuel/maxFuel);
    }
    
    public bool UpdateCash(float change)
    {
        float newbal = Cash + change;
        if (newbal >= minCash)
        {
            Cash = newbal;
            uiElements.UpdateCashAmount(currentCash);
            return true;
        }
        return false;
    }

    public void LoadData(GameData data)
    {
        Cash = data.cashAmount;
        Fuel = data.fuel;
        Stamina = data.stamina;
        uiElements.UpdateCashAmount(Cash);
        uiElements.UpdateFuelValue(Fuel);
        uiElements.UpdateStaminaValue(Stamina);
    }

    public void SaveData(ref GameData data)
    {
        data.cashAmount = Cash;
        data.fuel = Fuel;
        data.stamina = Stamina;
    }
}
