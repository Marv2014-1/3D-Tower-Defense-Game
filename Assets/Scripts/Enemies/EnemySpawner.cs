using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private EnemySummonData[] enemySummonData; // Array of enemy data
    [SerializeField] private Transform[] spawnPoints; // Locations where enemies will spawn
    [SerializeField] private float spawnInterval = 5f; // Time between spawns
    [SerializeField] private int maxEnemies = 10; // Maximum number of enemies in the scene

    private List<GameObject> activeEnemies = new List<GameObject>(); // Track active enemies

    private void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Check if the number of active enemies is below the limit
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        if (enemySummonData.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No enemy data or spawn points assigned!");
            return;
        }

        // Randomly select an enemy type and spawn point
        EnemySummonData randomEnemy = enemySummonData[Random.Range(0, enemySummonData.Length)];
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn the enemy
        GameObject enemy = Instantiate(randomEnemy.EnemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        activeEnemies.Add(enemy);

        // Add logic to remove the enemy from the active list when destroyed
        enemy.GetComponent<Enemy>().OnEnemyDestroyed += () => activeEnemies.Remove(enemy);
    }
}
