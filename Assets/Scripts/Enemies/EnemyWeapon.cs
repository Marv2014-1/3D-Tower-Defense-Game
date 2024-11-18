using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [HideInInspector] public int ArrowDamage;
    private Collider hitboxCollider;

    public float knockbackForce = 5f;
    protected Transform playerTransform;
    private float playerHealth=100;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        hitboxCollider.enabled = false;
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void OnTriggerEnter(Collider collision)
    {
        MeleeEnemy enemy = GetComponentInParent<MeleeEnemy>();
        int damageAmount = enemy != null ? enemy.GetAttackDamage() : 0;

        if (collision.CompareTag("Player"))
        {
            // Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;
            // collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmount, knockbackDirection, knockbackForce);
            // collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmount, knockbackDirection, knockbackForce);
            // minus player health
            playerHealth -= damageAmount;


            //log player health
            Debug.Log($"Player health: {playerHealth}");
            
        }
    }

    public void ActivateWeapon()
    {
        hitboxCollider.enabled = true;
    }

    public void DeactivateWeapon()
    {
        hitboxCollider.enabled = false;
    }
}
