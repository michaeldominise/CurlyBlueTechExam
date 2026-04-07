using System;
using System.Collections.Generic;
using UnityEngine;

public class GoalService
{
    public event Action<List<GoalData>> OnGoalsGenerated;
    public event Action<List<GoalData>> OnGoalsUpdated;
    public event Action OnAllGoalsCompleted;

    private readonly List<GoalData> _goals = new();

    public void GenerateGoals(int count)
    {
        _goals.Clear();

        var values = Enum.GetValues(typeof(PlatformType));

        for (int i = 0; i < count; i++)
        {
            var random = (PlatformType)values.GetValue(UnityEngine.Random.Range(1, values.Length - 1));
            _goals.Add(new GoalData(random));
        }

        OnGoalsGenerated?.Invoke(_goals);
    }

    public void RegisterGoal(PlatformType platformUsed)
    {
        foreach (var goal in _goals)
        {
            if (goal.IsCompleted) continue;
            if (goal.RequiredPlatform != platformUsed) continue;
            
            goal.IsCompleted = true;
            break;
        }

        OnGoalsUpdated?.Invoke(_goals);

        if (AreAllCompleted())
        {
            OnAllGoalsCompleted?.Invoke();
            ResetAndGenerate(_goals.Count);
        }
    }

    private bool AreAllCompleted()
    {
        foreach (var g in _goals)
            if (!g.IsCompleted) return false;

        return true;
    }
    
    public void ResetAndGenerate(int count)
    {
        GenerateGoals(count);
    }
}