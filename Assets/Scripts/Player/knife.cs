using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class KnifeAttack : MonoBehaviour
{
    public int damage = 20;

    private MeshCollider meshCollider;

    private void Start()
    {
        // Get the MeshCollider on this object
        meshCollider = GetComponent<MeshCollider>();

        // Ensure the MeshCollider is set as a trigger
        if (!meshCollider.convex || !meshCollider.isTrigger)
        {
            meshCollider.convex = true;
            meshCollider.isTrigger = true;
            Debug.LogWarning("MeshCollider has been set to Convex and Trigger mode.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Try to find the Enemy component and deal damage
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !enemy.IsDead())
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Knife hit {other.name}, dealt {damage} damage.");
            }
        }
    }
}
