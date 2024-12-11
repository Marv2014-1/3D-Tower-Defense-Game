using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinedTower : MonoBehaviour
{
    public GameObject towerPrefab;
    public GameObject arrowPrefab; // Assign the Arrow prefab in the Inspector
   // public Transform arrowSpawnPoint; 
  //add collider that detects if player is standing near using OnTriggerEnter

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.E)){
        UpgradeTower();
      }

    }

    void FixedUpdate(){
        
    }

    void UpgradeTower(){
        GameObject towerInstance = Instantiate(towerPrefab, transform.position, transform.rotation);

        ArcheryTower tower = towerInstance.GetComponent<ArcheryTower>();
        tower.arrowPrefab = arrowPrefab;
        //tower.arrowSpawnPoint = arrowSpawnPoint;
        Debug.Log("Tower Upgraded");
        Destroy(gameObject);
    }
}

   

   
       
