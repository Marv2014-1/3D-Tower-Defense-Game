using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
   [HideInInspector] public float speed;
    [HideInInspector] public int ArrowDamage;
    private Vector3 startPosition;
    private float maxTravelDistance;
    private Rigidbody rb;
    private float knockback;

    public void Initialize(Vector3 direction, float speed, int damage, float attackRange, float knockback)
    {
        rb = GetComponent<Rigidbody>();
        this.speed = speed;
        ArrowDamage = damage;
        this.knockback = knockback;
        maxTravelDistance = attackRange * 2f;

        rb.velocity = direction * speed;
        startPosition = transform.position;
    }

    private void Update()
    {
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
            // other.GetComponent<PlayerHealth>()?.TakeDamage(ArrowDamage, knockbackDirection, knockback);
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy")) // Ignore other enemies
        {
            Destroy(gameObject); // Destroy on other collisions
        }
    }
}
