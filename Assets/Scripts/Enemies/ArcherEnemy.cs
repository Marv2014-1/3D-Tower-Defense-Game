using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcherEnemy : Enemy
{
    [Header("Arrow Settings")]
    [SerializeField] public GameObject arrowPrefab;
    [SerializeField] public Transform shootPoint;
    public float knockBackForce = 5f;

    [Header("Bow Settings")]
    [Range(0, 10)]
    public float bowPower = 30f;

    [Header("Attack Settings")]
    [SerializeField] private readonly float attackRange = 10f;
    [SerializeField] private readonly float fireRate = 2f;
    private float nextFireTime = 0f;

    private EnemyArrow arrow;
    private Transform playerTransform;
    private Animator animator;
    private bool canMove;
    private float attackDamage;

    protected override void Awake()
    {
        base.Awake();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override void Update()
    {
        base.Update();

        if (IsDead() || playerTransform == null) return;

        // Stop following waypoints if the player is in range and attack
        if (PlayerInRange())
        {
            AttackPlayer();
        }
    }

    private bool PlayerInRange()
    {
        // Ensure there's a player transform and check attack range
        return playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= attackRange;
    }

    private void AttackPlayer()
    {

        if (Time.time >= nextFireTime)
        {
            Debug.Log("Attack Triggered");

            if (animator != null)
            {
                animator.SetTrigger("TriggerAttack");
                SpawnArrow();
            }
            
            nextFireTime = Time.time + fireRate; // Reset fire cooldown
        }
    }

    public void SpawnArrow()
    {
        if (playerTransform == null)
        {
            Debug.Log("Player Transform is null");
            return;
        }

        Debug.Log("Spawning Arrow");
        Vector3 arrowDirection = (playerTransform.position - shootPoint.position).normalized;
        Quaternion arrowRotation = Quaternion.LookRotation(arrowDirection);
        SpawnArrowInstance(arrowPrefab, shootPoint.position, arrowRotation, arrowDirection);
    }

    /// <summary>
    /// Handles the instantiation and initialization of an arrow instance.
    /// </summary>
    /// <param name="prefab">The arrow prefab to instantiate.</param>
    /// <param name="position">The spawn position.</param>
    /// <param name="rotation">The spawn rotation.</param>
    /// <param name="direction">The direction the arrow will travel.</param>
    private void SpawnArrowInstance(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 direction)
    {
        EnemyArrow arrow = Instantiate(prefab, position, rotation).GetComponent<EnemyArrow>();
        if (arrow != null)
        {
            arrow.Initialize(direction, bowPower, (int)attackDamage, attackRange, knockBackForce);
        }
    }

    public void FinishAttack()
    {
        // Allow movement again if needed after the attack animation
        canMove = true;
    }
}
