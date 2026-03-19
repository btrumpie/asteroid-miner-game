using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    // This script is attached to an asteroid spawner that spawns a lot of asteroids quickly
    // This allows the starting asteroids to spawn in when the player starts a level

    public float timeUntilObliteration = 1f;
    void Start()
    {
        Invoke(nameof(Obliterate), timeUntilObliteration);
    }

    void Obliterate()
    {
        Destroy(gameObject);
    }
}
