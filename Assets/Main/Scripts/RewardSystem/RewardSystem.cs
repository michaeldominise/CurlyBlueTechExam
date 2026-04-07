using System;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    public static RewardSystem Instance { get; private set; }
    
    [SerializeField] private int multiplier = 3;

    private IScoreService _scoreService;
    private GoalService _goalService;
    
    public int Multiplier => multiplier;

    private void Awake() => Instance = this;

    public void Initialize(IScoreService scoreService, GoalService goalService)
    {
        _scoreService = scoreService;
        _goalService = goalService;

        _goalService.OnAllGoalsCompleted += HandleReward;
    }

    private void OnDestroy()
    {
        if (_goalService != null)
            _goalService.OnAllGoalsCompleted -= HandleReward;
    }

    private void HandleReward()
    {
        int totalScore = _scoreService.CurrentScore;
        int reward = totalScore * multiplier;
        
        _scoreService?.AddScore(totalScore);
        Debug.Log($"REWARD: {reward}");
    }
}