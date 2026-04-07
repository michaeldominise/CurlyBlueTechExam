using UnityEngine;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private ScorePresenter scorePresenter;
    [SerializeField] private GoalPresenter goalPresenter;
    [SerializeField] private ShootSequenceValidator[] validators;
    [SerializeField] private ScorePopupSpawner popupSpawner;
    [SerializeField] private RewardSystem rewardSystem;

    private IScoreService _scoreService;
    private GoalService _goalService;

    private void Awake()
    {
        _scoreService = new ScoreService();
        _goalService = new GoalService();

        scorePresenter.Initialize(_scoreService);
        popupSpawner.Initialize(_scoreService); // ✅ NEW

        goalPresenter.Initialize(_goalService);
        rewardSystem.Initialize(_scoreService, _goalService);

        foreach (var v in validators)
            v.Initialize(_scoreService, _goalService);

        _goalService.GenerateGoals(3);
    }
}