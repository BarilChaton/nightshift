using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables

    public bool CanMove = true;
    public bool CanLook = true;
    private bool isUsingController = false;

    [Header("Controller support")]
    [SerializeField] private bool playWithController = false;

    [Header("Look params")]
    [SerializeField, Range(0.1f, 0.2f)] private float mouseLookSpeedX = 0.1f;
    [SerializeField, Range(0.1f, 0.2f)] private float mouseLookSpeedY = 0.1f;
    [SerializeField, Range(0.1f, 1.0f)] private float controllerLookSpeedX = 0.5f;
    [SerializeField, Range(0.1f, 1.0f)] private float controllerLookSpeedY = 0.5f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Movement params")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float gravityMultiplier = 30.0f;

    [Header("Head bobbing params")]
    [SerializeField] private float bobFrequency = 1.5f;
    [SerializeField] private float bobHeight = 0.5f;

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset PlayerActions;

    [Header("Footstep params")]
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private float stepInterval = 1f;

    public static PlayerController instance;
    private Camera playerCamera;
    private AudioSource audioSource;
    private CharacterController characterController;

    private float rotationX = 0;
    private float defaultYPos = 0;
    private float timer = 0;
    private float stepTimer = 1f;

    private Vector3 moveDirection;
    private Vector2 currentInput;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector2 smoothedLookInput;
    private Vector2 lookInputSmoothVelocity;

    private InputAction moveAction;
    private InputAction lookAction;

    #endregion

    #region BaseMethods
    private void Awake() {
        instance = this;
        playerCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

        moveAction = PlayerActions.FindActionMap("Player").FindAction("Move");
        lookAction = PlayerActions.FindActionMap("Player").FindAction("Look");

        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultYPos = playerCamera.transform.localPosition.y;

        StartCoroutine(CheckForControllers());
    }

    private void Update() {
        if (CanMove) {
            HandleMovementInput();
            HandleHeadBobbing();
            HandleStepSounds();

        }

        if (CanLook) {
            HandleMouseLook();
        }

        ApplyFinalMovements();
    }

    private void OnEnable() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        moveAction.Enable();
        lookAction.Enable();
    }

    private void OnDisable() {
        moveAction.Disable();
        lookAction.Disable();
    }

    #endregion

    #region Movement
    private void HandleMouseLook() {
        lookInput = lookAction.ReadValue<Vector2>();
        float lookSpeedX = (playWithController && isUsingController) ? controllerLookSpeedX : mouseLookSpeedX;
        float lookSpeedY = (playWithController && isUsingController) ? controllerLookSpeedY : mouseLookSpeedY;

        smoothedLookInput = Vector2.SmoothDamp(smoothedLookInput, lookInput, ref lookInputSmoothVelocity, 0.25f);

        rotationX -= smoothedLookInput.y * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, smoothedLookInput.x * lookSpeedX, 0);
    }

    private void HandleMovementInput() {
        moveInput = moveAction.ReadValue<Vector2>();
        
        currentInput = new Vector2(moveSpeed * moveInput.y, moveSpeed * moveInput.x);
        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleHeadBobbing() {
        if (characterController.velocity.magnitude > 0.1f) {
            float frequency = bobFrequency;
            timer += Time.deltaTime * frequency;
            timer %= Mathf.PI * 2;

            float newYPos = defaultYPos + Mathf.Sin(timer) * bobHeight;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, newYPos, playerCamera.transform.localPosition.z);
        } else {
            timer = 0;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos, playerCamera.transform.localPosition.z);
        }
    }

    private void HandleStepSounds() {
        if (characterController.velocity.magnitude > 0.01f && characterController.isGrounded) {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f) {
                PlayStepSound();
                stepTimer = stepInterval;
            }
        }
    }

    private void PlayStepSound() {
        if (stepSound != null) {
            audioSource.pitch = Random.Range(1.0f, 1.2f);
            audioSource.PlayOneShot(stepSound);
        }
    }

    private void ApplyFinalMovements() {
        if (!characterController.isGrounded) {
            moveDirection.y -= gravityMultiplier * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    #endregion

    #region Encounters

    public void FreezeMovement() {
        CanMove = false;
        CanLook = false;
        moveDirection.y = 0;
        moveDirection.x = 0;
        moveDirection.z = 0;
        characterController.Move(moveDirection);
    }    
    public void UnfreezeMovement() {
        CanMove = true;
        CanLook = true;
    }

    #endregion

    IEnumerator CheckForControllers() {
        while (true) {
            var controllers = Input.GetJoystickNames();

            if (!isUsingController && controllers.Length > 0) {
                isUsingController = true;
            } else if (isUsingController && controllers.Length == 0) {
                isUsingController = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }
    
}
