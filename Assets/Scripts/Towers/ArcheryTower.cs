using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheryTower : Tower
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab; // Assign the Arrow prefab in the Inspector
    private Transform arrowSpawnPoint;
    public float arrowSpeed = 20f; // Increased speed to better handle fast enemies
    public float arrowRange = 50f; // Adjusted range as needed

    // Start is called before the first frame update
    void Start()
    {
        arrowSpawnPoint = transform.Find("ProjectileSpawn");
        cooldown = 500; // Adjust cooldown as necessary
        range = 30; // Tower's detection range
        onCooldown = false;
        enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    public override void Attack()
    {
        if (closestEnemy == null)
            return;

        // Calculate the interception point
        Vector3 interceptPoint = CalculateInterceptPoint(
            arrowSpawnPoint.position,
            closestEnemy.transform.position,
            closestEnemy.Velocity,
            arrowSpeed
        );

        // Determine the direction to the interception point
        Vector3 direction = (interceptPoint - arrowSpawnPoint.position).normalized;

        // Instantiate the arrow facing the interception direction
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, lookRotation);

        // Initialize the arrow's properties
        Arrow arrow = arrowInstance.GetComponent<Arrow>();
        if (arrow != null)
        {
            arrow.SetDamage(damage);
            arrow.SetSpeed(arrowSpeed);
            arrow.SetRange(arrowRange);
            arrow.SetTarget(closestEnemy); // Assign the target enemy
            arrow.SetVelocity(direction * arrowSpeed); // Set initial velocity towards interception point
        }
        else
        {
            Debug.LogError("Arrow prefab does not have an Arrow component.");
        }
    }

    /// Calculates the interception point where the arrow should aim to hit the moving enemy.
    private Vector3 CalculateInterceptPoint(Vector3 shooterPosition, Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 relativePosition = targetPosition - shooterPosition;
        Vector3 relativeVelocity = targetVelocity;

        float a = Vector3.Dot(relativeVelocity, relativeVelocity) - projectileSpeed * projectileSpeed;
        float b = 2 * Vector3.Dot(relativeVelocity, relativePosition);
        float c = Vector3.Dot(relativePosition, relativePosition);

        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0 || Mathf.Abs(a) < 0.001f)
        {
            // No valid interception point; aim directly at the current position
            return targetPosition;
        }

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float t1 = (-b + sqrtDiscriminant) / (2 * a);
        float t2 = (-b - sqrtDiscriminant) / (2 * a);

        float t = Mathf.Max(t1, t2);

        if (t < 0)
        {
            // No positive time solution; aim directly at the current position
            return targetPosition;
        }

        // Calculate the interception point
        return targetPosition + targetVelocity * t;
    }
}
