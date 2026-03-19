using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    private Transform player;
    public float maxDistance = 100f;  // Maximum sight distance
    public LayerMask obstacleMask;  // Define what layers can block vision
    private Color targetColor; // Color we are transitioning to
    private Color currentColor; // Current color of the skybox
    public float transitionSpeedBlocked = 1f; // Speed when sun is blocked (stars appear)
    public float transitionSpeedUnblocked = 2f; // Speed when sun is unblocked (stars disappear)

    void Start()
    {
        player = Camera.main.transform;
        RenderSettings.skybox.SetColor("_Tint", Color.black); // Skybox starts at low visibility
        currentColor = RenderSettings.skybox.GetColor("_Tint"); // Get initial color
    }

    void Update()
    {
        // Gradually transition to the target color based on the visibility of the sun
        float transitionSpeed = CheckLineOfSight() ? transitionSpeedBlocked : transitionSpeedUnblocked;
        RenderSettings.skybox.SetColor("_Tint", Color.Lerp(currentColor, targetColor, Time.deltaTime * transitionSpeed));
        currentColor = RenderSettings.skybox.GetColor("_Tint"); // Update current color to new one
    }

    bool CheckLineOfSight()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        if (!Physics.Raycast(transform.position, direction, distance, obstacleMask))
        {
            targetColor = new Color(0.15f, 0.15f, 0.15f, 1f); // Bright sky, no stars
            return true;
        }
        else
        {
            targetColor = Color.gray; // Dark sky, stars visible
            return false;
        }
    }
}
