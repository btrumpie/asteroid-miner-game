using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Range(0.1f, 9f)][HideInInspector] public float sensitivity = 2f;
    [Range(0f, 90f)][HideInInspector] public float yRotationLimit = 88f;
    public bool disableCameraMovement = false;

    private PlayerStats playerStats;
    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        sensitivity = playerStats.cameraSensitivity;
        Camera.main.fieldOfView = playerStats.fieldOfView;
    }

    void Update()
    {
        sensitivity = playerStats.cameraSensitivity;
        Camera.main.fieldOfView = playerStats.fieldOfView;
    }
    void LateUpdate()
    {
        if (disableCameraMovement) return;

        rotation.x += Input.GetAxis(xAxis) * sensitivity;
        rotation.y += Input.GetAxis(yAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
        transform.localRotation = xQuat * yQuat;

        //Lock cursor with Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Toggle cursor lock state
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
                Cursor.visible = false; // Hide the cursor
            }
            else
            {
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor
                Cursor.visible = true; // Show the cursor
            }
        }
    }
}
