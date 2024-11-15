using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;
    public Transform camPos;
    private Vector3 moveDir = Vector3.zero,
        velocity;
    public float slowSpeed = 6f,
        runSpeed = 15f,
        moveSpeed,
        turnSpeed = 0.1f,
        jumpTimer = 0.5f,
        gravityScale = 1.0f,
        gravityBase = -9.81f;
    public float[] jumpPower = { 7f, 10f, 14f };
    private int jumps = 0,
        maxJumps = 3,
        jumpStage = 0;
    private bool canMove = true;
    public bool isGrounded;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        moveSpeed = runSpeed;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // This is my gross solution to finding out the exact frame the player lands after a jump. I did my best leave me alone.
        if (jumpStage == 1)
        {
            jumpStage = 2;
        }
        
        GetMovement();

        if (isGrounded)
        {
            if (moveDir != Vector3.zero)
            {
                //Turn();
            }

            if (velocity.y < 0)
            {
                velocity.y = 0;
            }
        }

        // Moves player according to movement direction
        cc.Move(moveDir * Time.deltaTime);
        InstantTurn();
        // Applies gravity to player
        velocity.y += gravityBase * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        isGrounded = cc.isGrounded;
        if (isGrounded && jumpStage == 2)
        {
            StartCoroutine(JumpTimer());
        }
    }

    void GetMovement()
    {
        // Get direction camera is facing
        Vector3 forward = camPos.TransformDirection(Vector3.forward);
        Vector3 right = camPos.TransformDirection(Vector3.right);

        // Get player inputs, combine with camera direction to get base direction of movement
        bool isSlow = Input.GetKey(KeyCode.LeftShift) || !isGrounded;
        float curSpeedX = canMove ? Input.GetAxis("Vertical") : 0;
        float curSpeedZ = canMove ? Input.GetAxis("Horizontal") : 0;
        moveDir = (forward * curSpeedX + right * curSpeedZ);

        // Normalize movement if it exceeds a magnitude of 1 (prevents speed bug when moving diagonally)
        if (moveDir.sqrMagnitude > 1f)
        {
            moveDir.Normalize();
        }

        // Apply either run or slow speed, slow speed is used for moving while jumping
        moveSpeed = isSlow ? Mathf.Lerp(moveSpeed, slowSpeed, 0.12f) : Mathf.Lerp(moveSpeed, runSpeed, 0.12f);
        moveDir *= moveSpeed;
    }

    // Ensures player smoothly turns to face the direction they're moving in
    void Turn()
    {
        Quaternion lookDir = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookDir, Time.deltaTime * turnSpeed);
    }

    // Faces the player in the direction they're moving in immediately, used for jumping
    void InstantTurn()
    {
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
        }
    }

    // Adjusts player's y-value to jump, also tracks number of jumps for triple jump
    void Jump()
    {
        InstantTurn();
        velocity.y += Mathf.Sqrt(jumpPower[jumps] * -2.0f * gravityBase);

        jumps++;
        if (jumps >= maxJumps)
        {
            jumps = 0;
        }

        jumpStage = 1;
    }

    // Tracks how long the player has been on the ground since last jump, needed for triple jump
    IEnumerator JumpTimer()
    {
        jumpStage = 0;

        yield return new WaitForSeconds(0.33f);

        if (isGrounded)
        {
            jumps = 0;
        }
    }
}