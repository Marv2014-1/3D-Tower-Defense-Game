using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheryTower : Tower
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab; // Assign the Arrow prefab in the Inspector
    public Transform arrowSpawnPoint; // Assign the spawn point in the Inspector
    public float arrowSpeed = 10f;
    public float arrowRange = 20f;


    // Start is called before the first frame update
    //void Start()
    //{
    //}

    //Update is called once per frame
    /*void Update()
    {
        UpdateClosestEnemy();
        RotateTowardsClosestEnemy();
        Debug.DrawLine(arrowSpawnPoint.position, closestEnemy.transform.position, Color.red, 0.5f);
        Attack();
    }*/
    public override void Attack() //some code borrowed from RS
    {
        if (closestEnemy == null)
            return;

        // Calculate the direction from the spawn point to the closest enemy
        Vector3 direction = (closestEnemy.transform.position - arrowSpawnPoint.position).normalized;

        // Instantiate the arrow
        GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);

        // Rotate the arrow to face the direction of the enemy
        // arrowInstance.transform.rotation = Quaternion.LookRotation(direction);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion correction = Quaternion.Euler(-90, 0, 0); // Adjust as necessary

        arrowInstance.transform.rotation = lookRotation * correction;


        // Configure the arrow's behavior
        Arrow arrow = arrowInstance.GetComponent<Arrow>();
        if (arrow != null)
        {
            arrow.SetDamage(damage);
            arrow.SetSpeed(arrowSpeed);
            arrow.SetRange(arrowRange);

            Debug.Log("ArcheryTower shot an arrow.");
        }
        else
        {
            Debug.LogError("Arrow prefab does not have an Arrow component.");
        }
    }
}


