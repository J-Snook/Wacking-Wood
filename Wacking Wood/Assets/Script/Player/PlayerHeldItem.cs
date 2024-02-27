using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class PlayerHeldItem : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject _axe;
    public bool isHoldingItem = false;
    private GameObject heldItem;
    public GameObject HeldItem { get { return heldItem; }}
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
            item.layer = LayerMask.NameToLayer("HeldItems");
            item.transform.parent = _camera;
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
        heldItem.layer = LayerMask.NameToLayer("Default");
        ObjectManagement.Instance.attachObject(heldItem);
        heldItem = null;
        isHoldingItem = false;
    }

    public bool PlaceItem()
    {
        if (heldItem != null && canPlace)
        {
            GameObject rtnValue = heldItem;
            heldItem.layer = LayerMask.NameToLayer("Default");
            heldItem =null;
            isHoldingItem = false;
            return true;
        }
        return false;
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
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        { 
            if (heldItem != null)
            {
                if (heldItem == _axe)
                {
                    _axe.SetActive(false);
                    heldItem = null;
                    isHoldingItem = false;
                }
            } else
            {
                _axe.SetActive(true);
                holdItem(_axe, false,false);
            }
        }
    }
}