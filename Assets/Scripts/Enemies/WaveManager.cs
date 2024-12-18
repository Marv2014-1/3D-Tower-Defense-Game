using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Configuration")]
    public List<Wave> waves;

    [Header("Spawners")]
    public List<WaveSpawner> waveSpawners;

    [Header("UI Settings")]
    public TMP_Text waveCounterText;
    public TMP_Text waveCountdownText;

    [Header("End Menu Settings")]
    public GameObject endMenu;          // Reference to the EndMenu GameObject
    public TMP_Text endMenuText;        // Reference to the TextMeshPro component within EndMenu

    [Header("Spawn Conditions")]
    public int enemyThreshold = 3;                 // Threshold for enemy count
    public float timeThreshold = 15f;               // Duration to wait before spawning next wave
    public float enemyCountUnchangedThreshold = 45f; // Time in seconds to wait before starting next wave if enemy count is unchanged

    private float countdown;
    private int waveIndex = 0;
    private bool isCountingDown = false;
    private List<Enemy> allSpawnedEnemies = new List<Enemy>();

    // Variables to track enemy count changes
    private float timeSinceEnemyCountChanged = 0f;
    private int lastEnemyCount = 0;

    void Start()
    {
        countdown = timeThreshold;
        Enemy.OnEnemyDeath += HandleEnemyDeath;
        UpdateWaveUI();

        // Ensure EndMenu is disabled at the start
        if (endMenu != null)
        {
            endMenu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("EndMenu is not assigned in the WaveManager.");
        }

        lastEnemyCount = allSpawnedEnemies.Count;
        timeSinceEnemyCountChanged = 0f;

        // Optionally start the first wave automatically
        StartCoroutine(SpawnWave());
    }

    void Update()
    {
        // If all waves are completed, do nothing
        if (waveIndex >= waves.Count)
        {
            waveCounterText.text = "Final Wave!";
            waveCountdownText.text = "";
            return;
        }

        // Current number of active enemies
        int currentEnemyCount = allSpawnedEnemies.Count;

        // Remove any null entries from the enemy list to prevent phantom enemies
        allSpawnedEnemies.RemoveAll(enemy => enemy == null);

        // Track enemy count changes
        if (currentEnemyCount != lastEnemyCount)
        {
            timeSinceEnemyCountChanged = 0f;
            lastEnemyCount = currentEnemyCount;
        }
        else
        {
            timeSinceEnemyCountChanged += Time.deltaTime;
        }

        // Start the next wave if enemy count hasn't changed for the threshold duration
        if (timeSinceEnemyCountChanged >= enemyCountUnchangedThreshold && !isCountingDown)
        {
            StartCoroutine(SpawnWave());
            timeSinceEnemyCountChanged = 0f;
        }

        // Check the number of active enemies against the threshold
        if (currentEnemyCount < enemyThreshold)
        {
            if (!isCountingDown)
            {
                isCountingDown = true;
                countdown = timeThreshold;
            }

            if (isCountingDown)
            {
                countdown -= Time.deltaTime;
                UpdateWaveUI();

                if (countdown <= 0f)
                {
                    StartCoroutine(SpawnWave());
                    // Reset countdown and counting flag for the next wave
                    countdown = timeThreshold;
                    isCountingDown = false;
                }
            }
        }
        else
        {
            // Reset the countdown if enemy count is back to threshold or above
            if (isCountingDown)
            {
                isCountingDown = false;
                countdown = timeThreshold;
                UpdateWaveUI();
            }
        }
    }

    IEnumerator SpawnWave()
    {
        if (waveIndex >= waves.Count)
        {
            // All waves completed
            TriggerEndMenu();
            yield break;
        }

        Wave currentWave = waves[waveIndex];
        waveCounterText.text = $"Wave: {waveIndex}";

        foreach (WaveSpawner spawner in waveSpawners)
        {
            if (spawner != null)
            {
                StartCoroutine(spawner.SpawnWave(currentWave));
            }
        }

        isCountingDown = false;
        UpdateWaveUI();

        waveIndex++; // Increment wave index after spawning the wave

        yield return null;
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        if (allSpawnedEnemies.Contains(enemy))
            allSpawnedEnemies.Remove(enemy);

        UpdateWaveUI();

    }

    public void RegisterEnemies(List<Enemy> enemies)
    {
        if (enemies != null && enemies.Count > 0)
        {
            allSpawnedEnemies.AddRange(enemies);
        }

        UpdateWaveUI();
    }

    private void UpdateWaveUI()
    {
        if (waveIndex < waves.Count)
            waveCounterText.text = $"Wave: {waveIndex}";
        else
            waveCounterText.text = "Final Wave!";

        // Update the countdown or enemy count display
        if (allSpawnedEnemies.Count > 0)
        {
            waveCountdownText.text = $"Enemies Left: {allSpawnedEnemies.Count}";
        }
        else
        {
            if (isCountingDown)
                waveCountdownText.text = $"Next Wave In: {countdown:0.00}s";
            else
                waveCountdownText.text = "Spawning Next Wave!";
        }
    }

    private void TriggerEndMenu()
    {
        if (endMenu != null && endMenuText != null)
        {
            // Set the victory message
            endMenuText.text = "You Win!";

            // Enable the EndMenu GameObject
            endMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;

            // Pause the game
            Time.timeScale = 0f;

            this.enabled = false;
        }
        else
        {
            Debug.LogWarning("EndMenu or EndMenuText is not assigned in the WaveManager.");
        }
    }

    void OnDestroy()
    {
        Enemy.OnEnemyDeath -= HandleEnemyDeath;
    }

    void OnEnable()
    {
        Enemy.OnEnemyDeath += HandleEnemyDeath;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDeath -= HandleEnemyDeath;
    }
}
