using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerInputController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float smoothTime = 0.03f;
    public Transform cameraPivot;

    [Header("Jump")]
    public float jumpForce = 5f;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isSprinting;

    private Rigidbody rb;

    // Rotation
    private float targetXRotation;
    private float currentXRotation;
    private float xVelocity;
    private float yVelocity;
    private float targetYaw;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        LockCursor();
        Application.runInBackground = true; // helps reduce editor FPS drop
    }

    // INPUT
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        HandleCursorState();
        HandleMouseLook(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        HandleMovement(Time.fixedDeltaTime);
        ApplyRotation();
    }

    private void HandleMovement(float dt)
    {
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 moveDir = transform.right * moveInput.x + transform.forward * moveInput.y;
        Vector3 step = moveDir * (speed * dt);

        rb.MovePosition(rb.position + step);
    }

    private void HandleMouseLook(float dt)
    {
        float sensitivity = mouseSensitivity * 100f;

        float mouseX = lookInput.x * sensitivity * dt;
        float mouseY = lookInput.y * sensitivity * dt;

        targetXRotation -= mouseY;
        targetXRotation = Mathf.Clamp(targetXRotation, -80f, 80f);

        targetYaw += mouseX;

        currentXRotation = Mathf.SmoothDamp(
            currentXRotation,
            targetXRotation,
            ref xVelocity,
            smoothTime
        );
    }

    private void ApplyRotation()
    {
        float smoothYaw = Mathf.SmoothDampAngle(
            rb.rotation.eulerAngles.y,
            targetYaw,
            ref yVelocity,
            smoothTime
        );

        cameraPivot.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
        rb.MoveRotation(Quaternion.Euler(0f, smoothYaw, 0f));
    }

    // 🔒 Cursor Handling
    private void HandleCursorState()
    {
        // Click to lock (FPS standard)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            LockCursor();
        }

        // Press ESC to unlock
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UnlockCursor();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            LockCursor();
        }
    }

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