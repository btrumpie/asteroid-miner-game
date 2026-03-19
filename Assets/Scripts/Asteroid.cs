using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Asteroid : MonoBehaviour
{
    [Header("Rotation Speed")]
    public float minRotationSpeed = 5f;
    public float maxRotationSpeed = 20f;
    [Header("Scale")]
    public float minScale = 75f;
    public float maxScale = 250f;
    [Header("Movement Speed")]
    public float minMoveSpeed = 1f;
    public float maxMoveSpeed = 5f;
    [Header("Other Variables")]
    public float fadeInSpeed = 1f;
    public Gradient colorGradient;
    public float maxLifeTime = 120f;
    public float timeAlive = 0f;
    public float fadeOutSpeed = 1f;
    public int moneyGainedPerInterval = 10;

    private Vector3 rotationAxis;
    private float rotationSpeed;
    private float moveSpeed;
    private Rigidbody rb;
    private Material asteroidMaterial;
    private float currentAlpha = 0f;
    private Color targetColor;
    private PlayerStats playerStats;
    private float moneyTimer;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        playerStats = FindObjectOfType<PlayerStats>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        asteroidMaterial = meshRenderer.material;

        // Ensure that the shader supports transparency
        asteroidMaterial.SetFloat("_Mode", 3);
        asteroidMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        asteroidMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //asteroidMaterial.SetInt("_ZWrite", 0);
        asteroidMaterial.DisableKeyword("_ALPHATEST_ON");
        asteroidMaterial.EnableKeyword("_ALPHABLEND_ON");
        asteroidMaterial.renderQueue = 3000;

        // Pick a random asteroid color from the gradient
        targetColor = colorGradient.Evaluate(Random.Range(0f, 1f));

        // Set the initial color to fully transparent (alpha = 0)
        asteroidMaterial.SetColor("_Color", new Color(targetColor.r, targetColor.g, targetColor.b, currentAlpha));

        // Generate a random rotation axis
        rotationAxis = Random.onUnitSphere;

        // Set a random rotation speed within the specified range
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        // Set a random scale within the specified range
        float scale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(scale, scale, scale);

        // Set a random movement speed within the specified range
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

        // Move the asteroid along the x-axis
        rb.velocity = Vector3.right * moveSpeed;
    }

    void Update()
    {
        // Gradually increase the alpha value to make the asteroid fade in
        if (currentAlpha < 0.999f)
        {
            currentAlpha += fadeInSpeed * Time.deltaTime;
            currentAlpha = Mathf.Clamp01(currentAlpha);  // Ensure alpha doesn't go above 1
            asteroidMaterial.SetColor("_Color", new Color(targetColor.r, targetColor.g, targetColor.b, currentAlpha));
        }

        // Rotate the asteroid around the random axis
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);

        // Check if the player is close enough and looking at the asteroid
        if (IsPlayerLookingAtAsteroid())
        {
            if (Input.GetMouseButton(0))  // Left-click to start scaling down asteroid
            {
                ScaleDownAsteroid();
            }
            if (Input.GetMouseButton(1))  // Right-click to start slowing down asteroid
            {
                SlowDownAsteroid();
            }
        }

        // Update time alive
        timeAlive += Time.deltaTime;

        // Starting asteroids do not fade in
        if (Time.timeSinceLevelLoad < 0.5f)
        {
            currentAlpha = 1;
            asteroidMaterial.SetColor("_Color", new Color(targetColor.r, targetColor.g, targetColor.b, 1));
        }

        // Fade out and destroy asteroid if it has been alive too long
        if (timeAlive > maxLifeTime)
        {
            DestroyAsteroid();
        }
    }

    void DestroyAsteroid()
    {
        currentAlpha -= fadeOutSpeed * Time.deltaTime;
        currentAlpha = Mathf.Clamp01(currentAlpha);  // Ensure alpha doesn't go below 0
        asteroidMaterial.SetColor("_Color", new Color(targetColor.r, targetColor.g, targetColor.b, currentAlpha));
        if (Mathf.Approximately(currentAlpha, 0))
            Destroy(gameObject); // Destroy the asteroid once it is invisible
    }

    bool IsPlayerLookingAtAsteroid()
    {
        // Check if the player is within a certain range and looking at the asteroid using a raycast
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        if (distanceToPlayer < 30f)  // You can adjust the range
        {
            Vector3 directionToAsteroid = transform.position - playerTransform.position;
            Ray ray = new Ray(playerTransform.position, directionToAsteroid);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10f))  // Raycast from player towards asteroid
            {
                if (hit.transform == transform)  // Check if the ray hit this asteroid
                {
                    return true;  // The player is looking at the asteroid
                }
            }
        }
        return false;  // Player is not close enough or not looking at the asteroid
    }

    void ScaleDownAsteroid()
    {
        if (transform.localScale.x > 1f)
        {
            float scaleAmount = playerStats.drillSpeed * Time.deltaTime;
            transform.localScale -= new Vector3(scaleAmount, scaleAmount, scaleAmount);
            if (moneyTimer <= 0)
            {
                moneyTimer = 10f / playerStats.drillSpeed;
                playerStats.money += moneyGainedPerInterval;
            }
            else
            {
                moneyTimer -= Time.deltaTime;
            }
        }
        else
        {
            // The asteroid has been drilled
            Destroy(gameObject);
        }
    }

    void SlowDownAsteroid()
    {
        float slowAmount = playerStats.asteroidDeceleratorSpeed * Time.deltaTime;

        // Slow down asteroid with positive velocity
        if (rb.velocity.x > 0.5)
            rb.velocity -= new Vector3(slowAmount, 0, 0);
        if (rb.velocity.y > 0.5)
            rb.velocity -= new Vector3(0, slowAmount, 0);
        if (rb.velocity.z > 0.5)
            rb.velocity -= new Vector3(0, 0, slowAmount);

        // Slow down asteroid with negative velocity
        if (rb.velocity.x < -0.5)
            rb.velocity -= new Vector3(slowAmount, 0, 0);
        if (rb.velocity.y < -0.5)
            rb.velocity -= new Vector3(0, slowAmount, 0);
        if (rb.velocity.z < -0.5)
            rb.velocity -= new Vector3(0, 0, slowAmount);

        // Slow down positive rotation speed
        if (rotationSpeed > 0.01)
            rotationSpeed -= playerStats.asteroidDeceleratorSpeed * Time.deltaTime * 4;
        if (rotationSpeed < -0.01)
            rotationSpeed += playerStats.asteroidDeceleratorSpeed * Time.deltaTime * 4;

        // Slow down negative rotation speed


        // Asteroid stops moving in a direction if velocity in that direction is near zero
        if (rb.velocity.x > -0.5 && rb.velocity.x < 0.5)
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        if (rb.velocity.y > -0.5 && rb.velocity.y < 0.5)
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (rb.velocity.z > -0.5 && rb.velocity.z < 0.5)
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy any asteroids that spawned incorrectly
        if (timeAlive < 1f)
        {
            Destroy(gameObject);
        }
    }
}
