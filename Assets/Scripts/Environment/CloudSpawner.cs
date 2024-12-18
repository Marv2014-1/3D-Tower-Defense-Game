using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Cloud Settings")]
    public GameObject[] cloudPrefabs; // Array of cloud prefabs
    public int initialCloudDensity = 10; // Initial number of clouds to spawn
    public int maxAdditionalClouds = 20; // Maximum additional clouds allowed after crossing halfway
    public float spawnRadius = 50f; // Distance from the central point
    public float despawnDistance = 70f; // Distance at which clouds despawn
    public float cloudSpeed = 3f; // Movement speed of the clouds
    public float spawnHeightMin = 10f; // Minimum Y position for spawning clouds
    public float spawnHeightMax = 20f; // Maximum Y position for spawning clouds
    public float spawnDelay = 1f; // Delay between cloud spawns

    private List<GameObject> spawnedClouds = new List<GameObject>();
    private float nextSpawnTime = 0f;
    private int currentCloudDensity;
    private int additionalCloudsSpawned = 0;

    private float halfwayDistance;

    void Start()
    {
        currentCloudDensity = initialCloudDensity;
        halfwayDistance = spawnRadius + (despawnDistance - spawnRadius) / 2f;
        // Initially spawn the initialCloudDensity number of clouds
        for (int i = 0; i < initialCloudDensity; i++)
        {
            SpawnCloudAtRandomPosition();
        }
        nextSpawnTime = Time.time + spawnDelay;
    }

    void Update()
    {
        MoveAndManageClouds();
        ManageCloudSpawning();
    }

    // Spawn clouds with delay based on currentCloudDensity
    void ManageCloudSpawning()
    {
        if (additionalCloudsSpawned >= maxAdditionalClouds)
            return; // Reached maximum additional clouds

        if (spawnedClouds.Count < currentCloudDensity && Time.time >= nextSpawnTime)
        {
            SpawnCloudAtRandomPosition();
            nextSpawnTime = Time.time + spawnDelay;
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

    // Move clouds and check for despawning or passing halfway
    void MoveAndManageClouds()
    {
        for (int i = spawnedClouds.Count - 1; i >= 0; i--)
        {
            GameObject cloud = spawnedClouds[i];
            if (cloud == null)
            {
                spawnedClouds.RemoveAt(i);
                continue;
            }

            // Move the cloud
            Vector3 movement = Vector3.forward * (cloudSpeed + Random.Range(-1f, 1f)) * Time.deltaTime;
            cloud.transform.position += movement;

            // Calculate distance from central point
            float distance = Vector3.Distance(transform.position, cloud.transform.position);

            // // Check if the cloud has crossed the halfway mark
            // if (distance > halfwayDistance && !cloud.GetComponent<Cloud>().hasPassedHalfway)
            // {
            //     // Mark that this cloud has passed halfway
            //     Cloud cloudScript = cloud.GetComponent<Cloud>();
            //     if (cloudScript != null)
            //     {
            //         cloudScript.hasPassedHalfway = true;
            //         // Allow spawning an additional cloud
            //         if (additionalCloudsSpawned < maxAdditionalClouds)
            //         {
            //             currentCloudDensity++;
            //             additionalCloudsSpawned++;
            //         }
            //     }
            // }

            // Check if the cloud has moved too far
            if (distance > despawnDistance)
            {
                Destroy(cloud);
                spawnedClouds.RemoveAt(i);
                currentCloudDensity--; // Decrement current cloud density
            }
        }
    }
}
