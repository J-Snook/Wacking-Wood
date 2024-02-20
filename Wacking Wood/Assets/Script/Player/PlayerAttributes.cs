using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    #region Variables
    
        private float currentStamina = 100f;
        private float maxStamina = 100f;
        private float minStamina = 0f;
        private float refillTime = 3f;
    
        private float currentFuel = 100f;
        private float maxFuel = 100f;
        private float minFuel = 0f;

        private double currentCash = 0;
        private double minCash = 0;

        public PlayerUI uiElements;

    #endregion

    #region Properties

        public float Stamina
        {
            get => currentStamina;
            set => currentStamina = value;
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
        
        public double Cash
        {
            get => currentCash;
            set => currentCash = value;
        }

    #endregion
    

    // Update is called once per frame
    void Update()
    {
        UpdateStamina();
        UpdateFuel();
        UpdateCash();
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
            currentStamina += 1f;
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
    
    private void UpdateCash()
    {
        if (currentCash < minCash)
        {
            currentCash = minCash;
        }
        
        uiElements.UpdateCashAmount(currentCash);
    }
}
