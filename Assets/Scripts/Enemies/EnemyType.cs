using UnityEngine;

/// Represents a type of enemy with a name and associated prefab.
[System.Serializable]
public class EnemyType
{
    [Tooltip("Label for the enemy type")]
    public string name;

    [Tooltip("Prefab reference for the enemy")]
    public GameObject prefab;
}
