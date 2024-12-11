using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinedTower : MonoBehaviour
{
    public GameObject towerPrefab;
    public GameObject arrowPrefab; // Assign the Arrow prefab in the Inspector
    private bool playerNear;
    // Start is called before the first frame update
    void Start()
    {
        playerNear = false;
    }

    // Update is called once per frame
    void Update()
    {
      if(playerNear){
        if (Input.GetKeyDown(KeyCode.E)){
          UpgradeTower();
        }
      }
    }

    void UpgradeTower(){
        GameObject towerInstance = Instantiate(towerPrefab, transform.position, transform.rotation);

        ArcheryTower tower = towerInstance.GetComponent<ArcheryTower>();
        tower.arrowPrefab = arrowPrefab;
        //tower.arrowSpawnPoint = arrowSpawnPoint;
        Debug.Log("Tower Upgraded");
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collision){
      if (collision.CompareTag("Player")){
        playerNear = true;
        Debug.Log("Player in upgrade range");
      }
    }

    void OnTriggerExit(Collider collision){
      if (collision.CompareTag("Player")){
        playerNear = false;
        Debug.Log("Player left upgrade range");
      }
    }
}

   

   
       
