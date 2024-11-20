using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public int damage;
    public float range;
    public float cooldown; //time between firing
    public LayerMask enemyLayerMask; // Layer mask to identify enemies
    public float rotationSpeed = 5f;
    private Enemy closestEnemy;  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClosestEnemy();
        RotateTowardsClosestEnemy();
        Attack();
    }

    // This method will be implemented by each specific tower type
    public abstract void Attack();

    private void UpdateClosestEnemy() //borrowed from RS, then modified 
    {
        // Find all enemies within range
        Collider[] hitEnemies = Physics.OverlapSphere(this.transform.position, range, enemyLayerMask); //change from 2D
        //update to be based on proximity to the main base? ^

        if (hitEnemies.Length == 0)
        {
            closestEnemy = null;
            return;
        }

        // Identify the closest enemy
        float closestDistance = Mathf.Infinity;
        Enemy nearest = null;

        foreach (SphereCollider enemyCollider in hitEnemies)
        {
            float distance = Vector2.Distance(this.transform.position, enemyCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = enemyCollider.GetComponent<Enemy>();
            }
        }

        closestEnemy = nearest;
    }
    private void RotateTowardsClosestEnemy() //from RS
    {
        if (closestEnemy == null)
            return;

        Vector2 direction = closestEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust the angle based on your bow's sprite orientation if needed 
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
