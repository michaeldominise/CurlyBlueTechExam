public class GoalData
{
    public PlatformType RequiredPlatform;
    public bool IsCompleted;

    public GoalData(PlatformType type)
    {
        RequiredPlatform = type;
        IsCompleted = false;
    }
}