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

    public float timeBetweenWaves = 5f;

    private float countdown;
    private int waveIndex = 0;
    private List<Enemy> allSpawnedEnemies = new List<Enemy>();

    void Start()
    {
        countdown = timeBetweenWaves;
        Enemy.OnEnemyDeath += HandleEnemyDeath;
        UpdateWaveUI();
    }

    void Update()
    {
        if (waveIndex >= waves.Count)
        {
            waveCounterText.text = "All Waves Completed!";
            waveCountdownText.text = "";
            return;
        }

        // If there are active enemies, don't run the countdown.
        // Instead, display how many enemies remain.
        if (allSpawnedEnemies.Count > 0)
        {
            UpdateWaveUI();
            return;
        }

        // If no enemies remain, run countdown to next wave
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
        UpdateWaveUI();
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

        waveIndex++;
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
            allSpawnedEnemies.AddRange(enemies);

        UpdateWaveUI();
    }

    private void UpdateWaveUI()
    {
        if (waveIndex < waves.Count)
            waveCounterText.text = $"Wave: {waveIndex + 1}";
        else
            waveCounterText.text = "All Waves Completed!";

        // If enemies are present, show enemy count. Otherwise, show countdown.
        if (allSpawnedEnemies.Count > 0)
        {
            waveCountdownText.text = $"Enemies Left: {allSpawnedEnemies.Count}";
        }
        else
        {
            waveCountdownText.text = string.Format("{0:00.00}", Mathf.Max(countdown, 0f));
        }
    }

    void OnDestroy()
    {
        Enemy.OnEnemyDeath -= HandleEnemyDeath;
    }
}
