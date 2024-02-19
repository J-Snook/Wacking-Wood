using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogStorage : MonoBehaviour, IInteractSystem
{
    private List<GameObject> storedLogs = new List<GameObject>();
    private PlayerHeldItem playerHeldScript;
    private string text = "gsrsrhs";
    public string promptText => text;
    private void Start()
    {
        playerHeldScript = FindObjectOfType<PlayerHeldItem>();
        if (playerHeldScript == null)
        {
            Debug.Log("PlayerHeldScript not found");
        }
    }

    private void Update()
    {
        if (playerHeldScript.isHoldingItem)
        {
            text = "Press F to Place Log";
            Debug.Log("1");
} 
        else if (storedLogs.Count > 0)
        {
            text = "Press F to Pickup Log";
            Debug.Log("2");
        }
        else
        {
            text=string.Empty;
            Debug.Log("3");
        }
    }

    public void Interact(InteractionSystem player)
    {
        if (playerHeldScript.isHoldingItem)
        {
            GameObject heldItem = playerHeldScript.PlaceItem();
            if (heldItem != null)
            {
                heldItem.transform.parent = transform;
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.rotation = transform.rotation;
                storedLogs.Add(heldItem);
            }
        } 
        else if (storedLogs.Count >0)
        {
            //Pick up log out of stack
        }
    }
}
