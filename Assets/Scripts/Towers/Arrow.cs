using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage;
    private float speed;
    private float range;
    private Vector3 startPosition;
    private Rigidbody rb;
    private Enemy target;
    private Vector3 velocity;

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

    public void SetSpeed(float speedAmount)
    {
        speed = speedAmount;
    }

    public void SetRange(float rangeAmount)
    {
        range = rangeAmount;
    }

    public void SetTarget(Enemy enemyTarget)
    {
        target = enemyTarget;
    }

    /// Sets the initial velocity of the arrow towards the interception point.
    public void SetVelocity(Vector3 initialVelocity)
    {
        velocity = initialVelocity;
    }

    private void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true; // We'll handle movement manually

        Collider col = GetComponent<Collider>();
        if (!col)
        {
            col = gameObject.AddComponent<BoxCollider>();
        }
        col.isTrigger = true;

        // Optionally, rotate the arrow to align with its velocity
        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity.normalized);
        }
    }

    private void Update()
    {
        // Move the arrow forward based on its velocity
        transform.position += velocity * Time.deltaTime;

        // Optionally, add slight homing adjustment if target is still alive
        if (target != null)
        {
            Vector3 toTarget = target.transform.position - transform.position;
            if (toTarget.sqrMagnitude > 0.1f)
            {
                // Adjust velocity slightly towards the target
                Vector3 direction = toTarget.normalized;
                float adjustmentFactor = 0.1f; // Adjust as needed for smoothness
                velocity = Vector3.Lerp(velocity, direction * speed, adjustmentFactor * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(velocity.normalized);
            }
        }

        // Destroy the arrow if it exceeds its range
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && enemy == target)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
