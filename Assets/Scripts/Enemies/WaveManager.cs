using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float timeBetweenWaves = 10f;

    private float countdown;
    private int waveIndex = 0;
    private bool doneCounting = false;
    private List<Enemy> allSpawnedEnemies = new List<Enemy>();

    void Start()
    {
        countdown = timeBetweenWaves;
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

        // If there are active enemies, update the UI and skip the countdown
        if (allSpawnedEnemies.Count > 0)
        {
            UpdateWaveUI();
            return;
        }

        // If no enemies remain, run countdown to next wave
        if (countdown > 0f && !doneCounting)
        {
            countdown -= Time.deltaTime;
            UpdateWaveUI();

            if (countdown <= 0f)
            {
                waveCountdownText.text = "Spawning Next Wave!";
                StartCoroutine(SpawnWave());
                // Reset countdown for the next wave
                countdown = timeBetweenWaves;
                doneCounting = true;
            }
        }
        else
        {
            UpdateWaveUI();
        }
    }

    IEnumerator SpawnWave()
    {
        if (waveIndex >= waves.Count)
            yield break;

        Wave currentWave = waves[waveIndex];
        waveCounterText.text = $"Wave: {waveIndex + 1}";

        foreach (WaveSpawner spawner in waveSpawners)
        {
            if (spawner != null)
                StartCoroutine(spawner.SpawnWave(currentWave));
        }

        // Do not increment waveIndex here
        yield return null;
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        if (allSpawnedEnemies.Contains(enemy))
            allSpawnedEnemies.Remove(enemy);

        UpdateWaveUI();

        // Wait a few seconds before checking if all enemies are dead
        StartCoroutine(CheckAllEnemiesDead());

        // If all enemies are dead, increment waveIndex
        if (allSpawnedEnemies.Count == 0)
        {
            waveIndex++;
            UpdateWaveUI();

            // Check if all waves are completed
            if (waveIndex >= waves.Count)
            {
                // Trigger EndMenu
                TriggerEndMenu();
            }
        }
    }

    IEnumerator CheckAllEnemiesDead()
    {
        yield return new WaitForSeconds(5f);

        if (allSpawnedEnemies.Count == 0)
        {
            doneCounting = false;
        }
    }

    public void RegisterEnemies(List<Enemy> enemies)
    {
        if (enemies != null && enemies.Count > 0)
            allSpawnedEnemies.AddRange(enemies);

        UpdateWaveUI();
    }

    private void UpdateWaveUI()
    {
        if (waveIndex < waves.Count)
            waveCounterText.text = $"Wave: {waveIndex + 1}";
        else
            waveCounterText.text = "Final Wave!";

        // If enemies are present, show enemy count. Otherwise, show countdown or spawning message.
        if (allSpawnedEnemies.Count > 0)
        {
            waveCountdownText.text = $"Enemies Left: {allSpawnedEnemies.Count}";
        }
        else
        {
            if (countdown > 0f && !doneCounting)
                waveCountdownText.text = $"Next Wave In: {countdown:0.00}s";
            else if (doneCounting)
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
}
