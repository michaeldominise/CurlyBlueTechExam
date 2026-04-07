using UnityEngine;
using TMPro;
using System;
using Random = System.Random;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private Vector2 spawnXPosOffset = new Vector2(-1, 1);
    [SerializeField] private Vector2 spawnYPosOffset = new Vector2(0, 1);
    
    [Header("Animation")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private float moveSpeed = 50f;

    [Header("Scale (One-shot pulse)")]
    [SerializeField] private float startScaleMultiplier = 1.4f;
    [SerializeField] private float scaleLerpSpeed = 8f;
    [SerializeField] private TextMeshProUGUI label;
    
    private float _time;
    private Vector3 _baseScale;
    private Vector3 _currentScale;
    private Color _startColor;

    private Action<ScorePopup> _returnToPool;

    private void Awake()
    {
        _baseScale = transform.localScale;
        _startColor = label.color;
    }

    public void Initialize(int value, Action<ScorePopup> returnAction)
    {
        label.text = $"+{value}";

        _time = 0f;
        _returnToPool = returnAction;

        _currentScale = _baseScale * startScaleMultiplier;
        transform.localScale = _currentScale;
        transform.localPosition = new Vector3
        (
            UnityEngine.Random.Range(spawnXPosOffset.x, spawnXPosOffset.y), 
            UnityEngine.Random.Range(spawnYPosOffset.x, spawnYPosOffset.y)
        );

        gameObject.SetActive(true);
    }

    private void Update()
    {
        _time += Time.deltaTime;

        AnimateMovement();
        AnimateScale();
        AnimateFade();

        if (_time >= duration)
        {
            _returnToPool?.Invoke(this);
        }
    }

    // ------------------------
    // ANIMATION
    // ------------------------
    private void AnimateMovement()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }

    private void AnimateScale()
    {
        _currentScale = Vector3.Lerp(_currentScale, _baseScale, Time.deltaTime * scaleLerpSpeed);
        transform.localScale = _currentScale;
    }

    private void AnimateFade()
    {
        float alpha = Mathf.Lerp(_startColor.a, 0f, _time / duration);

        var color = label.color;
        color.a = alpha;
        label.color = color;
    }
}