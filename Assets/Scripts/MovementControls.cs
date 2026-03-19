using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour
{
    [HideInInspector] public float speed = 5f;
    private Rigidbody rigidBody;
    private PlayerStats playerStats;
    public bool disableMovement = false;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (disableMovement) return;

        speed = playerStats.moveSpeed;

        // Get input values for movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float moveY = 0f;

        // Space moves up, Left Control moves down
        if (Input.GetKey(KeyCode.Space)) moveY = 1f;
        if (Input.GetKey(KeyCode.LeftControl)) moveY = -1f;

        // Determine movement direction relative to camera rotation
        Vector3 moveDirection = (transform.forward * moveZ + transform.right * moveX + transform.up * moveY).normalized;

        // Apply force-based movement
        Vector3 force = moveDirection * speed * Time.deltaTime;

        // Apply the force to the Rigidbody (move the parent object using physics)
        rigidBody.AddForce(force, ForceMode.VelocityChange); // Use VelocityChange for immediate movement response
    }
}
