using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySummonData", menuName = "Create EnemySummonData")]
public class EnemySummonData : ScriptableObject
{
    public GameObject EnemyPrefab; // The prefab for the enemy
    public int EnemyID; // An ID to identify the enemy

    // Method to create an instance of EnemySummonData
    public static EnemySummonData CreateInstance()
    {
        return ScriptableObject.CreateInstance<EnemySummonData>();
    }
}
