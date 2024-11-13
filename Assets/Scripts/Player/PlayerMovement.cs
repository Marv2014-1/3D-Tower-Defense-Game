using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Camera Settings")]
    public Transform playerCamera;
    public float cameraDistance = 0.0f; // Not necessary for first-person but kept if needed

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }

        // Ensure camera is properly positioned
        playerCamera.localPosition = new Vector3(0, 1.6f, 0); // Adjust as needed
    }

    void Update()
    {
        // Ground Check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }

        // Input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Debug: Check input values
        Debug.Log($"moveX: {moveX}, moveZ: {moveZ}");

        // Calculate movement relative to camera's forward and right
        Vector3 move = playerCamera.right * moveX + playerCamera.forward * moveZ;
        move.y = 0; // Ensure movement is horizontal

        controller.Move(move * speed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("Jump!");
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Debug: Check velocity
        Debug.Log($"Velocity Y: {velocity.y}");
    }
}

