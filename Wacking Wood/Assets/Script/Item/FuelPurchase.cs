using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPurchase : MonoBehaviour,IInteractSystem
{
    [SerializeField] private float addFuelAmount;
    [SerializeField] private float fuelCost;
    private PlayerAttributes _player;
    private string text
    {
        get
        {
            if(_player.Cash < fuelCost)
            {
                return "Sorry you dont have enough money.";
            }
            if(_player.Fuel<_player.MaxFuel)
            {
                return "Press F to Purchase Fuel";
            }
            return string.Empty;
        }
    }

    public string promptText => text;

    public void Interact(InteractionSystem player)
    {
        if (_player.Fuel< _player.MaxFuel)
        {
            if (_player.UpdateCash(-fuelCost))
            {
                _player.Fuel += addFuelAmount;
            }
        }
    }

    void Start()
    {
        _player = PlayerAttributes.instance;
    }
}
