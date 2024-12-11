using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheryTower : Tower
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab; // Assign the Arrow prefab in the Inspector
    private Transform arrowSpawnPoint; 
    public float arrowSpeed = 10f;
    public float arrowRange = 20f;


    // Start is called before the first frame update
    void Start()
    {
        arrowSpawnPoint = transform.Find("ProjectileSpawn");
        cooldown = 500;
        range = 30;
        onCooldown = false;
        enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    public override void Attack()
    {
        if (closestEnemy == null)
            return;

        Vector3 offset = new Vector3(0, 2, 0);
        Vector3 direction = (closestEnemy.transform.position - arrowSpawnPoint.position + offset).normalized;

        GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.LookRotation(direction));
        Arrow arrow = arrowInstance.GetComponent<Arrow>();
        if (arrow != null)
        {
            arrow.SetDamage(damage);
            arrow.SetSpeed(arrowSpeed);
            arrow.SetRange(arrowRange);
            arrow.SetTarget(closestEnemy); // Assign the target enemy
        }
        else
        {
            Debug.LogError("Arrow prefab does not have an Arrow component.");
        }
    }

}


