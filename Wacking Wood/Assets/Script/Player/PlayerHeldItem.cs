using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHeldItem : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject _axe;
    [SerializeField] private GameObject _chainsaw;
    [SerializeField] private GameObject _fuelBar;
    [SerializeField] private ChainsawSwing css;
    [SerializeField] private AxeSwing axs;
    [SerializeField] private GameObject EB1;
    [SerializeField] private GameObject EB2;
    
    public bool isHoldingItem = false;
    private GameObject heldItem;
    public GameObject HeldItem { get { return heldItem; }}
    private bool canDrop;
    private bool canPlace;


    public void Start()
    {
        css.enabled = false;
        axs.enabled = false;
    }

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
                    axs.enabled = false;
                    EB1.SetActive(false);
                    heldItem = null;
                    isHoldingItem = false;
                }
            } else
            {
                _axe.SetActive(true);
                axs.enabled = true;
                EB1.SetActive(true);
                holdItem(_axe, false,false);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        { 
            if (heldItem != null)
            {
                if (heldItem == _chainsaw)
                {
                    _chainsaw.SetActive(false);
                    _fuelBar.SetActive(false);
                    EB2.SetActive(false);
                    css.enabled = false;
                    
                    heldItem = null;
                    isHoldingItem = false;
                }
            } else
            {
                _chainsaw.SetActive(true);
                _fuelBar.SetActive(true);
                EB2.SetActive(true);
                css.enabled = true;
                holdItem(_chainsaw, false,false);
            }
        }
    }
}