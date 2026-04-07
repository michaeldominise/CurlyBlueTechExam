using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerInputController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform cameraPivot;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;

    [Header("Look")]
    [SerializeField] private float lookSensitivity = 2f;

    [Header("Look Limits")]
    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Spring Settings")]
    [SerializeField] private float springStrength = 120f;
    [SerializeField] private float damping = 20f;

    private Rigidbody _rb;

    // Input
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isSprinting;

    // Rotation state
    private float _yaw;
    private float _pitch;

    private float _targetYaw;
    private float _targetPitch;

    private float _yawVelocity;
    private float _pitchVelocity;

    // ------------------------
    // INIT
    // ------------------------

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        if (playerBody == null)
            playerBody = transform;
    }

    private void Start()
    {
        LockCursor();
    }

    // ------------------------
    // INPUT
    // ------------------------

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        _isSprinting = context.ReadValueAsButton();
    }

    // ------------------------
    // UPDATE (LOOK + CURSOR)
    // ------------------------

    private void Update()
    {
        HandleCursor();
        HandleLook();
    }

    private void HandleCursor()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UnlockCursor();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            Cursor.lockState != CursorLockMode.Locked)
        {
            LockCursor();
        }
    }

    private void HandleLook()
    {
        // Disable look when cursor unlocked
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        // Update targets from input
        _targetYaw += _lookInput.x * lookSensitivity;
        _targetPitch -= _lookInput.y * lookSensitivity;

        _targetPitch = Mathf.Clamp(_targetPitch, minPitch, maxPitch);

        float dt = Time.deltaTime;

        // --- Yaw spring ---
        float yawForce = (_targetYaw - _yaw) * springStrength;
        _yawVelocity += yawForce * dt;
        _yawVelocity *= Mathf.Exp(-damping * dt);
        _yaw += _yawVelocity * dt;

        // --- Pitch spring ---
        float pitchForce = (_targetPitch - _pitch) * springStrength;
        _pitchVelocity += pitchForce * dt;
        _pitchVelocity *= Mathf.Exp(-damping * dt);
        _pitch += _pitchVelocity * dt;

        // Apply rotations
        playerBody.rotation = Quaternion.Euler(0f, _yaw, 0f);
        cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    // ------------------------
    // FIXED UPDATE (MOVEMENT)
    // ------------------------

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 input = new Vector3(_moveInput.x, 0f, _moveInput.y);

        if (input.sqrMagnitude < 0.01f)
            return;

        float speed = _isSprinting
            ? moveSpeed * sprintMultiplier
            : moveSpeed;

        Vector3 moveDir =
            playerBody.forward * input.z +
            playerBody.right * input.x;

        moveDir.y = 0f;
        moveDir.Normalize();

        Vector3 movement = moveDir * (speed * Time.fixedDeltaTime);

        _rb.MovePosition(_rb.position + movement);
    }

    // ------------------------
    // CURSOR CONTROL
    // ------------------------

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}