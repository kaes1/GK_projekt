using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    //Player's camera.
    [SerializeField] private Camera PlayerCamera;

    [SerializeField] private float mouseSensitivity;

    private float xAxisClamp;

    private void Awake()
    {
        //Lock cursor.
        Cursor.lockState = CursorLockMode.Locked;
        xAxisClamp = 0;
    }

    private void Update()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; 

        xAxisClamp += mouseY;

        if (xAxisClamp >= 90.0)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp <= -90.0)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        //For up-down rotate the camera.
        PlayerCamera.transform.Rotate(-Vector3.right * mouseY);
        //For left-right rotate the body.
        this.transform.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
