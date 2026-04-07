using UnityEngine;
using System.Collections.Generic;

public class ScorePopupSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform canvasTransform;
    [SerializeField] private ScorePopup popupPrefab;
    [SerializeField] private Transform popupAnchor;

    [Header("Pool")]
    [SerializeField] private int initialPoolSize = 10;

    private readonly Queue<ScorePopup> _pool = new();

    private IScoreService _service;
    private Camera _cam;

    // ------------------------
    // INIT
    // ------------------------

    public void Initialize(IScoreService service)
    {
        _service = service;
        _service.OnScoreAdded += OnScoreAdded;

        InitializePool();
        _cam = Camera.main;
    }

    private void OnDestroy()
    {
        if (_service != null)
            _service.OnScoreAdded -= OnScoreAdded;
    }

    // ------------------------
    // EVENT
    // ------------------------

    private void OnScoreAdded(int value)
    {
        Vector3 worldPos = popupAnchor != null
            ? popupAnchor.position
            : Vector3.zero;

        Spawn(value, worldPos);
    }

    // ------------------------
    // SPAWN
    // ------------------------

    private void Spawn(int value, Vector3 worldPosition)
    {
        var popup = GetFromPool();

        Vector3 screenPos = _cam.WorldToScreenPoint(worldPosition);
        popup.transform.position = screenPos;

        popup.Initialize(value, ReturnToPool);
    }

    // ------------------------
    // POOL
    // ------------------------

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            var popup = CreatePopup();
            ReturnToPool(popup);
        }
    }

    private ScorePopup GetFromPool()
    {
        if (_pool.Count > 0)
            return _pool.Dequeue();

        return CreatePopup();
    }

    private void ReturnToPool(ScorePopup popup)
    {
        popup.gameObject.SetActive(false);
        _pool.Enqueue(popup);
    }

    private ScorePopup CreatePopup()
    {
        var popup = Instantiate(popupPrefab, canvasTransform);
        popup.gameObject.SetActive(false);
        return popup;
    }
}