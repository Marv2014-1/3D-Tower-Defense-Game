// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [RequireComponent(typeof(Collider))]
// [RequireComponent(typeof(Rigidbody))]
// public abstract class Enemy : MonoBehaviour
// {
//     public float detectionRadius = 5f;
//     public int cost = 1;
//     public int maxHealth = 100;
//     public int xpDrop = 10;
//     public float baseMoveSpeed = 2f;
//     public float currentMoveSpeed;
//     protected int currentHealth;
//     protected Transform targetTransform; // Changed from playerTransform to targetTransform
//     protected Rigidbody rb;
//     public bool canMove;
//     public GameObject deathEffect;
//     protected Animator animator;

//     protected virtual void Awake()
//     {
//         currentHealth = maxHealth;
//         currentMoveSpeed = baseMoveSpeed;
//         canMove = true;
//         rb = GetComponent<Rigidbody>();
//         animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
//         FindNearestAlly(); // Find the initial target
//     }
//     protected float targetRefreshInterval = 1f; // Refresh interval
//     private float lastTargetRefreshTime = 0f;

//     protected void Update()
//     {
//         if (Time.time >= lastTargetRefreshTime + targetRefreshInterval)
//         {
//             FindNearestAlly();
//             lastTargetRefreshTime = Time.time;
//         }
//     }
//     protected virtual void FixedUpdate()
//     {
//         if (canMove)
//         {
//             MoveTowardsTarget();
//         }
//         else
//         {
//             rb.velocity = Vector3.zero;
//         }
//     }

//     protected void MoveTowardsTarget()
//     {
//         if (!canMove || targetTransform == null) return;

//         Vector3 direction = (targetTransform.position - transform.position).normalized;
//         Vector3 movement = direction * currentMoveSpeed * Time.fixedDeltaTime;
//         rb.MovePosition(rb.position + movement);
//     }

//     protected void FindNearestAlly()
//     {
//         GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
//         float shortestDistance = Mathf.Infinity;
//         GameObject nearestAlly = null;

//         foreach (GameObject ally in allies)
//         {
//             float distance = Vector3.Distance(transform.position, ally.transform.position);
//             if (distance < shortestDistance && distance <= detectionRadius)
//             {
//                 shortestDistance = distance;
//                 nearestAlly = ally;
//             }
//         }

//         targetTransform = nearestAlly?.transform;
//     }

//     public virtual void TakeDamage(int damageAmount)
//     {
//         currentHealth -= damageAmount;
//         if (currentHealth <= 0)
//         {
//             Die();
//         }
//     }

//     protected virtual void Die()
//     {
//         canMove = false;
//         if (deathEffect != null)
//         {
//             Instantiate(deathEffect, transform.position, Quaternion.identity);
//         }
//         Destroy(gameObject);
//     }
// }
