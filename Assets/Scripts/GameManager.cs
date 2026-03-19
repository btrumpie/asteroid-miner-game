using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int level = 1;
    public bool playerInShip = false;

    [Header("Canvases")]
    public Canvas gameCanvas;
    public Canvas gameOverCanvas;

    [Header("Ship Exterior and Interior")]
    public GameObject shipInterior;
    public GameObject shipExterior;

    [Header("Asteroid Spawner")]
    public AsteroidSpawner asteroidSpawner;

    [Header("Enemy Prefabs")]
    public GameObject[] enemies;

    [Header("Chances Every Second")]
    public float chanceToSpawnWatcher = 0.01f;

    private int fixedUpdatesCalled = 0;
    private Oxygen playerOxygen;
    private FollowPlayer asteroidSpawnerFollowPlayer;
    private Health playerHealthScript;

    void Start()
    {
        playerOxygen = Camera.main.GetComponent<Oxygen>();
        asteroidSpawnerFollowPlayer = asteroidSpawner.GetComponent<FollowPlayer>();
        playerHealthScript = Camera.main.GetComponent<Health>();
    }

    void Update()
    {
        if (shipInterior.activeSelf)
        {
            playerOxygen.inOxygenatedArea = true;
            playerInShip = true;
        }
        else
        {
            playerOxygen.inOxygenatedArea = false; // Player loses oxygen outside of ship
            playerInShip = false;
        }

        // Game over when player loses all their health
        if (playerHealthScript.health <= 0)
        {
            GameOver();
        }
    }

    void FixedUpdate()
    {
        fixedUpdatesCalled++;

        if (fixedUpdatesCalled >= 50)
        {
            fixedUpdatesCalled = 0;
            ChancesEverySecond();
        }
    }

    void ChancesEverySecond()
    {
        // This method is called once every second

        // Potentially spawn a watcher
        if (!shipInterior.activeSelf && Random.value < chanceToSpawnWatcher)
        {
            SpawnEnemy(enemies[0]);
        }
    }

    public void SpawnEnemy(GameObject enemy)
    {
        // Instantiate the enemy (this is the general spawn call)
        GameObject spawnedEnemy = Instantiate(enemy);

        // Check if the spawned enemy is a Watcher
        if (spawnedEnemy.GetComponent<Watcher>() != null)
        {
            float randomDistance = Random.Range(40f, 100f);
            float randomAngle = Random.Range(180f, 360f);
            Vector3 spawnDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));
            float randomHeight = Random.Range(-10f, 10f);
            Vector3 spawnPosition = Camera.main.transform.position + spawnDirection * randomDistance;
            spawnPosition.y += randomHeight;
            Vector3 viewportPoint = Camera.main.WorldToViewportPoint(spawnPosition);

            // Ensure the Watcher is off-screen
            while (viewportPoint.x >= 0f && viewportPoint.x <= 1f && viewportPoint.y >= 0f && viewportPoint.y <= 1f)
            {
                // If the Watcher is on-screen, adjust the spawn position and check again
                randomAngle = Random.Range(180f, 360f);
                spawnDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));

                // Recalculate spawn position
                spawnPosition = Camera.main.transform.position + spawnDirection * randomDistance;
                spawnPosition.y += randomHeight;

                viewportPoint = Camera.main.WorldToViewportPoint(spawnPosition);
            }

            // Set the Watcher's position to the calculated spawn position
            spawnedEnemy.transform.position = spawnPosition;
        }

        //Debug.Log(enemy.name + " spawned.");
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Game Over");
    }
}
