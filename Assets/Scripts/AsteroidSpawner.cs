using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    public float maxSpawnDistance = 50f;
    public float minTimeBetweenSpawns = 1f;
    public float maxTimeBetweenSpawns = 5f;

    private void Start()
    {
        // Start the spawning process
        Invoke("SpawnAsteroid", Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
    }

    void SpawnAsteroid()
    {
        // Pick a random asteroid prefab from the array
        GameObject asteroidPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        // Calculate a random position within maxSpawnDistance
        Vector3 spawnPosition = transform.position + Random.onUnitSphere * maxSpawnDistance;

        // Instantiate the selected asteroid at the calculated position
        Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Invoke the SpawnAsteroid method again after a random delay
        Invoke("SpawnAsteroid", Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
    }
}
