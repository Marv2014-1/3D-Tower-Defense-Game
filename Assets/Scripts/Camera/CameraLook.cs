using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public Transform playerBody;
    public Transform compass;
    private Vector3 initPos,
        targetPos,
        velocity = Vector3.one,
        mapCamPos = new Vector3(0, 200, -85),
        mapCamAng = new Vector3(70, 0, 0);
    public float speed = 0.25f;
    public float lookSpeed = 0.25f;
    public float lookXLimit = 7.5f;
    public float lookXMod = 25f;
    public float lookYLimit = 25f;
    public float turnSpeed = 2f;
    private float rotationX = 25;
    private float rotationY = 0;
    private bool mapCam = false;

    void Start()
    {
        initPos = transform.position - playerBody.position;
        targetPos = initPos;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!mapCam)
        {
            if (Input.GetMouseButton(1))
            {

                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit + lookXMod, lookXMod);

                rotationY += -Input.GetAxis("Mouse X") * lookSpeed;
                rotationY = Mathf.Clamp(rotationY, -lookYLimit, lookYLimit);
            }
            else
            {
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;

                //rotationX = Mathf.Lerp(rotationX, lookXMod, speed / 4);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotationY = 0;
            }

        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchCam();
        }
    }

    void FixedUpdate()
    {
        if (!mapCam)
        {
            Turn();

            // Keeps the camera in the same position relative to the player
            transform.position = Vector3.Lerp(transform.position, targetPos + playerBody.position, speed);
            transform.LookAt(playerBody.transform.position);

            // Ensures the compass's world rotation stays at (0, 0, 0), this is needed for the player's movement
            compass.eulerAngles = new Vector3(0, compass.eulerAngles.y, compass.eulerAngles.z);
        }    
    }

    // Circles the camera around the player using mouse movement
    void Turn()
    {
        targetPos = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * targetPos;
    }

    // Switches the camera from a 3rd person view to a bird's eye view
    void SwitchCam()
    {
        mapCam = !mapCam;
        transform.position = mapCamPos;
        transform.eulerAngles = mapCamAng;
        compass.eulerAngles = Vector3.zero;
    }
}
