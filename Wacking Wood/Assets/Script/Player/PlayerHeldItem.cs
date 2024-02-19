using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHeldItem : MonoBehaviour
{
    public bool isHoldingItem;
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
            item.transform.parent = transform;
            Debug.Log("Item Picked Up");
            return true;
        }
        Debug.Log("Item Cant be Picked Up");
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
        Debug.Log("Item Dropped");
    }

    public GameObject PlaceItem()
    {
        if (heldItem != null && canPlace)
        {
            GameObject rtnValue = heldItem;
            heldItem=null;
            Debug.Log("Item Placed");
            return rtnValue;

        }
        Debug.Log("Item Cant be place");
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
            Debug.Log("G pressed Cant Drop");
        }
    }
}
