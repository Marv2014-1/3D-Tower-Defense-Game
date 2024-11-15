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
    public float turnSpeed = 10f;
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

            transform.position = Vector3.Lerp(transform.position, targetPos + playerBody.position, speed);
            transform.LookAt(playerBody.transform.position);
            compass.localRotation = Quaternion.Euler(-25, 0, 0);
        }    
    }

    void Turn()
    {
        targetPos = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * targetPos;
    }

    void SwitchCam()
    {
        mapCam = !mapCam;
        transform.position = mapCamPos;
        transform.eulerAngles = mapCamAng;
        compass.eulerAngles = Vector3.zero;
    }
}
