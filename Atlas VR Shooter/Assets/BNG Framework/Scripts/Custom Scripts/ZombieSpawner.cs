using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject zombiePrefab; // Prefab of the zombie to spawn
    public Transform player; // Reference to the player's transform
    public float spawnRadius = 10f; // Distance from the player to spawn zombies
    public float minSpawnDistance = 5f; // Minimum distance from the player to spawn zombies
    public float spawnRate = 2f; // Time between spawns (in seconds)
    public int maxZombies = 10; // Maximum number of zombies allowed on screen
    public int currentZombies = 0; // Current number of zombies on screen

    [Header("Difficulty Scaling")]
    public float spawnRateIncrease = 0.1f; // How much to increase spawn rate over time
    public float maxZombiesIncrease = 1; // How much to increase max zombies over time
    public float difficultyInterval = 10f; // Time interval for scaling difficulty (in seconds)

    private float timer = 0f;
    private float difficultyTimer = 0f;

    private void Start()
    {
        // Start spawning zombies
        StartCoroutine(SpawnZombies());
    }

    private void Update()
    {
        // Scale difficulty over time
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= difficultyInterval)
        {
            ScaleDifficulty();
            difficultyTimer = 0f;
        }
    }

    private IEnumerator SpawnZombies()
    {
        while (true)
        {
            if (currentZombies < maxZombies)
            {
                SpawnZombie();
                yield return new WaitForSeconds(spawnRate);
            }
            else
            {
                yield return null; // Wait until a zombie is destroyed
            }
        }
    }

    private void SpawnZombie()
    {
        // Calculate a random spawn position outside the player's line of sight
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);

        // Ensure the spawn position is at least minSpawnDistance away from the player
        if (Vector3.Distance(spawnPosition, player.position) < minSpawnDistance)
        {
            spawnPosition = player.position + (spawnPosition - player.position).normalized * minSpawnDistance;
        }

        // Instantiate the zombie
        GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        currentZombies++;

        // Track when the zombie is destroyed
        zombie.GetComponent<Damageable>().onDestroyed.AddListener(OnZombieDestroyed);
    }

    private void OnZombieDestroyed()
    {
        currentZombies--;
    }

    private void ScaleDifficulty()
    {
        // Increase spawn rate and max zombies over time
        spawnRate = Mathf.Max(0.5f, spawnRate - spawnRateIncrease); // Decrease spawn interval
        maxZombies += (int)maxZombiesIncrease;

        Debug.Log($"Difficulty Increased! Spawn Rate: {spawnRate}, Max Zombies: {maxZombies}");
    }
    // Draw Gizmos in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (player == null)
            return;

        // Draw the spawn radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, spawnRadius);

        // Draw the minimum spawn distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, minSpawnDistance);
    }
}
