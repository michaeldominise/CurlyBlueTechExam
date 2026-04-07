using UnityEngine;

[CreateAssetMenu(menuName = "Ring/Behavior/Move To Position")]
public class MoveToPositionBehavior : RingBehavior
{
    [Header("Movement")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 _originalPosition;
    private Vector3 _targetPosition;

    private Vector3 _currentTarget;
    private bool _isMoving;

    private const float POSITION_THRESHOLD = 0.05f;

    public override void Initialize(Transform ring)
    {
        _originalPosition = ring.position;
        _targetPosition = _originalPosition + offset;

        _currentTarget = _originalPosition;
        _isMoving = false;
    }

    public override void OnBallReleased(Transform ring)
    {
        // Determine current state based on position
        bool isAtOriginal = IsClose(ring.position, _originalPosition);

        // 🔥 Toggle target
        _currentTarget = isAtOriginal
            ? _targetPosition
            : _originalPosition;

        _isMoving = true;
    }

    public override void Execute(Transform ring, float deltaTime)
    {
        if (!_isMoving) return;

        ring.position = Vector3.Lerp(
            ring.position,
            _currentTarget,
            deltaTime * moveSpeed
        );

        if (IsClose(ring.position, _currentTarget))
        {
            ring.position = _currentTarget; // snap clean
            _isMoving = false;
        }
    }

    // ------------------------
    // HELPERS
    // ------------------------

    private bool IsClose(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b) < POSITION_THRESHOLD;
    }
}