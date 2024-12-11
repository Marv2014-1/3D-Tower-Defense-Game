using UnityEngine;

[System.Serializable]
public class Wave
{
    public WaveSpawner.EnemyType[] enemyTypes; // List of enemy types
    public int[] counts; // Counts per enemy type
    public float rate; // Spawn rate
}
