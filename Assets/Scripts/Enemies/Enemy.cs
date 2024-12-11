using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    public int maxHealth = 20;
    public float moveSpeed = 2f;
    public float rotationSpeed = 360f; // Degrees per second for rotation

    [Header("Waypoints")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private Rigidbody rb;
    protected Animator animator; // Allow access in child classes
    protected int currentHealth;

    private bool isDead = false; // Ensure actions stop after death

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        if (isDead) return; // Stop all actions if the enemy is dead

        if (currentHealth <= 0)
        {
            PlayDeathAnimation();
            return;
        }

        if (currentWaypointIndex < waypoints.Length)
        {
            FollowPath();
        }
        else
        {
            ReachedEndOfPath();
        }
    }

    private void FollowPath()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Enable IsWalking animation if moving
        bool isMoving = direction.magnitude > 0.1f;
        animator.SetBool("IsWalking", isMoving);

        if (isMoving)
        {
            // Calculate target rotation towards the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            // Smoothly rotate towards the target rotation
            Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.MoveRotation(newRotation);

            // Move the enemy towards the waypoint
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
        }

        // Check if close enough to waypoint to move to the next
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            currentWaypointIndex++;
        }
    }

    private void ReachedEndOfPath()
    {
        // Custom behavior when the enemy reaches the final waypoint
        Destroy(gameObject);
        WaveSpawner.EnemiesAlive--;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || currentHealth <= 0) return;

        currentHealth -= damage;
        animator.SetBool("IsHurting", true);

        Invoke(nameof(StopHurtAnimation), 0.5f); // Stop hurting animation after a short delay

        if (currentHealth <= 0)
        {
            PlayDeathAnimation();
        }
    }

    private void StopHurtAnimation()
    {
        if (!isDead)
        {
            animator.SetBool("IsHurting", false);
        }
    }

    private void PlayDeathAnimation()
    {
        if (isDead) return;

        isDead = true; // Mark the enemy as dead
        animator.SetBool("IsDead", true);

        // Disable movement and other components during death
        rb.isKinematic = true;

        // Optionally, remove the collider to prevent interactions
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        Destroy(gameObject, 2f); // Wait for death animation before destroying
    }

    public bool IsDead()
    {
        return isDead;
    }

}
