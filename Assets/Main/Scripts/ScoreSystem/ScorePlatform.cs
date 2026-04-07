using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ScorePlatform : MonoBehaviour
{
    public enum State { Inactive, Active }

    [Header("Score")]
    [SerializeField] private int platformScoreValue = 10;
    [SerializeField] private PlatformType platformType;

    [Header("State")]
    [SerializeField] private bool activeOnStart = false;

    [Header("Visual")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color inactiveColor = Color.gray;

    private State _state;
    private Color _originalColor;

    private void Awake()
    {
        _originalColor = targetRenderer.material.color;
        Initialize();
    }

    public void Initialize()
    {
        SetState(activeOnStart ? State.Active : State.Inactive);
    }

    public void SetState(State state)
    {
        _state = state;
        targetRenderer.material.color =
            _state == State.Active ? _originalColor : inactiveColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_state != State.Active) return;

        var player = other.GetComponentInParent<PlayerPickController>();
        if (player == null) return;

        var ball = GetHeldBall(player);
        if (ball == null) return;

        ball.SetScore(platformScoreValue, platformType);
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponentInParent<PlayerPickController>();
        if (player == null) return;

        var ball = GetHeldBall(player);
        if (ball == null) return;

        ball.ResetScore();
    }

    private BallScore GetHeldBall(PlayerPickController player)
    {
        if (player.CurrentItem is not Component item) return null;
        return item.GetComponent<BallScore>();
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}