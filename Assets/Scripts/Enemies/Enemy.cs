using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [Header("General Settings")]
    public int maxHealth = 100;
    public float moveSpeed = 2f;
    public float attackRange = 2f;

    [Header("Path Settings")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    protected int currentHealth;
    protected Rigidbody rb;
    protected Transform targetTransform;
    private bool isAttacking = false;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (isAttacking) return;

        // Prioritize attacking nearby allies
        if (FindNearestTarget("Ally"))
        {
            MoveTowardsTarget();
        }
        // Otherwise, follow the path toward the castle
        else if (currentWaypointIndex < waypoints.Length)
        {
            FollowPath();
        }
    }

    protected bool FindNearestTarget(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < shortestDistance && distance <= attackRange)
            {
                shortestDistance = distance;
                nearestTarget = target;
            }
        }

        if (nearestTarget != null)
        {
            targetTransform = nearestTarget.transform;
            isAttacking = true;
            StartCoroutine(AttackTarget());
            return true;
        }

        targetTransform = null;
        return false;
    }

    protected void FollowPath()
    {
        Transform waypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (waypoint.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

        // Move to the next waypoint if close enough
        if (Vector3.Distance(transform.position, waypoint.position) < 0.5f)
        {
            currentWaypointIndex++;
        }
    }

    protected IEnumerator AttackTarget()
    {
        while (isAttacking && targetTransform != null)
        {
            // Perform the attack (add your damage logic here)
            Debug.Log($"Attacking {targetTransform.name}");
            yield return new WaitForSeconds(1f); // Delay between attacks
        }

        isAttacking = false;
    }

    protected void MoveTowardsTarget()
    {
        if (targetTransform == null) return;

        Vector3 direction = (targetTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Castle"))
        {
            Debug.Log("Enemy reached the castle! Game over.");
            // Implement game-over logic here
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
