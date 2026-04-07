using UnityEngine;
using TMPro;

public class PlatformFeedbackPresenter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerPickController player;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Display")]
    [SerializeField] private string defaultText = "";

    private BallScore _currentBall;

    private void Update()
    {
        UpdateFeedback();
    }

    private void UpdateFeedback()
    {
        if (player == null || feedbackText == null)
            return;

        var ball = GetHeldBall();

        if (ball == null || ball.CurrentPlatformType == PlatformType.NoPlatform)
        {
            Clear();
            return;
        }

        Show(ball);
    }

    private BallScore GetHeldBall()
    {
        if (player.CurrentItem is not Component item)
            return null;

        return item.GetComponent<BallScore>();
    }

    private void Show(BallScore ball)
    {
        if (_currentBall == ball) return; // prevent unnecessary updates

        _currentBall = ball;

        feedbackText.gameObject.SetActive(true);
        feedbackText.text = $"{ball.CurrentPlatformType} (+{ball.CurrentScore})";
    }

    private void Clear()
    {
        if (_currentBall == null) return;

        _currentBall = null;

        feedbackText.text = defaultText;
        feedbackText.gameObject.SetActive(false);
    }
}