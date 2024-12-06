using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public int health = 100;

    public void TakeDamage(int damageAmount, Vector3 knockbackDirection, float knockbackForce)
    {
        health -= damageAmount;
        Debug.Log($"{name} took {damageAmount} damage. Remaining health: {health}");

        // Apply knockback effect
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }

        // Handle death
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{name} has died.");
        Destroy(gameObject);
    }
}
