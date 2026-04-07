using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("Default Shake")]
    [SerializeField] private float defaultDuration = 0.2f;
    [SerializeField] private float defaultIntensity = 0.2f;
    [SerializeField] private float defaultFrequency = 25f;

    [Header("Control")]
    [SerializeField] private float shakeInterval = 0.1f;

    private float _shakeDuration;
    private float _shakeIntensity;
    private float _shakeFrequency;

    private float _time;
    private float _lastShakeTime;

    private Vector3 _originalLocalPos;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _originalLocalPos = transform.localPosition;
    }

    // ------------------------
    // PUBLIC API
    // ------------------------

    public void Shake()
    {
        Shake(defaultDuration, defaultIntensity, defaultFrequency);
    }

    public void Shake(float duration, float intensity, float frequency)
    {
        if (Time.time < _lastShakeTime + shakeInterval)
            return;

        _lastShakeTime = Time.time;

        _shakeDuration = duration;
        _shakeIntensity = intensity;
        _shakeFrequency = frequency;

        _time = 0f;
    }

    // ------------------------
    // UPDATE
    // ------------------------

    private void LateUpdate()
    {
        if (_shakeDuration <= 0f)
        {
            transform.localPosition = _originalLocalPos;
            return;
        }

        _time += Time.deltaTime;
        _shakeDuration -= Time.deltaTime;

        float x = Mathf.PerlinNoise(_time * _shakeFrequency, 0f) - 0.5f;
        float y = Mathf.PerlinNoise(0f, _time * _shakeFrequency) - 0.5f;

        Vector3 offset = new Vector3(x, y, 0f) * _shakeIntensity;

        transform.localPosition = _originalLocalPos + offset;
    }
}