using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal.Internal;

public class Terminal : MonoBehaviour
{
    private Transform playerCamera;
    private bool inUse = false;
    private MovementControls movementControls;
    private CameraControls cameraControls;

    public GameObject upgradesCanvas;
    public GameObject gameCanvas;
    public Text crosshair;
    public float interactionDistance = 3f;

    void Start()
    {
        playerCamera = Camera.main.transform;
        movementControls = Camera.main.GetComponent<MovementControls>();
        cameraControls = Camera.main.GetComponent<CameraControls>();
    }

    void Update()
    {
        if (inUse && Input.GetKeyDown(KeyCode.E))
        {
            upgradesCanvas.SetActive(false);
            movementControls.disableMovement = false;
            cameraControls.disableCameraMovement = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inUse = false;
            return;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                crosshair.text = "[E] Interact";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    inUse = true;
                    upgradesCanvas.SetActive(true);
                    movementControls.disableMovement = true;
                    cameraControls.disableCameraMovement = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }
        else
        {
            crosshair.text = "+";
        }
    }
}
