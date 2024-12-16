using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuinedTower : MonoBehaviour
{
    public TMP_Text displayText; //assign ui element in inspector
    public GameObject towerPrefab;
    public GameObject arrowPrefab; // Assign the Arrow prefab in the Inspector
    private bool playerNear;
    // Start is called before the first frame update
    void Start()
    {
        playerNear = false;
        if (displayText != null)
        {
            displayText.gameObject.SetActive(false);
        }
        else{
          Debug.Log("displayText not assigned in editor");
        }
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
        if (displayText != null)
        {
            displayText.text = "Press E to upgrade tower! (Cost: 100 Gold)"; // Set the text
            displayText.gameObject.SetActive(true); // Show the UI text
        }
      }
    }

    void OnTriggerExit(Collider collision){
      if (collision.CompareTag("Player")){
        playerNear = false;
        Debug.Log("Player left upgrade range");
        if (displayText != null)
        {
            displayText.gameObject.SetActive(false); // Hide the UI text
        }
      }
    }
}

   

   
       
