using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Watcher : MonoBehaviour
{
    public float sightRange = 200f;
    public float angerAcceleration = 2f;
    public float timeUntilAttack = 2f;
    public float stoppingDistance = 15f;
    public float moveSpeed = 3f;
    public float attackModeSpeedMultiplier = 3f;
    public float timeBetweenHits = 0.5f;
    public int damage = 10;
    public float maxLifetime = 30f;

    private Transform player;
    private Camera playerCamera;
    private Light leftEyeLight;
    private Light rightEyeLight;
    private Renderer leftEye;
    private Renderer rightEye;
    private Renderer[] renderers;
    private float viewTimer = 0f;
    private float viewTimerPercentage;
    private bool hasAttacked = false;
    private float damageTimer;
    private Health playerHealth;
    private float timeAlive = 0;

    void Start()
    {
        damageTimer = timeBetweenHits;
        playerCamera = Camera.main;
        player = Camera.main.transform;
        leftEyeLight = GetComponentsInChildren<Light>()[0];
        rightEyeLight = GetComponentsInChildren<Light>()[1];
        renderers = GetComponentsInChildren<Renderer>();

        leftEye = renderers[1];
        rightEye = renderers[3];
    }

    void Update()
    {
        viewTimerPercentage = viewTimer / timeUntilAttack;
        Vector3 lookDirection = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = targetRotation * Quaternion.Euler(0, -90, 0);

        if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }

        if (hasAttacked) return;

        if (!IsInViewOfCamera() && timeAlive > maxLifetime)
        {
            Destroy(gameObject);
        }

        ChangeEyeRedness(viewTimerPercentage);

        if (IsInLineOfSight() && IsInViewOfCamera())
        {
            Vector3 viewportPoint = playerCamera.WorldToScreenPoint(transform.position);
            float distanceFromCenter = Vector3.Distance(viewportPoint, new Vector3(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2, 0));
            viewTimer += Time.deltaTime * Mathf.Pow(1.6f - (distanceFromCenter / 1920f), angerAcceleration);

            if (viewTimer >= timeUntilAttack)
            {
                moveSpeed *= attackModeSpeedMultiplier;
                stoppingDistance = 1.6f;
                hasAttacked = true;
            }
        }
        else
        {
            viewTimer = 0f;
        }
    }

    bool IsInLineOfSight()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, sightRange))
        {
            return hit.transform == player;
        }

        return false;
    }

    bool IsInViewOfCamera()
    {
        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(transform.position);

        bool inFront = viewportPoint.z > 0;
        bool inView = viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;

        return inFront && inView;
    }

    void ChangeEyeRedness(float percentageOfRedness)
    {
        Color color = new Color(1, 1 - percentageOfRedness, 1 - percentageOfRedness, 1);
        leftEyeLight.color = color;
        rightEyeLight.color = color;
        leftEye.material.color = color;
        rightEye.material.color = color;
        leftEyeLight.range = 0.4f + percentageOfRedness / 1.5f;
        rightEyeLight.range = 0.4f + percentageOfRedness / 1.5f;
    }

    void OnTriggerEnter(Collider other)
    {
        playerHealth = other.GetComponent<Health>();
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerHealth.health -= damage;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0)
            {
                damageTimer = timeBetweenHits;
                playerHealth.health -= damage;
            }
        }
    }
}
