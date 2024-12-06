using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public int damage;
    public float range;
    public LayerMask enemyLayerMask; // Layer mask to identify enemies
    public float rotationSpeed = 5f;
    public Enemy closestEnemy;
    public bool onCooldown; //true if tower has fired too recently
    public int cooldown; //time between firing
    private int timeCooling;

    // Start is called before the first frame update
    void Start()
    {
        onCooldown = false;
        timeCooling = 0;
    }

    // Update is called once per frame
    void Update()//unity coroutine timer? look at rs pause screen code
    {
        UpdateClosestEnemy();
        RotateTowardsClosestEnemy();

        if (!onCooldown)
        {
            Attack();
            onCooldown = true;
        }
        else
        {
            timeCooling++;
            if (timeCooling >= cooldown)
            {
                onCooldown = false;
                timeCooling = 0;
            }
        }

        //Attack();
    }

    // This method will be implemented by each specific tower type
    public abstract void Attack();

    public void UpdateClosestEnemy() //borrowed from RS, then modified 
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

        foreach (Collider enemyCollider in hitEnemies)
        {
            float distance = Vector3.Distance(transform.position, enemyCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = enemyCollider.GetComponent<Enemy>();
            }
        }

        closestEnemy = nearest;
        
    }
    public void RotateTowardsClosestEnemy() 
    {
        if (closestEnemy == null)
            return;

        // Calculate direction to the closest enemy
        Vector3 direction = closestEnemy.transform.position - transform.position;
        //direction.y = 0; // Lock vertical movement (only rotate around Y-axis)

        if (direction != Vector3.zero)
        {
            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            transform.Find("ProjectileSpawn").rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
