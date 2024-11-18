using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour
{
    public float detectionRadius = 5f;
    public int cost = 1;
    public int maxHealth = 100;
    public int xpDrop = 10;
    public float baseMoveSpeed = 2f;
    public float currentMoveSpeed;
    protected int currentHealth;
    protected Transform playerTransform;
    protected Rigidbody rb;
    public bool canMove;
    public GameObject deathEffect;
    protected Animator animator;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        currentMoveSpeed = baseMoveSpeed;
        canMove = true;
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        if (canMove)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    protected void MoveTowardsPlayer()
    {
        if (!canMove || playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Vector3 movement = direction * currentMoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
    

    public virtual void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        canMove = false;
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
