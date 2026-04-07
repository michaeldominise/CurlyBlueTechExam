using UnityEngine;

public class ScorePresenter : MonoBehaviour
{
    [SerializeField] private ScoreView view;

    private IScoreService _service;

    public void Initialize(IScoreService service)
    {
        _service = service;

        _service.OnScoreChanged += OnScoreChanged;
    }

    private void OnDestroy()
    {
        if (_service == null) return;

        _service.OnScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int totalScore)
    {
        view.UpdateScore(totalScore);
        view.PlayAddScoreEffect();
        TriggerShake();
    }

    private void TriggerShake()
    {
        if (CameraShake.Instance == null) return;
        
        CameraShake.Instance.Shake();
    }
}