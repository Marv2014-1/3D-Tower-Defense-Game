using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]

public class Enemy : MonoBehaviour
{
    [Header("General Settings")]
    public int maxHealth = 100;
    public float moveSpeed = 2f;

    protected int currentHealth;
    protected Transform targetTransform;
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        FindTarget();
        MoveTowardsTarget();
    }

    protected virtual void FindTarget()
    {
        // Find the nearest "Player" tagged GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            targetTransform = player.transform;
        }
    }

    protected virtual void MoveTowardsTarget()
    {
        if (targetTransform == null) return;

        Vector3 direction = (targetTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
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
