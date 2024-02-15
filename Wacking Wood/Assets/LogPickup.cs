using UnityEngine;

public class LogPickup : MonoBehaviour, IInteractSystem
{
    [SerializeField] private string text = string.Empty;
    private bool isHoldingLog = false;
    private GameObject heldLog;
    private InteractionSystem player;

    public string promptText => text;

    void Start()
    {
        player = GetComponent<InteractionSystem>();
    }

    void Update()
    {
        // Check if the player wants to drop the log
        if (isHoldingLog && Input.GetKeyDown(KeyCode.G))
        {
            DropLog();
        }
    }

    void DropLog()
    {
        if (heldLog != null)
        {
            // Set parent to null to drop the log
            heldLog.transform.SetParent(null);

            // Enable the Rigidbody component
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

    public void Interact(InteractionSystem player)
    {
        if (!isHoldingLog && Input.GetKeyDown(KeyCode.F))
        {
            // Pick up the log
            heldLog = gameObject;

            // Set the parent of the object to the player to make it follow the player
            heldLog.transform.SetParent(player.transform);

            // Reset the local position and rotation of the object
            heldLog.transform.localPosition = new Vector3(1f, 0.5f, 0f); // Right shoulder position
            heldLog.transform.localRotation = Quaternion.Euler(-90, 0, 0); // Adjust rotation to face forward

            // Disable the Rigidbody by making it kinematic and removing gravity
            Rigidbody rb = heldLog.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            isHoldingLog = true;
        }
        else if (isHoldingLog && heldLog == gameObject && Input.GetKeyDown(KeyCode.F))
        {
            // Drop the log if the player is already holding it
            DropLog();
        }
    }
}
