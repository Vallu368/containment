using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true; //if player is in control
    private bool isSprinting => canSprint && Input.GetKey(sprintKey); //is true if canSprint is true and sprintKey is pressed
    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f; //how far up you can look
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f; //how far down you can look

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection; //final direction
    private Vector2 currentInput;

    private float rotationX = 0;
    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (CanMove) //if !canmove nothing happens here :3
        {
            HandleMovementInput();
            HandleMouseLook();
            ApplyFinalMovements();
        }
    }
    private void HandleMovementInput() //keyboard controls :3 playerinput + speed
    {
        currentInput = new Vector2((isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal")); //inputs + speed
        //if isSprinting then gives sprintspeed, else gives walkspeed

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y); //orientation + current dir
        moveDirection.y = moveDirectionY;
    }
    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit); //clamp between looklimits so you cant turn camera 360
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0); //apply rotation x
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0); //sideways rotation
    }
    private void ApplyFinalMovements()
    {
        if(!characterController.isGrounded) //if not grounded you fall
            moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);


    }
}
