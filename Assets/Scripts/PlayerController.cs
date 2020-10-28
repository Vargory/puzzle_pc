using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Goes on the Inspector
    [Header("Camera Settings")]
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private float mouseSensitivity = 3.5f;
    [SerializeField] [Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.03f;
    [SerializeField] private bool lockCursor = true;

    
    [Header("Movement Settings")]
    [SerializeField] private CharacterController controller = null;
    [SerializeField] [Range(0.0f, 0.5f)] private float moveSmoothTime = 0.3f;
    [SerializeField] private float walkSpeed = 0f, runSpeed = 0f;
    [SerializeField] private float runSpeedUp = 0f;
    [SerializeField] private KeyCode _runKey = KeyCode.LeftShift;
    public bool canMove= true;

    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] private float mass = 1.0f;
    [SerializeField] private float _jumpMultiplier = 0f;
    [SerializeField] private AnimationCurve _jumpFallOff = null;

    [Header("Bobbing Settings")] 
    [SerializeField] Transform Vcam;
    [SerializeField] private Vector3 camOrigin;
    [SerializeField] private float idleCounter;
    [SerializeField] private float movementCounter;

    //Camera related variables
    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    
    //Movement related variables
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    private float movementSpeed = 0f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 lastPosition;
    
    //Jump related variables
    private bool isJumping;
    
    //Bobbing related variables
    private Vector3 targetBobPosition;
    
    private void Awake()
    {
        camOrigin = Vcam.localPosition;
    }
    
    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    private void Update()
    {
        lastPosition = controller.GetComponent<Transform>().position;
        if (canMove)
        {
            UpdateMouseLook();
            UpdateMovement(); 
        }
        UpdateHeadBob();
    }

    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * (currentMouseDelta.x * mouseSensitivity));
    }

    private void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (controller.isGrounded) 
            velocityY = 0.0f;

        velocityY += gravity * mass * Time.deltaTime;
        velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * movementSpeed + Vector3.up * velocityY;
            
        controller.Move(velocity * Time.deltaTime);
            
            
        SetMovementSpeed();
        JumpInput();
    }

    void HeadBob(float p_z, float p_x_intensity, float p_y_intensity)
    {
        targetBobPosition = camOrigin + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intensity, 0);
    }

    void UpdateHeadBob()
    {
        if (lastPosition == GetComponent<Transform>().position)
        {
            Vcam.localPosition = Vector3.Lerp(Vcam.localPosition, targetBobPosition, Time.deltaTime * 4f);
            HeadBob(idleCounter, 0.015f, 0.015f);
            idleCounter += Time.deltaTime * 1.5f;
        }
        else
        {
            Vcam.localPosition = Vector3.Lerp(Vcam.localPosition, targetBobPosition, Time.deltaTime * 6f);
            HeadBob(movementCounter, 0.03f, 0.03f);
            movementCounter += Time.deltaTime * 3f;
        }
        
        if (Input.GetKey(_runKey))
        {
            Vcam.localPosition = Vector3.Lerp(Vcam.localPosition, targetBobPosition, Time.deltaTime * 10f);
            HeadBob(movementCounter, 0.45f, 0.45f);
            movementCounter += Time.deltaTime * 4.5f;
        }
    }
    
    void SetMovementSpeed()
    {
        if (Input.GetKey(_runKey))
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runSpeedUp);
        else
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime);
    }
    
    private void JumpInput()
    {
        if (Input.GetKeyDown(_jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }
    
    private IEnumerator JumpEvent()
    {
        //Debug.Log(controller.slopeLimit);
        controller.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        do {
            float _jumpForce = _jumpFallOff.Evaluate(timeInAir);
            controller.Move(Vector3.up * ((_jumpForce * _jumpMultiplier)* Time.deltaTime));
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);

        controller.slopeLimit = 45.0f;
        isJumping = false;
    }
}