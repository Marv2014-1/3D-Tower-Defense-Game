using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 3f;
    private float nextAttackTime = 0f;
    [SerializeField] private float attackCooldown = 2f; // Adjust the cooldown value as needed

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

        if (targetTransform == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

        if (distanceToTarget <= attackRange && Time.time >= nextAttackTime)
        {
            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
        else
        {
            MoveTowardsTarget();
        }
    }

    private void PerformAttack()
    {
        // if (animator == null || weaponCollider == null) return;

        // // canMove = false;
        // string attackTrigger = CheckAttackDirection();
        // Attack(attackTrigger, 0.1f, 0.3f); // Adjust activation/deactivation times as needed
    }

    private string CheckAttackDirection()
    {
        Vector3 directionToTarget = targetTransform.position - transform.position;
        if (Mathf.Abs(directionToTarget.x) > Mathf.Abs(directionToTarget.z))
        {
            return directionToTarget.x > 0 ? "TriggerAttackSide" : "TriggerAttackSide";
        }
        else
        {
            return directionToTarget.z > 0 ? "TriggerAttackFront" : "TriggerAttackBack";
        }
    }

    public void Attack(string triggerName, float activateTime, float deactivateTime)
    {
        // animator.SetTrigger(triggerName);
        Invoke(nameof(ActivateWeapon), activateTime);
        Invoke(nameof(DeactivateWeapon), deactivateTime);
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
