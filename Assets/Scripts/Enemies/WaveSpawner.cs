using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Manages the spawning of enemy waves in the game.
public class WaveSpawner : MonoBehaviour
{
    [Header("Wave Configuration")]
    [Tooltip("List of waves to spawn")]
    public List<Wave> waves; // List of waves

    [Header("Waypoint Settings")]
    [Tooltip("Starting point for enemy spawn")]
    public Transform startPoint; // Start spawn point

    [Tooltip("Waypoints for enemy pathing")]
    public Transform[] waypoints; // Array of waypoints to follow

    [Header("UI Settings")]
    [Tooltip("UI Text element to display wave countdown")]
    public Text waveCountdownText;

    [Tooltip("Time between consecutive waves in seconds")]
    public float timeBetweenWaves = 5f;

    private float countdown = 2f;
    private int waveIndex = 0;

    // List to keep track of spawned enemies
    private List<Enemy> spawnedEnemies = new List<Enemy>();

    void Update()
    {
        // Ensure waves are configured
        if (waves == null || waves.Count == 0)
        {
            Debug.LogError("No waves configured in WaveSpawner!");
            return;
        }

        // Check if there are still enemies alive
        if (spawnedEnemies.Count > 0) return;

        // Check if all waves have been completed
        if (waveIndex >= waves.Count)
        {
            Debug.Log("All waves completed!");
            this.enabled = false; // Stop the WaveSpawner
            return;
        }

        // Handle countdown and update UI
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;

        if (waveCountdownText != null)
        {
            waveCountdownText.text = string.Format("{0:00.00}", countdown);
        }
    }

    /// Coroutine to spawn all enemies in the current wave.
    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];
        int totalEnemies = 0;

        // Calculate total enemies in the wave
        for (int i = 0; i < wave.counts.Count; i++)
        {
            totalEnemies += wave.counts[i];
        }

        // Reset the list for the new wave
        spawnedEnemies.Clear();

        // Spawn enemies for this wave
        for (int i = 0; i < wave.enemyTypes.Count; i++)
        {
            for (int j = 0; j < wave.counts[i]; j++)
            {
                Enemy enemy = SpawnEnemy(wave.enemyTypes[i].prefab);
                if (enemy != null)
                {
                    // Assign this spawner as the owner
                    enemy.SetSpawner(this);
                    spawnedEnemies.Add(enemy);
                }
                yield return new WaitForSeconds(1f / wave.rate);
            }
        }

        waveIndex++;
    }

    /// Instantiates an enemy prefab at the start point and assigns waypoints.
    Enemy SpawnEnemy(GameObject enemyPrefab)
    {
        // Instantiate the enemy at the start point
        GameObject enemyInstance = Instantiate(enemyPrefab, startPoint.position, startPoint.rotation);

        // Assign the waypoints to the enemy
        Enemy enemy = enemyInstance.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.waypoints = waypoints;
        }

        return enemy;
    }

    /// Callback method for when an enemy dies
    public void OnEnemyDeath(Enemy enemy)
    {
        if (spawnedEnemies.Contains(enemy))
        {
            spawnedEnemies.Remove(enemy);
        }

        // Check if all enemies are dead to spawn the next wave
        if (spawnedEnemies.Count == 0)
        {
            // Optionally, reset countdown here if you want to delay the next wave
            countdown = timeBetweenWaves;
        }
    }
}
