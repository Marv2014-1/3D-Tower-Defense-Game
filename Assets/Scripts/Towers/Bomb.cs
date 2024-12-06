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
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {//upon collision, should 'explode' dealing damage to all enemies in its blast radius
        Collider[] hitEnemies = Physics.OverlapSphere(this.transform.position, range, enemyLayerMask);

        for (int i = 0; i < hitEnemies.Length; i++)
        {
            Enemy enemy = hitEnemies[i].GetComponent<Enemy>();
            //enemy.TakeDamage(damage);
        }
    }
}
