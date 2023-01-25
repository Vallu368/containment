using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true; //if player is in control
    private bool isSprinting => canSprint && Input.GetKey(sprintKey); //is true if canSprint is true and sprintKey is pressed
    private bool shouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded; //same as above but jump
    private bool shouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;



    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;



    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 1.5f;


    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f; //how far up you can look
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f; //how far down you can look

    [Header("Jumping Parameters")]
    [SerializeField] private float gravity = 30.0f;
    [SerializeField] private float jumpForce = 8.0f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;



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
            if (canJump)
            {
                HandleJump();
            }
            if (canCrouch)
            {
                HandleCrouch();
            }
            ApplyFinalMovements();
        }
    }
    private void HandleMovementInput() //keyboard controls :3 playerinput + speed
    {
        currentInput = new Vector2((isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal")); //inputs + speed
        //if iscrouching gives crouchspeed, isSprinting then gives sprintspeed, else gives walkspeed

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
    private void HandleJump()
    {
        if(shouldJump)
        {
            moveDirection.y = jumpForce;
        }
    }
    private void HandleCrouch()
    {
        if(shouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }
    }
    private void ApplyFinalMovements()
    {
        if(!characterController.isGrounded) //if not grounded you fall
            moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);

    }
    private IEnumerator CrouchStand() //lerping height and centerpoint to stand or crouch
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f)) //if already crouching, raycast 1 unit up, if it hits anything the rest of the coroutine doesnt happen
        {
            Debug.Log("cant stand up, something blocking");
            yield break;
        }
        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight; //is already crouching target is standingheight, if standing target is crouchingheight
        float currentHeight = characterController.height; //current height
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter; //same as earlier but center instead of height
        Vector3 currentCenter = characterController.center; //current center

        while(timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch); //goes from currentheight to targetheight in timetocrouch
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch); //same as above but center
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter; //sanity checks


        isCrouching = !isCrouching; //toggle iscrouching to opposite

        duringCrouchAnimation = false;
    }
}
