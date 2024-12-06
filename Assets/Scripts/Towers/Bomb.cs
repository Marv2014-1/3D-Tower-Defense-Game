using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private int damage;
    private float speed;
    private float range;
    private Vector3 startPosition;
    private float blastRadius;
    public LayerMask enemyLayerMask; // Layer mask to identify enemies

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { //maybe should be changed to travel in an arc but math hard

        // Move the bomb forward in the direction it's facing
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.Self);

        // Destroy the bomb after it has traveled the specified range
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {//upon collision, should 'explode' dealing damage to all enemies in its blast radius
        Collider[] hitEnemies = Physics.OverlapSphere(this.transform.position, range, enemyLayerMask);

        for (int i = 0; i < hitEnemies.Length; i++)
        {
            Enemy enemy = hitEnemies[i].GetComponent<Enemy>();
            //enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
