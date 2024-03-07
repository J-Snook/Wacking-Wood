using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraHolderTransform;
    [SerializeField] private Transform characterTransform;

    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector3 playerVelocity;

    [SerializeField] private float cameraSensitivity;
    [SerializeField] private Vector3 cameraRotation;
    [SerializeField] private float cameraRotationLimit;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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

        playerVelocity *=  movementSpeed * Time.deltaTime;
        playerVelocity = characterTransform.TransformDirection(playerVelocity);

        characterController.Move(playerVelocity);
    }

    void Looking()
    {
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseMovement = mouseMovement * cameraSensitivity;
        cameraRotation.x += mouseMovement.x;
        cameraRotation.y += mouseMovement.y;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -cameraRotationLimit, cameraRotationLimit);
        var xQuat = Quaternion.AngleAxis(cameraRotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(cameraRotation.y, Vector3.left);

        cameraHolderTransform.localRotation = yQuat;
        transform.localRotation = xQuat;
    }
}
