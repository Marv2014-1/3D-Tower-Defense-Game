using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheryTower : Tower {
    [Header("Arrow Settings")]
    public GameObject arrowPrefab; // Assign the Arrow prefab in the Inspector
    public Transform arrowSpawnPoint; // Assign the spawn point in the Inspector
    public float arrowSpeed = 10f;
    public float arrowRange = 20f;


    // Start is called before the first frame update
    //void Start()
    //{
    //}

    // Update is called once per frame
    //void Update()
    //{    
    // }
    public override void Attack() //some code borrowed from RS
    {
        // Instantiate an arrow at the spawn point with the same rotation as the bow
        GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, transform.rotation);

        Arrow arrow = arrowInstance.GetComponent<Arrow>();

        if (arrow != null)
        {
            arrow.SetDamage(damage);
            arrow.SetSpeed(arrowSpeed);
            arrow.SetRange(arrowRange);

            Debug.Log("Bow shot an arrow.");
        }
        else
        {
            Debug.LogError("Arrow prefab does not have an Arrow component.");
        }
    }


}
