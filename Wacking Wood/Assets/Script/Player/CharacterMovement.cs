using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Transform cameraHolderTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform jumpDetectTransform;
    [SerializeField] private float jumpDetectDistance;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityForce;
    [SerializeField] private Vector2 cameraSensitivity;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private float ySpecificVelocity;
    [SerializeField] private LayerMask notThisLayerMask;
    [SerializeField] private bool isHeadBobEnabled;
    [SerializeField] private Vector3 cameraStartLocalPosition;
    [SerializeField] private float headBobAmplitude;
    [SerializeField] private float headBobFrequency;
    [SerializeField] private float returnSpeed;

    void Start()
    {
        cameraStartLocalPosition = cameraTransform.localPosition;
    }

    void Update()
    {
        Movement();
        Looking();
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
        playerVelocity *= (Input.GetKey(KeyCode.LeftShift)) ? movementSpeed * 1.5f * Time.deltaTime : movementSpeed * Time.deltaTime;
        playerVelocity = characterTransform.TransformDirection(playerVelocity);

        characterController.Move(playerVelocity);
    }

    void Looking()
    {
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        mouseMovement *= cameraSensitivity;

        characterTransform.Rotate(0f, mouseMovement.x, 0.0f);
        cameraHolderTransform.Rotate(-mouseMovement.y, 0.0f, 0.0f);

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
