using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHeldItem : MonoBehaviour
{
    public bool isHoldingItem = false;
    private GameObject heldItem;
    private bool canDrop;
    private bool canPlace;

    public bool holdItem(GameObject item, bool itemDroppable=true, bool itemPlaceable=true)
    {
        if (heldItem == null)
        {
            heldItem = item;
            canDrop = itemDroppable;
            canPlace = itemPlaceable;
            isHoldingItem = true;
            item.transform.parent = transform;
            return true;
        }
        return false;
    }
    
    private void DropItem()
    {
        heldItem.transform.parent = null;

        // Enable RB
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        heldItem = null;
        isHoldingItem = false;
    }

    public GameObject PlaceItem()
    {
        if (heldItem != null && canPlace)
        {
            GameObject rtnValue = heldItem;
            heldItem=null;
            isHoldingItem = false;
            return rtnValue;

        }
        return null;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (heldItem != null && canDrop)
            {
                DropItem();
            }
        }
    }
}
