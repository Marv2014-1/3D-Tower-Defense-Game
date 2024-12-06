using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public int damage = 10; // Damage dealt by the melee weapon
    public float knockbackForce = 5f; // Knockback force applied to the target
    private Collider weaponCollider;

    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        if (weaponCollider == null)
        {
            Debug.LogError("EnemyWeapon requires a Collider component.");
        }
        weaponCollider.enabled = false; // Ensure the collider is disabled initially
    }

    private void OnTriggerEnter(Collider collision)
    {
        // if (collision.CompareTag("Ally") || collision.CompareTag("Player")) // Replace with appropriate target tags
        // {
        //     Debug.Log($"{collision.name} hit by weapon!");

        //     // Apply damage to the target
        //     IDamageable damageable = collision.GetComponent<IDamageable>();
        //     if (damageable != null)
        //     {
        //         Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;
        //         damageable.TakeDamage(damage, knockbackDirection, knockbackForce);
        //     }
        // }
    }

    public void ActivateWeapon()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }
    }

    public void DeactivateWeapon()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}
