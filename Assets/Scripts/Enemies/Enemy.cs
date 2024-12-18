using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    public int maxHealth = 20;
    public int coinDrop = 15;
    public float moveSpeed = 2f;
    public float rotationSpeed = 360f; // Degrees per second for rotation

    [Header("Waypoints")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private Rigidbody rb;
    protected Animator animator; // Allow access in child classes
    protected int currentHealth;

    private bool isDead = false; // Ensure actions stop after death

    public Vector3 Velocity { get; private set; }

    // Reference to the CastleHealthManager
    private CastleHealthManager castleHealthManager;

    /// <summary>
    /// Static event triggered when any enemy dies.
    /// Subscribed by WaveManager to track active enemies.
    /// </summary>
    public static event System.Action<Enemy> OnEnemyDeath;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Find the CastleHealthManager in the scene
        castleHealthManager = FindObjectOfType<CastleHealthManager>();
        if (castleHealthManager == null)
        {
            Debug.LogError("CastleHealthManager not found in the scene!");
        }
    }

    protected virtual void Update()
    {
        if (isDead) return; // Stop all actions if the enemy is dead

        Velocity = rb.velocity;

        if (currentHealth <= 0)
        {
            Die();
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

        // delete the enemy if they go below the map (-5 y)
        if (transform.position.y < -5)
        {
            Die();
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
        // Damage the castle
        if (castleHealthManager != null)
        {
            castleHealthManager.DamageCastle(1);
        }
        else
        {
            Debug.LogError("CastleHealthManager not found in the scene!");
        }

        // Notify the WaveManager of death and destroy the enemy
        Die();
    }

    /// Applies damage to the enemy.
    public void TakeDamage(int damageAmount)
    {
        if (isDead || currentHealth <= 0) return;

        currentHealth -= damageAmount;
        animator.SetBool("IsHurting", true);

        CancelInvoke(nameof(StopHurtAnimation)); // Cancel any existing invokes
        Invoke(nameof(StopHurtAnimation), 0.5f); // Stop hurting animation after a short delay

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void StopHurtAnimation()
    {
        if (!isDead)
        {
            animator.SetBool("IsHurting", false);
        }
    }

    /// Handles the enemy's death process.
    private void Die()
    {
        if (isDead) return;

        isDead = true; // Mark the enemy as dead
        animator.SetBool("IsDead", true);

        // Give player coins on death
        Coins playerCoins = FindObjectOfType<Coins>();
        if (playerCoins != null)
        {
            playerCoins.addCoins(coinDrop);
        }
        else
        {
            Debug.LogError("Coins component not found in the scene!");
        }

        // Updates player's score on death
        ScoreManager score = FindObjectOfType<ScoreManager>();
        if (score != null)
        {
            score.UpdateScore(coinDrop);
        }
        else
        {
            Debug.LogError("ScoreManager component not found in the scene!");
        }

        // Disable movement and other components during death
        rb.isKinematic = true;

        // Optionally, remove the collider to prevent interactions
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Notify the WaveManager before destruction
        NotifyWaveManagerOfDeath();

        // Destroy the enemy GameObject after the death animation has played
        Destroy(gameObject, 2f); // Adjust the delay as per your death animation length
    }

    /// Notifies the WaveManager that this enemy has died.
    private void NotifyWaveManagerOfDeath()
    {
        // Trigger the static OnEnemyDeath event
        OnEnemyDeath?.Invoke(this);
    }


    public bool IsDead()
    {
        return isDead;
    }
}
