using System.Collections.Generic;
using UnityEngine;

/// Defines a wave consisting of multiple enemy types, their counts, and spawn rate.
[System.Serializable]
public class Wave
{
    [Tooltip("List of enemy types to spawn in this wave")]
    public List<EnemyType> enemyTypes = new List<EnemyType>();

    [Tooltip("Corresponding counts for each enemy type")]
    public List<int> counts = new List<int>();

    [Tooltip("Spawn rate (enemies per second)")]
    public float rate;
}
