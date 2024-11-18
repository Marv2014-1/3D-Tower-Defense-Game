using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 3f;
    private float nextAttackTime = 0f;

    [SerializeField] private Hitbox hitbox;

    protected override void Update()
    {
        base.Update();

        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void PerformAttack()
    {
        if (animator == null || hitbox == null) return;

        canMove = false;
        string attackTrigger = CheckAttackDirection();
        Attack(attackTrigger, 0.1f, 0.3f);
    }

    private string CheckAttackDirection()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.z))
        {
            return directionToPlayer.x > 0 ? "TriggerAttackSide" : "TriggerAttackSide";
        }
        else
        {
            return directionToPlayer.z > 0 ? "TriggerAttackFront" : "TriggerAttackBack";
        }
    }

    public void Attack(string triggerName, float activateTime, float deactivateTime)
    {
        animator.SetTrigger(triggerName);
        Invoke(nameof(ActivateHitbox), activateTime);
        Invoke(nameof(DeactivateHitbox), deactivateTime);
    }

    private void ActivateHitbox()
    {
        hitbox?.ActivateHitbox();
    }

    private void DeactivateHitbox()
    {
        hitbox?.DeactivateHitbox();
    }
}
