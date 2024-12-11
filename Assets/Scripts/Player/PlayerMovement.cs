using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;
    private Animator anim;
    public Transform camPos;
    public Vector3 moveDir = Vector3.zero,
        velocity;
    public float slowSpeed = 6f, runSpeed = 15f, moveSpeed, turnSpeed = 0.1f,
        jumpTimer = 0.5f, jumpCut = 0.025f, divePower = 2.5f, launchPower, 
        highGravityScale = 2.5f, lowGravityScale = 2.0f;
    private float diveMulti = 1.0f, launchMulti = 1.0f;
    public float[] jumpPower = { 7f, 10f, 14f };
    private int jumps = 0, maxJumps = 3, jumpStage = 0;
    private bool canMove = true, dive = false;
    public bool isGrounded, holdJump, launch = false;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        moveSpeed = runSpeed;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
                anim.SetBool("isGrounded", false);
            } 
            else
            {
                dive = true;
            }
        }

        if (Input.GetButtonUp("Jump") && holdJump)
        {
            holdJump = false;
        }

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);
        }
    }

    void FixedUpdate()
    {
        if (!launch)
        {
            // This is part of my gross solution to finding out the exact frame the player lands after a jump. I did my best leave me alone.
            if (jumpStage == 1)
            {
                jumpStage = 2;
            }

            GetMovement();

            if (dive)
            {
                Dive();
            }
        }
        else
        {
            if (isGrounded)
            {
                launch = false;
            }

            Launch();
        }
        

        // Moves player according to movement direction then turn player towards direction
        cc.Move(moveDir * Time.deltaTime);
        InstantTurn();

        // Applies gravity to player
        ApplyGravity();
        cc.Move(velocity * Time.deltaTime);

        // Check if player is on the ground, if so and had just landed start jump timer for triple jump mechanic
        isGrounded = cc.isGrounded;
        if (isGrounded && jumpStage == 2)
        {
            StartCoroutine(JumpTimer());
            anim.SetBool("isGrounded", true);
            dive = false;
            diveMulti = 1.0f;
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

    // Ensures player smoothly turns to face the direction they're moving in, currently obsolete
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
        velocity.y += Mathf.Sqrt(jumpPower[jumps] * -2.0f * Physics.gravity.y);

        jumps++;
        if (jumps >= maxJumps)
        {
            jumps = 0;
        }

        jumpStage = 1;
        holdJump = true;
    }

    void Dive()
    {
        moveDir = moveDir * divePower * diveMulti;
        diveMulti += 0.05f;
        jumps = 0;
    }

    void Launch()
    {
        moveDir = moveDir * launchPower * launchMulti;
        launchMulti += 0.00f;
        jumps = 0;
    }

    // Adjusts player's y-value to simulate gravity
    void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0 || launch) // Player is on the ground
        {
            velocity.y = Physics.gravity.y * Time.deltaTime;
            anim.SetBool("isFalling", false);
            return;
        }

        if (velocity.y < 0) // Player is falling after apex of jump
        {
            velocity.y += Physics.gravity.y * (highGravityScale - 1) * Time.deltaTime;
            anim.SetBool("isFalling", true);
        } 
        else if (velocity.y > 0 && !holdJump) // Player is jumping but let go of jump button
        {
            velocity.y += Physics.gravity.y * (lowGravityScale - 1) * Time.deltaTime;
        } 
        else // Player is jumping and still holding jump button
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }
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