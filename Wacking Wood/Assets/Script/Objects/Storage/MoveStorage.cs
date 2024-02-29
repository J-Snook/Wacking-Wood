using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStorage : MonoBehaviour,IInteractSystem
{
    private PlayerHeldItem playerHeldScript;
    public string text
    {
        get
        {
            if (!playerHeldScript.isHoldingItem)
            {
                return "Press F to Pull Cart";
            }
            return string.Empty;
        }
    }

    public string promptText => text;

    public void Interact(InteractionSystem player)
    {
        if (!playerHeldScript.isHoldingItem)
        {
            playerHeldScript.holdItem(transform.parent.gameObject, true);
            transform.parent.localPosition = Vector3.back;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHeldScript = PlayerHeldItem.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
