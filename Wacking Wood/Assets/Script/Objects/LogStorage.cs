using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogStorage : MonoBehaviour, IInteractSystem
{
    private List<LogPickup> storedLogs = new List<LogPickup>();
    private PlayerHeldItem playerHeldScript;
    public string text
    {
        get 
        {
            if(playerHeldScript.isHoldingItem)
            {
                return "Press F to Place Log";
            }
            else if (storedLogs.Count > 0)
            {
                return "Press F to Pickup Log";
            } else
            {
                return string.Empty;
            }
        }
    }
    public string promptText => text;
    private void Start()
    {
        playerHeldScript = FindObjectOfType<PlayerHeldItem>();
        if (playerHeldScript == null)
        {
            Debug.Log("PlayerHeldScript not found");
        }
    }

    public void Interact(InteractionSystem player)
    {
        if (playerHeldScript.isHoldingItem)
        {
            GameObject heldItem = playerHeldScript.PlaceItem();
            if (heldItem.TryGetComponent(out LogPickup logPickup))
            {
                heldItem.transform.parent = transform;
                heldItem.transform.localPosition = Vector3.up*storedLogs.Count;
                heldItem.transform.rotation = transform.rotation;
                logPickup.logStorage = this;
                storedLogs.Add(logPickup);
            }
        } 
        else if (storedLogs.Count >0)
        {
            LogPickup topLog = storedLogs[storedLogs.Count-1];
            storedLogs.RemoveAt(storedLogs.Count - 1);
            topLog.logStorage = null;
            topLog.Interact(player);
        }
    }
}
