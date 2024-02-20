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

        private float currentCash = 0;
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
        
    public float Cash { get; private set; }

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
            currentStamina += .5f;
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
        float newbal = currentCash + change;
        if (newbal >= minCash)
        {
            currentCash = newbal;
            uiElements.UpdateCashAmount(currentCash);
            return true;
        }
        return false;
    }
}
