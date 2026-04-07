using UnityEngine;

public class BallScore : MonoBehaviour
{
    [SerializeField] private int defaultScore = 2;

    public int CurrentScore { get; private set; }
    public PlatformType CurrentPlatformType { get; private set; }

    private void Awake()
    {
        ResetScore();
    }

    public void SetScore(int value, PlatformType platform)
    {
        CurrentScore = value;
        CurrentPlatformType = platform;
    }

    public void ResetScore()
    {
        CurrentScore = defaultScore;
        CurrentPlatformType = PlatformType.NoPlatform;
    }
}