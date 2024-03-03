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
            playerHeldScript.holdItem(transform.parent.parent.gameObject, true);
            transform.parent.parent.parent = player.transform;
            transform.parent.parent.localPosition = new Vector3(-1.45f, -0.9f, -6.6f);
            transform.parent.parent.rotation = player.transform.rotation;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHeldScript = PlayerHeldItem.Instance;
    }
}
