using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GoalPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goalText;

    private GoalService _goalService;
    private int RewardMultiplier => RewardSystem.Instance.Multiplier;

    public void Initialize(GoalService service)
    {
        _goalService = service;

        _goalService.OnGoalsGenerated += UpdateGoals;
        _goalService.OnGoalsUpdated += UpdateGoals;
    }

    private void OnDestroy()
    {
        if (_goalService == null) return;

        _goalService.OnGoalsGenerated -= UpdateGoals;
        _goalService.OnGoalsUpdated -= UpdateGoals;
    }

    private void UpdateGoals(List<GoalData> goals)
    {
        goalText.text = BuildGoalList(goals);
    }

    private string BuildGoalList(List<GoalData> goals)
    {
        string result = $"Goals (Reward x{RewardMultiplier}):\n";

        for (int i = 0; i < goals.Count; i++)
        {
            var g = goals[i];

            string text = $"Shoot from {g.RequiredPlatform}";

            string formatted = g.IsCompleted
                ? $"<color=#888888><s>{text}</s></color>"
                : $"<color=white>{text}</color>";

            string status = g.IsCompleted ? "/" : " ";

            result += $"{i + 1}. {status} {formatted}\n";
        }

        return result;
    }
}