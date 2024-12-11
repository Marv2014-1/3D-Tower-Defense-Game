using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 3f;
    private float nextAttackTime = 0f;
    [SerializeField] private float attackCooldown = 2f; // Cooldown between attacks

    [SerializeField] private GameObject enemyWeapon; // Reference to the weapon with the MeshCollider
    private MeshCollider weaponCollider;

    protected override void Awake()
    {
        base.Awake();
        if (enemyWeapon != null)
        {
            weaponCollider = enemyWeapon.GetComponent<MeshCollider>();
            if (weaponCollider != null)
            {
                weaponCollider.enabled = false; // Ensure the collider is initially disabled
            }
            else
            {
                Debug.LogError("EnemyWeapon must have a MeshCollider attached.");
            }
        }
    }

    protected void Update()
    {
        base.Update();

        if (currentHealth <= 0) return;

        // Simulate an attack on a nearby target (replace with actual target detection logic)
        Transform target = DetectTarget();
        if (target != null && Time.time >= nextAttackTime)
        {
            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private Transform DetectTarget()
    {
        // Placeholder logic for detecting a target within attack range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player")) // Replace with your target tag
            {
                return hitCollider.transform;
            }
        }
        return null;
    }

    private void PerformAttack()
    {
        int attackType = Random.Range(1, 4); // Randomly choose between IsAttack1, IsAttack2, IsAttack3

        switch (attackType)
        {
            case 1:
                animator.SetTrigger("IsAttack1");
                break;
            case 2:
                animator.SetTrigger("IsAttack2");
                break;
            case 3:
                animator.SetTrigger("IsAttack3");
                break;
        }

        ActivateWeapon();
        Invoke(nameof(DeactivateWeapon), 0.5f); // Adjust activation duration as needed
    }

    private void ActivateWeapon()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }
    }

    private void DeactivateWeapon()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}
