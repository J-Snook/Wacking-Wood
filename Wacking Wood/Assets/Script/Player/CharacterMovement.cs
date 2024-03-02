using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour, IDataPersistance
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Transform cameraHolderTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform jumpDetectTransform;
    [SerializeField] private float jumpDetectDistance;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintChange;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityForce;
    [SerializeField] private Vector2 cameraSensitivity;
    [SerializeField] private float cameraRotationLimit;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private float ySpecificVelocity;
    [SerializeField] private LayerMask notThisLayerMask;
    [SerializeField] private bool isHeadBobEnabled;
    [SerializeField] private Vector3 cameraStartLocalPosition;
    [SerializeField] private float headBobAmplitude;
    [SerializeField] private float headBobFrequency;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float sprintStaminaReduction;
    private Vector2 cameraRotation=Vector2.zero;
    private PlayerAttributes playerAttributes;
    private bool hadSpawnDelay=false;

    void Start()
    {
        cameraStartLocalPosition = cameraTransform.localPosition;
        playerAttributes = PlayerAttributes.instance;
        StartCoroutine(SpawnDelay());
    }

    IEnumerator SpawnDelay()
    {
        while (!hadSpawnDelay)
        {
            if (Physics.Raycast(jumpDetectTransform.position, Vector3.down))
            {
                hadSpawnDelay = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Update()
    {
        if(!hadSpawnDelay) { return; }
        Movement();
        Looking();
        FellThroughWorldCheck();
    }

    private void FellThroughWorldCheck()
    {
        if (transform.position.y <= -100)
        {
            transform.position = new Vector3(transform.position.x,50f,transform.position.z);
        }
    }

    public void LoadData(GameData data)
    {
        transform.position = data.playerPosition;
        transform.localRotation = data.playerRotation;
        cameraHolderTransform.localRotation = data.cameraRotation;
    }

    public void SaveData(ref GameData data)
    {
        data.playerRotation = transform.localRotation;
        data.playerPosition = transform.position;
        data.cameraRotation = cameraHolderTransform.localRotation;
    }

    void Movement()
    {
        playerVelocity.z = Input.GetAxis("Vertical");
        playerVelocity.x = Input.GetAxis("Horizontal");

        RaycastHit hit;
        if (Physics.Raycast(jumpDetectTransform.position, -Vector3.up, out hit, jumpDetectDistance, notThisLayerMask))
        {
            ySpecificVelocity = 0.0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ySpecificVelocity += jumpForce;
            }
        }
        else
        {
            ySpecificVelocity += gravityForce * Time.deltaTime;
        }

        playerVelocity.y = ySpecificVelocity;
        if (Input.GetKey(KeyCode.LeftShift) && playerAttributes.Stamina > 0f)
        {
            playerVelocity *= sprintChange;
            playerAttributes.Stamina -= Time.deltaTime * sprintStaminaReduction;
        }
        playerVelocity *=  movementSpeed * Time.deltaTime;
        playerVelocity = characterTransform.TransformDirection(playerVelocity);

        characterController.Move(playerVelocity);
    }

    void Looking()
    {
        if (PauseMenu.pausedGame) {return;}
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseMovement = mouseMovement * cameraSensitivity;
        cameraRotation.x += mouseMovement.x;
        cameraRotation.y += mouseMovement.y;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -cameraRotationLimit, cameraRotationLimit);
        var xQuat = Quaternion.AngleAxis(cameraRotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(cameraRotation.y, Vector3.left);

        cameraHolderTransform.localRotation = yQuat;
        transform.localRotation = xQuat;
        if (isHeadBobEnabled && new Vector2(playerVelocity.x, playerVelocity.z).magnitude != 0)
        {
            Vector3 headBobber = new Vector3(0f, 0f, 0f);
            headBobber.y += Mathf.Sin(Time.time * headBobFrequency);
            headBobber *= headBobAmplitude * Time.deltaTime;
            cameraTransform.localPosition += headBobber;
        }
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraStartLocalPosition, Time.deltaTime * returnSpeed);
        
    }
}
