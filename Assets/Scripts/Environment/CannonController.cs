using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float power = 10f, xAngle = 20f;
    private int contacts = 0;
    private Vector3 dir;
    private GameObject player;
    public Transform launchPoint;
    private bool launchNow = false;

    // Start is called before the first frame update
    void Start()
    {
        dir = transform.TransformDirection(Vector3.forward);
        dir *= power;
    }

    void Update()
    {
        if (contacts > 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                launchNow = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (launchNow)
        {
            launchNow = false;
            Launch();
        }
    }

    void Launch()
    {
        PlayerMovement move = player.GetComponent<PlayerMovement>();
        player.transform.position = launchPoint.transform.position;
        move.launchPower = power;
        move.moveDir = dir;
        move.launch = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;
        contacts++;
    }

    private void OnTriggerExit()
    {
        contacts--;
    }
}
