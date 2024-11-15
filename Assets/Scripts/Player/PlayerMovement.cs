using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public Transform camPos;
    public float slowSpeed = 6f,
        runSpeed = 15f,
        maxSpeed = 20f,
        brakeSpeed = 0.25f,
        turnSpeed = 2f,
        jumpTimer = 0.5f,
        gravityScale = 1.0f,
        gravityBase = -9.81f;
    public float[] jumpPower = { 7f, 10f, 14f };
    private int jumps = 0,
        maxJumps = 3;

    private Vector3 moveDirection = Vector3.zero,
        turnDir;
    private Rigidbody rb;

    private bool canMove = true,
        startJump = false;
    public bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        turnDir = camPos.TransformDirection(Vector3.forward);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            startJump = true;
        }
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            GetMovement();
            Brake();
        }

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(moveDirection);
        }

        if (startJump)
        {
            Jump();
        }

        if (!isGrounded)
        {
            rb.AddForce(gravityBase * gravityScale * Vector3.up, ForceMode.Acceleration);
        } 
        else
        {
            rb.AddForce(Vector3.up * -2f, ForceMode.Acceleration);
        }
        
        //Debug.Log(rb.velocity.magnitude);
    }

    void GetMovement()
    {
        Vector3 forward = camPos.TransformDirection(Vector3.forward);
        Vector3 right = camPos.TransformDirection(Vector3.right);

        bool isSlow = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isSlow ? slowSpeed : runSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedZ = canMove ? (isSlow ? slowSpeed : runSpeed) * Input.GetAxis("Horizontal") : 0;
        moveDirection = (forward * curSpeedX + right * curSpeedZ);

        if (moveDirection != Vector3.zero)
        {
            Turn();
        }
    }

    void Brake()
    {
        if (Input.GetAxis("Vertical") == 0f && Input.GetAxis("Horizontal") == 0f && rb.velocity.magnitude > 1f)
        {
            rb.drag += brakeSpeed;
        }
        else
        {
            rb.drag -= brakeSpeed;
        }

        rb.drag = Mathf.Clamp(rb.drag, 0f, 100f);
    }

    void Turn()
    {
        
        if (Input.GetAxis("Vertical") > 0)
        {
            turnDir = camPos.TransformDirection(Vector3.forward);
        } 
        else if (Input.GetAxis("Vertical") < 0)
        {
            turnDir = -camPos.TransformDirection(Vector3.forward);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            turnDir += camPos.TransformDirection(Vector3.right);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            turnDir += -camPos.TransformDirection(Vector3.right);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(turnDir), turnSpeed * Time.deltaTime);
    }

    void Jump()
    {
        Debug.Log("Jump");
        startJump = false;
        rb.drag = 0;
        rb.AddForce(Vector3.up * jumpPower[jumps], ForceMode.Impulse);

        jumps++;
        if (jumps >= maxJumps)
        {
            jumps = 0;
        }
    }

    IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(1);

        if (isGrounded)
        {
            jumps = 0;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = true;
            StartCoroutine(JumpTimer());
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = false;
        }
    }
}