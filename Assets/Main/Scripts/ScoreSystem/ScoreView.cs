using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Effect Settings")]
    [SerializeField] private float effectDuration = 0.2f;
    [SerializeField] private float pulseFrequency = 30f;
    [SerializeField] private float pulseAmplitude = 0.2f;
    [SerializeField] private float baseScale = 1f;

    public void UpdateScore(int value)
    {
        scoreText.text = $"Score: {value}";
    }

    public void PlayAddScoreEffect()
    {
        StopAllCoroutines();
        StartCoroutine(PulseEffect());
    }

    private IEnumerator PulseEffect()
    {
        float time = 0f;
        Vector3 originalScale = scoreText.transform.localScale;

        while (time < effectDuration)
        {
            time += Time.deltaTime;

            float wave = Mathf.Sin(time * pulseFrequency);
            float scaleFactor = baseScale + wave * pulseAmplitude;

            scoreText.transform.localScale = originalScale * scaleFactor;

            yield return null;
        }

        scoreText.transform.localScale = originalScale;
    }
}