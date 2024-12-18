using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the spawning of enemy waves in the game.
/// </summary>
public class WaveSpawner : MonoBehaviour
{
    [Header("Waypoint Settings")]
    [Tooltip("Starting point for enemy spawn")]
    public Transform startPoint; // Start spawn point

    [Tooltip("Waypoints for enemy pathing")]
    public Transform[] waypoints; // Array of waypoints to follow

    [Header("Spawner Configuration")]
    [Tooltip("Rate at which enemies are spawned (enemies per second)")]
    public float spawnRate = 1f;

    // Reference to the WaveManager to register spawned enemies
    private WaveManager waveManager;

    void Start()
    {
        // Find and reference the WaveManager in the scene
        waveManager = FindObjectOfType<WaveManager>();
        if (waveManager == null)
        {
            Debug.LogError("WaveManager not found in the scene!");
        }
    }

    /// <summary>
    /// Coroutine to spawn all enemies in the given wave.
    /// </summary>
    /// <param name="wave">The wave configuration to spawn.</param>
    public IEnumerator SpawnWave(Wave wave)
    {
        if (wave == null)
        {
            Debug.LogError("Wave is null in SpawnWave!");
            yield break;
        }

        List<Enemy> enemiesToRegister = new List<Enemy>();

        // Spawn enemies based on the wave's configuration
        for (int i = 0; i < wave.enemyTypes.Count; i++)
        {
            for (int j = 0; j < wave.counts[i]; j++)
            {
                Enemy enemy = SpawnEnemy(wave.enemyTypes[i].prefab);
                if (enemy != null)
                {
                    // Assign waypoints to the enemy
                    enemy.waypoints = waypoints;

                    // Optionally, set other enemy properties here

                    // Add to the list for registration
                    enemiesToRegister.Add(enemy);
                }

                // Wait based on the spawn rate
                yield return new WaitForSeconds(1f / spawnRate);
            }
        }

        // Register the spawned enemies with the WaveManager
        if (waveManager != null)
        {
            waveManager.RegisterEnemies(enemiesToRegister);
        }
    }

    /// <summary>
    /// Instantiates an enemy prefab at the start point.
    /// </summary>
    /// <param name="enemyPrefab">The enemy prefab to instantiate.</param>
    /// <returns>The instantiated Enemy component.</returns>
    private Enemy SpawnEnemy(GameObject enemyPrefab)
    {
        // Instantiate the enemy at the start point
        GameObject enemyInstance = Instantiate(enemyPrefab, startPoint.position, startPoint.rotation);

        // Get the Enemy component
        Enemy enemy = enemyInstance.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("Spawned enemy does not have an Enemy component!");
        }

        return enemy;
    }
}
