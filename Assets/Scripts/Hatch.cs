using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hatch : MonoBehaviour
{
    public Text crosshair;
    public float activationDistance = 5f;
    public Transform destination;
    public GameObject shipInterior;
    public GameObject shipExterior;
    public bool goesIndoors = false;
    private Transform player;
    private bool isPlayerNear = false;

    void Start()
    {
        player = Camera.main.transform;  // Get the player's transform (camera position)
    }

    void LateUpdate()
    {
        // Check if the player is near the door
        CheckPlayerDistance();

        // Check if the player is hovering over the door and presses 'E'
        if (isPlayerNear && IsMouseOverDoor())
        {
            crosshair.text = "[E] Interact";

            if (Input.GetKeyDown(KeyCode.E))
            {
                TeleportPlayer();
                Invoke(nameof(TeleportPlayer), 0.02f);
                Invoke(nameof(TeleportPlayer), 0.04f);
                Invoke(nameof(TeleportPlayer), 0.06f);
            }
        }
        else if (goesIndoors)
        {
            crosshair.text = "+";
        }
    }

    // Check if the player is within the activation distance
    void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        isPlayerNear = distance <= activationDistance;
    }

    // Check if the player is hovering their crosshair over the door
    bool IsMouseOverDoor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == gameObject;
        }

        return false;
    }

    void TeleportPlayer()
    {
        // Teleport the player to the desired location
        crosshair.text = "+";
        player.transform.position = destination.position;
        player.transform.rotation = destination.rotation;

        // Make the ship interior invisible when player is in space
        if (goesIndoors)
        {
            shipInterior.SetActive(true);
            shipExterior.SetActive(false);
            // Destroy all Watchers
            foreach (Watcher watcher in FindObjectsOfType<Watcher>())
            {
                Destroy(watcher.gameObject);
            }
        }
        else
        {
            shipInterior.SetActive(false);
            shipExterior.SetActive(true);
        }
    }
}
