using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public static int EnemiesAlive = 0;

    [System.Serializable]
    public class EnemyType
    {
        public string name; // Label for the enemy type
        public GameObject prefab; // Prefab reference
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemyType> enemyTypes = new List<EnemyType>(); // Dynamic enemy types for this wave
        public List<int> counts = new List<int>(); // Corresponding counts for each type
        public float rate; // Spawn rate
    }

    [Header("Wave Configuration")]
    public List<Wave> waves; // List of waves

    [Header("Waypoint Settings")]
    public Transform startPoint; // Start spawn point
    public Transform[] waypoints; // Array of waypoints to follow

    [Header("UI Settings")]
    public Text waveCountdownText;
    public float timeBetweenWaves = 5f;

    private float countdown = 2f;
    private int waveIndex = 0;

    void Update()
    {
        // Ensure waves are configured
        if (waves == null || waves.Count == 0)
        {
            Debug.LogError("No waves configured in WaveSpawner!");
            return;
        }

        // Check if there are still enemies alive
        if (EnemiesAlive > 0) return;

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

    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];
        int totalEnemies = 0;

        // Calculate total enemies in the wave
        for (int i = 0; i < wave.counts.Count; i++)
        {
            totalEnemies += wave.counts[i];
        }

        EnemiesAlive = totalEnemies;

        // Spawn enemies for this wave
        for (int i = 0; i < wave.enemyTypes.Count; i++)
        {
            for (int j = 0; j < wave.counts[i]; j++)
            {
                SpawnEnemy(wave.enemyTypes[i].prefab);
                yield return new WaitForSeconds(1f / wave.rate);
            }
        }

        waveIndex++;
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        // Instantiate the enemy at the start point
        GameObject enemyInstance = Instantiate(enemyPrefab, startPoint.position, startPoint.rotation);

        // Assign the waypoints to the enemy
        Enemy enemy = enemyInstance.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.waypoints = waypoints;
        }
    }
}
