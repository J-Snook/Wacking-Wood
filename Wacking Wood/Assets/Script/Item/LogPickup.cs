using UnityEngine;

public class LogPickup : MonoBehaviour, IInteractSystem
{
    [SerializeField] private string text = string.Empty;
    [SerializeField] private Rigidbody rb;
    private MoneySystem moneySystem;
    private PlayerUI playerUI;
    private PlayerHeldItem playerHeldItem;
    

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
        if (playerHeldItem.holdItem(gameObject))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            transform.localPosition = new Vector3(1f, 0.5f, 0f);
            transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
        
    }
}
