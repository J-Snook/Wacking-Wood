using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class PlayerHeldItem : MonoBehaviour
{
    #region Singleton
    public static PlayerHeldItem Instance;
    #endregion

    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject _fuelBar;
    [SerializeField] private ChainsawSwing chainsawScript;
    [SerializeField] private AxeSwing axeScript;
    [SerializeField] private GameObject EB1;
    [SerializeField] private GameObject EB2;
    
    public bool isHoldingItem = false;
    private GameObject heldItem;
    public GameObject HeldItem { get { return heldItem; }}
    private bool canDrop;
    private bool canPlace;

    private void Awake()
    {
        Instance= this;
    }
    
    public void Start()
    {
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

        if (heldItem.TryGetComponent(out Rigidbody rb))
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
                if (heldItem == axeScript.axe)
                {
                    axeScript.axe.SetActive(false);
                    EB1.SetActive(false);
                    heldItem = null;
                    isHoldingItem = false;
                }
            } else
            {
                axeScript.axe.SetActive(true);
                EB1.SetActive(true);
                holdItem(axeScript.axe, false,false);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        { 
            if (heldItem != null)
            {
                if (heldItem == chainsawScript.CS)
                {
                    chainsawScript.CS.SetActive(false);
                    _fuelBar.SetActive(false);
                    EB2.SetActive(false);
                    
                    heldItem = null;
                    isHoldingItem = false;
                }
            } else
            {
                chainsawScript.CS.SetActive(true);
                _fuelBar.SetActive(true);
                PlayerAttributes.instance.Fuel = PlayerAttributes.instance.Fuel;
                EB2.SetActive(true);
                holdItem(chainsawScript.CS, false,false);
            }
        }
    }
}