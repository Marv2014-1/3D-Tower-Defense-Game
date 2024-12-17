using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Cloud Settings")]
    public GameObject[] cloudPrefabs; // Array of cloud prefabs
    public int cloudDensity = 10; // Number of clouds to spawn
    public float spawnRadius = 50f; // Distance from the central point
    public float despawnDistance = 70f; // Distance at which clouds despawn
    public float cloudSpeed = 2f; // Movement speed of the clouds
    public float spawnHeightMin = 10f; // Minimum Y position for spawning clouds
    public float spawnHeightMax = 20f; // Maximum Y position for spawning clouds
    public float spawnDelay = 1f; // Delay between cloud spawns

    private List<GameObject> spawnedClouds = new List<GameObject>();
    private float nextSpawnTime = 0f;
    private int cloudsSpawned = 0;

    void Start()
    {
        nextSpawnTime = Time.time;
    }

    void Update()
    {
        MoveAndManageClouds();
        ManageCloudSpawning();
    }

    // Spawn clouds with delay
    void ManageCloudSpawning()
    {
        if (cloudsSpawned < cloudDensity && Time.time >= nextSpawnTime)
        {
            SpawnCloudAtRandomPosition();
            nextSpawnTime = Time.time + spawnDelay;
            cloudsSpawned++;
        }
    }

    // Spawn a single cloud at a random position within the spawn radius
    void SpawnCloudAtRandomPosition()
    {
        Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
        randomPosition.y = Random.Range(spawnHeightMin, spawnHeightMax); // Configurable Y position
        randomPosition += transform.position; // Offset by central position

        GameObject cloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];
        GameObject newCloud = Instantiate(cloudPrefab, randomPosition, Quaternion.identity, transform);

        spawnedClouds.Add(newCloud);
    }

    // Move clouds and check for despawning
    void MoveAndManageClouds()
    {
        for (int i = spawnedClouds.Count - 1; i >= 0; i--)
        {
            GameObject cloud = spawnedClouds[i];

            // Move the cloud
            cloud.transform.position += Vector3.forward * cloudSpeed * Time.deltaTime;

            // Check if the cloud has moved too far
            if (Vector3.Distance(transform.position, cloud.transform.position) > despawnDistance)
            {
                Destroy(cloud);
                spawnedClouds.RemoveAt(i);
                cloudsSpawned--; // Decrement cloud count
            }
        }
    }
}
