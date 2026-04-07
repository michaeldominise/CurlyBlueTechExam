using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerPickController : MonoBehaviour, IChargeProvider
{
    private enum State
    {
        Idle,
        Holding,
        Charging
    }

    [Header("Detection")]
    [SerializeField] private float pickDistance = 3f;
    [SerializeField] private LayerMask pickLayer;

    [Header("Hold")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float lerpSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Throw")]
    [SerializeField] private float minThrowForce = 2f;
    [SerializeField] private float maxThrowForce = 15f;
    [SerializeField] private float maxHoldTime = 2f;
    [SerializeField] private float throwAngleOffsetX = 10f; // degrees (upward)

    [Header("Debug")]
    [SerializeField] private bool enableDebug = false;

    private Camera _cam;

    private IPickable _currentItem;
    private Transform _currentTransform;
    private IHighlightable _currentHover;

    private float _holdTime;
    private State _state = State.Idle;
    
    public IPickable CurrentItem => _currentItem;

    // ------------------------
    // EVENTS (UI / SYSTEMS)
    // ------------------------
    public event Action<float> OnChargeChanged;
    public event Action OnChargeStart;
    public event Action OnChargeEnd;

    private void Awake() => _cam = Camera.main;

    private void Update()
    {
        DetectHover();
        UpdateCharging();
        HandleLerp();
    }

    // ------------------------
    // INPUT
    // ------------------------
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            HandlePress();

        if (context.canceled)
            HandleRelease();
    }

    private void HandlePress()
    {
        if (_state != State.Holding) return;

        _holdTime = 0f;
        SetState(State.Charging);

        OnChargeStart?.Invoke();
    }

    private void HandleRelease()
    {
        switch (_state)
        {
            case State.Idle:
                TryPick();
                break;

            case State.Charging:
                Throw();
                break;
        }
    }

    // ------------------------
    // STATE LOGIC
    // ------------------------
    private void UpdateCharging()
    {
        if (_state != State.Charging) return;

        _holdTime += Time.deltaTime;

        float normalized = GetNormalizedCharge();
        OnChargeChanged?.Invoke(normalized);
    }

    private void SetState(State newState)
    {
        _state = newState;

        if (enableDebug)
            Debug.Log($"State -> {newState}");
    }

    // ------------------------
    // PICK / THROW
    // ------------------------
    private void TryPick()
    {
        if (!TryRaycast(out var hit)) return;

        if (hit.collider.GetComponent<IPickable>() is not { } pickable)
            return;

        _currentItem = pickable;
        _currentTransform = hit.collider.transform;

        _currentItem.OnPick(holdPoint);
        SetState(State.Holding);

        if (enableDebug)
            Debug.Log($"Picked: {hit.collider.name}");
    }

    private void Throw()
    {
        float normalized = GetNormalizedCharge();
        float force = Mathf.Lerp(minThrowForce, maxThrowForce, normalized);

        Vector3 velocity = holdPoint.transform.forward * force;

        _currentItem?.OnRelease(velocity);

        OnChargeEnd?.Invoke();

        if (enableDebug)
            Debug.Log($"Thrown with force: {force}");

        _currentItem = null;
        _currentTransform = null;

        SetState(State.Idle);
    }

    // ------------------------
    // HOVER
    // ------------------------
    private void DetectHover()
    {
        if (!TryRaycast(out var hit))
        {
            ResetHover();
            return;
        }

        var highlight = hit.collider.GetComponent<IHighlightable>();

        if (highlight is null)
        {
            ResetHover();
            return;
        }

        if (highlight == _currentHover) return;

        _currentHover?.OnHoverExit();
        _currentHover = highlight;
        _currentHover.OnHoverEnter();
    }

    private void ResetHover()
    {
        if (_currentHover is null) return;

        _currentHover.OnHoverExit();
        _currentHover = null;
    }

    // ------------------------
    // LERP
    // ------------------------
    private void HandleLerp()
    {
        if (_currentTransform is null) return;

        _currentTransform.position = Vector3.Lerp(
            _currentTransform.position,
            holdPoint.position,
            lerpSpeed * Time.deltaTime);

        _currentTransform.rotation = Quaternion.Slerp(
            _currentTransform.rotation,
            holdPoint.rotation,
            rotationSpeed * Time.deltaTime);
    }

    // ------------------------
    // RAYCAST
    // ------------------------
    private Ray GetCenterRay() =>
        _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

    private bool TryRaycast(out RaycastHit hit)
    {
        var ray = GetCenterRay();

        if (enableDebug)
            Debug.DrawRay(ray.origin, ray.direction * pickDistance, Color.red);

        return Physics.SphereCast(ray, 1, out hit, pickDistance, pickLayer);
    }

    // ------------------------
    // HELPERS
    // ------------------------
    private float GetNormalizedCharge() => Mathf.Clamp01(_holdTime / maxHoldTime);
}