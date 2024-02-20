using UnityEngine;

public class LogPickup : MonoBehaviour, IInteractSystem
{
    [SerializeField] private Rigidbody rb;
    private MoneySystem moneySystem;
    private PlayerUI playerUI;
    private PlayerHeldItem playerHeldItem;
    public LogStorage logStorage;
    private string text
    {
        get
        {
            if (logStorage == null && !playerHeldItem.isHoldingItem)
            {
                return "Press F to Pickup Log";
            } 
            else if (logStorage != null)
            {
                return logStorage.text;
            } 
            else
            {
                return string.Empty;
            }
        }
    }

    public string promptText => text;

    void Start()
    {
        moneySystem = FindObjectOfType<MoneySystem>();
        if (moneySystem == null)
        {
            Debug.LogError("Money system not found ");
        }
        playerUI = FindObjectOfType<PlayerUI>();
        if (playerUI == null)
        {
            Debug.LogError("PlayerUI not found ");
        }
        playerHeldItem = FindObjectOfType<PlayerHeldItem>();
        if (playerHeldItem == null)
        {
            Debug.LogError("PlayerHeldItem not found ");
        }

    }


    public void Interact(InteractionSystem player)
    {
        if (logStorage== null)
        {
            if(playerHeldItem.holdItem(gameObject))
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                transform.localPosition = new Vector3(1f, 0.5f, 0f);
                transform.localRotation = Quaternion.Euler(-90, 0, 0);
            }
        } else
        {
            logStorage.Interact(player);
        }
        
    }
}
