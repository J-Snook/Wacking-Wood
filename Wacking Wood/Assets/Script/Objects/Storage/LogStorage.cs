using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogStorage : MonoBehaviour, IInteractSystem
{
    [SerializeField] private List<LogStoragePositions> logPositions;
    public List<LogPickup> storedLogs = new List<LogPickup>();
    private PlayerHeldItem playerHeldScript;
    public string text
    {
        get 
        {
            if(playerHeldScript.isHoldingItem && storedLogs.Count >= logPositions.Count)
            {
                return "Storage Full";
            }
            else if (playerHeldScript.isHoldingItem)
            {
                return "Press F to Place Log";
            }
            else if (storedLogs.Count > 0)
            {
                return "Press F to Pickup Log";
            }
            else
            {
                return string.Empty;
            }
        }
    }
    public string promptText => text;
    private void Start()
    {
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), PlayerAttributes.instance.gameObject.GetComponent<Collider>(), true);
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
            if (storedLogs.Count < logPositions.Count)
            {
                GameObject heldItem = playerHeldScript.HeldItem;
                if (heldItem.TryGetComponent(out LogPickup logPickup))
                {
                    if (playerHeldScript.PlaceItem())
                    {
                        int index = storedLogs.Count;
                        heldItem.transform.parent = transform;
                        heldItem.transform.localPosition = logPositions[index].pos; 
                        heldItem.transform.localRotation = Quaternion.Euler(logPositions[index].rot);
                        //heldItem.transform.localScale = Vector3.one;//new Vector3(0.00589783303f, 0.111021027f, 0.00270915194f);
                        logPickup.logStorage = this;
                        storedLogs.Add(logPickup);
                    }
                }
            }
        } 
        else if (storedLogs.Count > 0)
        {
            LogPickup topLog = storedLogs[^1];
            storedLogs.RemoveAt(storedLogs.Count - 1);
            topLog.logStorage = null;
            topLog.Interact(player);
        }
    }
}

[System.Serializable]
public class LogStoragePositions
{
    public Vector3 pos;
    public Vector3 rot;
}