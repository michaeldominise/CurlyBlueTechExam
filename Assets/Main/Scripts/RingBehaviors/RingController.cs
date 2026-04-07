using UnityEngine;
using System.Collections.Generic;

public class RingController : MonoBehaviour
{
    [Header("Behaviors")]
    [SerializeField] private RingBehavior normalBehavior;
    [SerializeField] private List<RingBehavior> behaviors;

    [Header("Dependencies")]
    [SerializeField] private ShootSequenceValidator validator;
    [SerializeField] private PlayerPickController playerController;

    private RingBehavior _currentBehavior;
    private RingBehavior _lastBehavior;
    
    private void Start()
    {
        SetBehavior(normalBehavior);

        if (validator != null)
            validator.OnValidShot += HandleValidShot;

        if (playerController != null)
            playerController.OnBallReleased += HandleBallReleased;
    }

    private void OnDestroy()
    {
        if (validator != null)
            validator.OnValidShot -= HandleValidShot;

        if (playerController != null)
            playerController.OnBallReleased -= HandleBallReleased;
    }

    private void Update()
    {
        _currentBehavior?.Execute(transform, Time.deltaTime);
    }

    private void HandleValidShot()
    {
        var next = GetRandomBehavior();
        SetBehavior(next);
    }

    private void SetBehavior(RingBehavior newBehavior)
    {
        if (_currentBehavior != null)
            _currentBehavior.Cleanup(transform);

        _currentBehavior = newBehavior;

        _currentBehavior?.Initialize(transform);
    }

    private RingBehavior GetRandomBehavior()
    {
        if (behaviors == null || behaviors.Count == 0)
            return normalBehavior;

        RingBehavior next;

        do
        {
            next = behaviors[Random.Range(0, behaviors.Count)];
        }
        while (next == _lastBehavior && behaviors.Count > 1);

        _lastBehavior = next;
        return next;
    }
    
    private void HandleBallReleased()
    {
        _currentBehavior?.OnBallReleased(transform);
    }
}