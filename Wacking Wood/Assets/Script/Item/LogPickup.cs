using UnityEngine;

public class LogPickup : MonoBehaviour, IInteractSystem
{
    [SerializeField] private string text = string.Empty;
    private static bool isHoldingLog = false; 
    private GameObject heldLog;

    public string promptText => text;

    void Start()
    {

    }

    void Update()
    {
        // This checks if player want to drop the log, if pressed G it activates DropLog func
        if (isHoldingLog && Input.GetKeyDown(KeyCode.G))
        {
            DropLog();
        }
    }

    public void Interact(InteractionSystem player)
    {
        if (!isHoldingLog)
        {
            
            if (heldLog == null)
            {
                
                heldLog = gameObject;

                // Log becomes child of player, so we can walk with the log around
                heldLog.transform.SetParent(player.transform);

                // transforming and rotating position so its on right shoulder
                heldLog.transform.localPosition = new Vector3(1f, 0.5f, 0f); 
                heldLog.transform.localRotation = Quaternion.Euler(-90, 0, 0); 

                // Disabling RB 
                Rigidbody rb = heldLog.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                isHoldingLog = true;
            }
        }
        
    }

    void DropLog()
    {
        if (heldLog != null)
        {
            
            heldLog.transform.SetParent(null);

            // Enable RB
            Rigidbody rb = heldLog.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            isHoldingLog = false;
            heldLog = null;
        }
    }
}
