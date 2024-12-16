using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage;
    private float speed;
    private float range;
    private Vector3 startPosition;
    private Rigidbody rb;
    private Enemy target;
    public float homingStrength = 2f;

    // References to the tip and tail
    public Transform tip;
    public Transform tail;

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

    private void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;

        Collider col = GetComponent<Collider>();
        if (!col)
        {
            col = gameObject.AddComponent<BoxCollider>();
        }
        col.isTrigger = true;
    }

    private void Update()
    {
        if (target != null)
        {
            // Calculate direction from arrow's position to target
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            Debug.DrawLine(transform.position, target.transform.position, Color.red, 0.5f);
            Debug.DrawRay(transform.position, directionToTarget, Color.green, 0.5f);
            // Determine the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * homingStrength);
        }

        // Move the arrow forward in its local space
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

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
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
