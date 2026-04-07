using UnityEngine;
using System.Collections.Generic;

public class ShootSequenceValidator : MonoBehaviour
{
    private GoalService _goalService;
    private IScoreService _scoreService;

    private readonly HashSet<GameObject> _validBalls = new();
    private readonly Dictionary<GameObject, PlatformType> _ballPlatformMap = new();

    public void Initialize(IScoreService scoreService, GoalService goalService)
    {
        _scoreService = scoreService;
        _goalService = goalService;
    }

    public void RegisterTrigger(ShootTrigger.TriggerType type, GameObject ballObj)
    {
        if (ballObj.GetComponent<BallScore>() is not { } ball)
            return;

        switch (type)
        {
            case ShootTrigger.TriggerType.Entry:
                _validBalls.Add(ballObj);
                _ballPlatformMap[ballObj] = ball.CurrentPlatformType;
                break;

            case ShootTrigger.TriggerType.Exit:
                HandleExit(ballObj, ball);
                break;
        }
    }

    private void HandleExit(GameObject ballObj, BallScore ball)
    {
        if (!_validBalls.Contains(ballObj)) return;

        _scoreService?.AddScore(ball.CurrentScore);

        if (_ballPlatformMap.TryGetValue(ballObj, out var platform))
        {
            _goalService?.RegisterGoal(platform);
        }

        ball.ResetScore();

        _validBalls.Remove(ballObj);
        _ballPlatformMap.Remove(ballObj);
    }
}