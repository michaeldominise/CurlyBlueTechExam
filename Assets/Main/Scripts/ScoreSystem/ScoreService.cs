using System;

public interface IScoreService
{
    event Action<int> OnScoreAdded;     // NEW (per score)
    event Action<int> OnScoreChanged;   // total score

    void AddScore(int value);
    
    int CurrentScore { get; }
}

public class ScoreService : IScoreService
{
    private readonly ScoreModel _model = new();

    public event Action<int> OnScoreAdded;
    public event Action<int> OnScoreChanged;
    
    public int CurrentScore => _model.Score;

    public void AddScore(int value)
    {
        _model.AddScore(value);

        OnScoreAdded?.Invoke(value);        
        OnScoreChanged?.Invoke(_model.Score); 
    }
}