using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour, IDataPersistance
{
    #region Variables

    [SerializeField] private float staminaRegen;
    [SerializeField] private PlayerUI uiElements;
    private float currentStamina = 100f;
    private float maxStamina = 100f;
    private float minStamina = 0f;
    private float refillTime = 3f;
    
    private float currentFuel = 0f;
    private float maxFuel = 100f;
    private float minFuel = 0f;

    private float currentCash = 0f;

    #endregion

    #region Singleton
    public static PlayerAttributes instance;
    #endregion

    #region Properties

    public float Cash { get { return currentCash; } private set { currentCash = value; } }
    public float MaxStamina { get => maxStamina; private set => maxStamina = value; }
    public float MinStamina { get => minStamina; private set => minStamina = value; }
    public float MaxFuel { get => maxFuel; private set => maxFuel = value; }
    public float MinFuel { get => minFuel; private set => minFuel = value; }

    public float Stamina
    {
        get { return currentStamina; }
        set
        {
            currentStamina = value;
            currentStamina = Mathf.Max(currentStamina, MinStamina);
            currentStamina = Mathf.Min(currentStamina, MaxStamina);
            refillTime = 3f;
            uiElements.UpdateStaminaValue(currentStamina / MaxStamina);
        }
    }

    public float Fuel
    {
        get { return currentFuel; }
        set
        {
            currentFuel = value;
            currentFuel = Mathf.Max(currentFuel,MinFuel);
            currentFuel = Mathf.Min(currentFuel, MaxFuel);
            uiElements.UpdateFuelValue(currentFuel / MaxFuel);
        }
    }

    #endregion

    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(this);
            Debug.Log("Player Attributes already exists destroying.");
        }
        instance= this;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateStamina();
    }

    private void UpdateStamina()
    {
        refillTime -= Time.deltaTime;
        if(refillTime <= 0)
        {
            currentStamina += Time.deltaTime * (staminaRegen / DayNightCycle.instance.staminaRegenReduction);
            uiElements.UpdateStaminaValue(currentStamina / MaxStamina);
        }
    }
    
    public bool UpdateCash(float change)
    {
        float newbal = Cash + change;
        if (newbal >= 0)
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
